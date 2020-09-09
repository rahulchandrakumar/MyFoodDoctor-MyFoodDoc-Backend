using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Functions.Firebase;

namespace MyFoodDoc.Functions
{
    public class PushNotifications
    {
        private readonly IApplicationContext _context;
        private readonly IFirebaseClient _firebaseClient;

        public PushNotifications(IApplicationContext context, IFirebaseClient firebaseClient)
        {
            _context = context;
            _firebaseClient = firebaseClient;
        }

        [FunctionName("PushNotificationsEmptyDiary")]
        public async Task RunAsync(
            [TimerTrigger("0 0 16 * * *" /*"%TimerInterval%"*/, RunOnStartup = false)]
            TimerInfo myTimer,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("EmptyDiaryPushNotifications. Start");

            var usersWithPushNotificationsEnabled = await _context.Users.Where(x => x.PushNotificationsEnabled && !string.IsNullOrEmpty(x.DeviceToken)).ToListAsync(cancellationToken);

            log.LogInformation($"Users with push notifications enabled: {usersWithPushNotificationsEnabled.Count()}");

            if (usersWithPushNotificationsEnabled.Any())
            {
                var dateToCheck = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                var notifications = new List<FirebaseNotification>();

                foreach (var user in usersWithPushNotificationsEnabled)
                {
                    var diaryRecords = await _context.Meals.Where(x => x.UserId == user.Id && x.Date >= dateToCheck.AddDays(-2)).ToListAsync(cancellationToken);

                    if (!diaryRecords.Any())
                    {
                        notifications.Add(new FirebaseNotification()
                        {
                            //Title = "My Food Doctor",
                            Body =
                                "Bleib am Ball und führe Protokoll – es lohnt sich. Die App analysiert deine Daten und gibt dir dann individuelle Empfehlungen zur Optimierung.",
                            DeviceToken = user.DeviceToken
                        });
                    }
                    else if (!diaryRecords.Any(x => x.Date == dateToCheck))
                    {
                        var bodies = new string[]
                        {
                            "Denke daran, dein Essen heute zu dokumentieren, nur so erhältst du passende Methoden, die dich unterstützen, deine Ziele zu erreichen. Du kannst die Mahlzeiten auch nachtragen.",
                            "Was hast du heute gegessen? Trage es ein! Notiere möglichst zum Zeitpunkt des Verzehrs und nicht erst später. Häufig weiß man abends nicht mehr, was man tagsüber gegessen hat."
                        };

                        Random rand = new Random();
                        int index = rand.Next(bodies.Length);

                        notifications.Add(new FirebaseNotification()
                        {
                            //Title = "My Food Doctor",
                            Body = bodies[index],
                            DeviceToken = user.DeviceToken
                        });
                    }
                }

                log.LogInformation($"Notifications to send: {notifications.Count()}");


                if (notifications.Any())
                {
                    var result = await _firebaseClient.SendAsync(notifications, cancellationToken);
                }
            }

            log.LogInformation("EmptyDiaryPushNotifications. End");
        }
    }
}
