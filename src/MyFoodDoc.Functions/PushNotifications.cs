using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.FirebaseClient.Abstractions;
using MyFoodDoc.FirebaseClient.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MyFoodDoc.Functions
{
    public class PushNotifications
    {
        private readonly IApplicationContext _context;
        private readonly IFirebaseClient _firebaseClient;
        private readonly int _statisticsPeriod;
        private readonly int _statisticsMinimumDays;

        public PushNotifications(IApplicationContext context, IFirebaseClient firebaseClient, IOptions<StatisticsOptions> statisticsOptions)
        {
            _context = context;
            _firebaseClient = firebaseClient;
            _statisticsPeriod = statisticsOptions.Value.Period > 0 ? statisticsOptions.Value.Period : 7;
            _statisticsMinimumDays = statisticsOptions.Value.MinimumDays > 0 ? statisticsOptions.Value.MinimumDays : 3;
        }

        [FunctionName("TimerPushNotifications")]
        public async Task RunTimerPushNotificationsAsync(
            [TimerTrigger("0 */1 * * * *" /*"%TimerInterval%"*/, RunOnStartup = true)]
            TimerInfo myTimer,
            ILogger log,
            CancellationToken cancellationToken)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }

            log.LogInformation($"TimerPushNotifications executed at: {DateTime.Now}");

            var onDate = DateTime.Now;

            var expiredTimers = await _context.UserTimer
                .Include(x => x.User)
                .Where(x => x.ExpirationDate <= onDate
                && x.User.PushNotificationsEnabled
                && !string.IsNullOrEmpty(x.User.DeviceToken))
                .ToListAsync(cancellationToken);

            log.LogInformation(
                $"Users with push notifications enabled: {expiredTimers?.Count()}");

            if (expiredTimers?.Any() == true)
            {
                var result = false;

                var notifications = expiredTimers.Select(x => new FirebaseNotification()
                {
                    Body = "Achtung, die Timer-Zeit ist abgelaufen, schau in die App für den nächsten Schritt!",
                    DeviceToken = x.User.DeviceToken
                });

                log.LogInformation($"Notifications to send: {notifications.Count()}");

                try
                {
                    result = await _firebaseClient.SendAsync(notifications, cancellationToken);
                }
                catch (Exception e)
                {
                    log.LogError(e.Message + e.StackTrace);
                }


                if (result)
                {
                    var userMethodsToUpdate = new List<UserMethod>();

                    foreach (var userTimer in expiredTimers)
                    {
                        var userMethod =
                            (await _context.UserMethods
                                .Where(x => x.UserId == userTimer.UserId && x.MethodId == userTimer.MethodId)
                                .ToListAsync(cancellationToken)).OrderBy(x => x.Created).LastOrDefault();

                        if (userMethod != null)
                        {
                            userMethod.Answer = false;

                            userMethodsToUpdate.Add(userMethod);
                        }
                    }

                    _context.UserTimer.RemoveRange(expiredTimers);

                    _context.UserMethods.UpdateRange(userMethodsToUpdate);

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            log.LogInformation("TimerPushNotifications. End");
        }

        [FunctionName("DiaryPushNotifications")]
        public async Task RunDiaryPushNotificationsAsync(
            [TimerTrigger("0 0 16 * * *" /*"%TimerInterval%"*/, RunOnStartup = false)]
            TimerInfo myTimer,
            ILogger log,
            CancellationToken cancellationToken)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }

            log.LogInformation($"DiaryPushNotifications executed at: {DateTime.Now}");

            var usersWithPushNotificationsEnabled = await _context.Users.Where(x => x.PushNotificationsEnabled && !string.IsNullOrEmpty(x.DeviceToken)).ToListAsync(cancellationToken);

            log.LogInformation($"Users with push notifications enabled: {usersWithPushNotificationsEnabled.Count()}");

            if (usersWithPushNotificationsEnabled?.Any() == true)
            {
                var dateToCheck = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                var notifications = new List<FirebaseNotification>();

                foreach (var user in usersWithPushNotificationsEnabled)
                {
                    var userCreatedDate = new DateTime(user.Created.Year, user.Created.Month, user.Created.Day);

                    var diaryRecords = await _context.Meals.Where(x => x.UserId == user.Id && x.Date >= dateToCheck.AddDays(-7)).ToListAsync(cancellationToken);

                    var daysRecorded = diaryRecords.Where(x => x.Date > dateToCheck.AddDays(-_statisticsPeriod))
                        .Select(x => x.Date)
                        .Distinct()
                        .Count();

                    var isFirstTargetsEvaluation = !(await _context.UserTargets.AnyAsync(x =>
                        x.UserId == user.Id && !string.IsNullOrEmpty(x.TargetAnswerCode), cancellationToken));

                    if (userCreatedDate <= dateToCheck.AddDays(-7) && !diaryRecords.Any())
                    {
                        notifications.Add(new FirebaseNotification()
                        {
                            Body =
                                "Was hast du heute gegessen? Überprüfe deine Methoden und starte heute wieder mit deinem Ernährungstagebuch!",
                            DeviceToken = user.DeviceToken
                        });
                    }
                    else if (userCreatedDate <= dateToCheck.AddDays(-5) && !diaryRecords.Any(x => x.Date == dateToCheck.AddDays(-5)))
                    {
                        notifications.Add(new FirebaseNotification()
                        {
                            Body =
                                "Bleib am Ball und führe wieder Protokoll – es lohnt sich.",
                            DeviceToken = user.DeviceToken
                        });
                    }
                    else if (isFirstTargetsEvaluation)
                    {
                        if (userCreatedDate <= dateToCheck.AddDays(-_statisticsPeriod) && diaryRecords.Where(x => x.Date >= dateToCheck.AddDays(-_statisticsPeriod) && x.Date < dateToCheck)
                            .Select(x => x.Date)
                            .Distinct()
                            .Count() >= _statisticsMinimumDays)
                        {
                            notifications.Add(new FirebaseNotification()
                            {
                                Body =
                                    "Geschafft! Heute ist es soweit: Du kannst deine auf dich zugeschnittene Methode abrufen!",
                                DeviceToken = user.DeviceToken
                            });
                        }
                        else if (diaryRecords.Any(x => x.Date == dateToCheck))
                        {
                            if (!diaryRecords.Any(x => x.Date < dateToCheck))
                            {
                                notifications.Add(new FirebaseNotification()
                                {
                                    Body =
                                        "Großartig, dass du deine Mahlzeiten heute protokolliert hast. Weiter so!",
                                    DeviceToken = user.DeviceToken
                                });
                            }
                            else if (daysRecorded >= _statisticsMinimumDays)
                            {
                                if (userCreatedDate == dateToCheck.AddDays(-_statisticsPeriod + 1))
                                {
                                    notifications.Add(new FirebaseNotification()
                                    {
                                        Body =
                                            "Fast geschafft! In Kürze erhältst du deine Methoden auf Basis deiner Ernährungsgewohnheiten.",
                                        DeviceToken = user.DeviceToken
                                    });
                                }
                                else
                                {
                                    if (daysRecorded == _statisticsMinimumDays)
                                    {
                                        notifications.Add(new FirebaseNotification()
                                        {
                                            Body =
                                                $"Klasse, du führst den {daysRecorded}. Tag dein Tagebuch. Bleib dabei und erhalte in wenigen Tage deine persönliche Auswertung!",
                                            DeviceToken = user.DeviceToken
                                        });
                                    }
                                    else
                                    {
                                        notifications.Add(new FirebaseNotification()
                                        {
                                            Body =
                                                "Prima, mach weiter mit dem Protokoll - je mehr Daten wir haben desto besser.",
                                            DeviceToken = user.DeviceToken
                                        });
                                    }
                                }
                            }
                            else
                            {
                                notifications.Add(new FirebaseNotification()
                                {
                                    Body =
                                        "Prima, mach weiter mit dem Protokoll - je mehr Daten wir haben desto besser.",
                                    DeviceToken = user.DeviceToken
                                });
                            }
                        }
                        else
                        {
                            if (daysRecorded >= _statisticsMinimumDays &&
                                userCreatedDate == dateToCheck.AddDays(-_statisticsPeriod + 1))
                            {
                                notifications.Add(new FirebaseNotification()
                                {
                                    Body =
                                        "Fast geschafft! In Kürze erhältst du deine Methoden auf Basis deiner Ernährungsgewohnheiten.",
                                    DeviceToken = user.DeviceToken
                                });
                            }
                            else
                            {
                                notifications.Add(new FirebaseNotification()
                                {
                                    Body =
                                        "Du hast heute noch nichts protokolliert! Macht nichts: Trag einfach alles nach!",
                                    DeviceToken = user.DeviceToken
                                });
                            }
                        }
                    }
                    else
                    {
                        var lastTargetActivated = await _context.UserTargets.Where(x =>
                            x.UserId == user.Id && !string.IsNullOrEmpty(x.TargetAnswerCode) && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderBy(x => x.Created).LastOrDefaultAsync(cancellationToken);

                        if (lastTargetActivated != null)
                        {
                            var lastTargetActivatedDate = new DateTime(lastTargetActivated.Created.Year, lastTargetActivated.Created.Month, lastTargetActivated.Created.Day);

                            if (lastTargetActivatedDate == dateToCheck.AddDays(-_statisticsPeriod + 1))
                            {
                                notifications.Add(new FirebaseNotification()
                                {
                                    Body =
                                        "Setzt du deine Methode um? Weiter so! Wenn nicht, dann starte heute! Es ist nicht zu spät!",
                                    DeviceToken = user.DeviceToken
                                });
                            }

                            daysRecorded = diaryRecords.Where(x => x.Date >= lastTargetActivatedDate)
                                .Select(x => x.Date)
                                .Distinct()
                                .Count();
                        }

                        if (daysRecorded == _statisticsPeriod)
                        {
                            notifications.Add(new FirebaseNotification()
                            {
                                Body =
                                    "Du kannst stolz auf dich sein, dass du jetzt schon so lange regelmäßig Protokoll führst. Weiter so!",
                                DeviceToken = user.DeviceToken
                            });
                        }

                        if (!diaryRecords.Any(x => x.Date == dateToCheck))
                        {
                            notifications.Add(new FirebaseNotification()
                            {
                                Body =
                                    "Du hast heute noch nichts protokolliert! Macht nichts: Trag einfach alles nach!",
                                DeviceToken = user.DeviceToken
                            });
                        }
                    }
                }

                log.LogInformation($"Notifications to send: {notifications.Count()}");

                if (notifications.Any())
                {
                    try
                    {
                        var result = await _firebaseClient.SendAsync(notifications, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        log.LogError(e.Message + e.StackTrace);
                    }
                }
            }

            log.LogInformation("DiaryPushNotifications. End");
        }
    }
}
