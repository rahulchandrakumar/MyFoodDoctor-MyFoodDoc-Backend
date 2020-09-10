using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MyFoodDoc.Functions.Firebase
{
    public class FirebaseClient : IFirebaseClient
    {
        private readonly ILogger<FirebaseClient> _logger;
        private FirebaseClientOptions _settings;

        public FirebaseClient(IOptions<FirebaseClientOptions> settings, ILogger<FirebaseClient> logger)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        private FirebaseMessaging FirebaseMessagingInstance
        {
            get
            {
                if (FirebaseMessaging.DefaultInstance == null)
                {
                    //Azure key vault adds extra slash before new line
                    _settings.PrivateKey = _settings.PrivateKey.Replace("\\n", "\n");

                    string jsonSettings = JsonConvert.SerializeObject(_settings);

                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(jsonSettings)
                    });
                }

                return FirebaseMessaging.DefaultInstance;
            }
        }

        public async Task<bool> SendAsync(IEnumerable<FirebaseNotification> notifications, CancellationToken cancellationToken)
        {
            var messages = notifications.Select(x => new Message()
            {
                Notification = new Notification
                {
                    Title = x.Title,
                    Body = x.Body
                },
                Token = x.DeviceToken
            }).ToList();
            
            try
            {
                var messaging = FirebaseMessagingInstance;

                var result = await messaging.SendAllAsync(messages, cancellationToken);

                _logger.LogInformation($"Notifications batch count={messages.Count()}. Success count={result.SuccessCount}, Error count={result.FailureCount}");

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.StackTrace);

                return false;
            }
        }
    }
}
