using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Cloud.PubSub.V1;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Functions.Firebase;

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

        [FunctionName("PushNotifications")]
        public async Task RunAsync(
            [TimerTrigger("0 0 16 * * *" /*"%TimerInterval%"*/, RunOnStartup = true)]
            TimerInfo myTimer,
            ILogger log,
            CancellationToken cancellationToken)
        {
            /*
            try
            {
                        
            var _credentialsJson = "google.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _credentialsJson));

            GoogleCredential credentialsPlay;
            using (var key = new FileStream(_credentialsJson, FileMode.Open, FileAccess.Read))
                credentialsPlay = GoogleCredential.FromStream(key); //.CreateScoped("");

            var publisherService = new AndroidPublisherService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentialsPlay
            });

            var request1 = publisherService.Purchases.Subscriptions.Get(
                "de.medicum.myfooddoc", 
                "medicum_monthly",
                "cilljkojdcjcieibgfafnpje.AO-J1Owt468vgApydHWihk9bcjT0XRGQgAUMi7T0p2Up16yluZ5BbEQJ2J53tA8po3CFUNp4_VI2cKTaTcJaGfBayhur3rgVDQC-5LQraMirTCG5WCHEJjnt8rsLounx81tnXLRrnsn9");
            var response1 = request1.Execute();
            
            }
            catch (


                Exception
                e)
            {
                Console.WriteLine(e);
                throw;
            }

            */

            log.LogInformation("PushNotifications. Start");

            var usersWithPushNotificationsEnabled = await _context.Users.Where(x => x.PushNotificationsEnabled && !string.IsNullOrEmpty(x.DeviceToken)).ToListAsync(cancellationToken);

            log.LogInformation($"Users with push notifications enabled: {usersWithPushNotificationsEnabled.Count()}");

            if (usersWithPushNotificationsEnabled.Any())
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

                    if (user.Created > dateToCheck.AddDays(-7) && !diaryRecords.Any())
                    {
                        notifications.Add(new FirebaseNotification()
                        {
                            Body =
                                "Was hast du heute gegessen? Überprüfe deine Methoden und starte heute wieder mit deinem Ernährungstagebuch!",
                            DeviceToken = user.DeviceToken
                        });
                    }
                    else if (user.Created > dateToCheck.AddDays(-5) && !diaryRecords.Any(x => x.Date == dateToCheck.AddDays(-5)))
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
                            x.UserId == user.Id && !string.IsNullOrEmpty(x.TargetAnswerCode) && x.Created > DateTime.Now.AddDays(-_statisticsPeriod)).OrderByDescending(x => x.Created).SingleOrDefaultAsync(cancellationToken);

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
                    var result = await _firebaseClient.SendAsync(notifications, cancellationToken);
                }
            }

            log.LogInformation("PushNotifications. End");
        }
    }
}
