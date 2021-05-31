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
using MyFoodDoc.FirebaseClient.Abstractions;
using Newtonsoft.Json;

namespace MyFoodDoc.FirebaseClient.Clients
{
    public class FirebaseClient : IFirebaseClient
    {
        private const int MAX_BATCH_SIZE = 500;

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

                var batches = messages.Count / MAX_BATCH_SIZE + (messages.Count % MAX_BATCH_SIZE > 0 ? 1 : 0);

                for (int i = 0; i < batches; i++)
                {
                    var batchMessages = messages.Skip(i * MAX_BATCH_SIZE).Take(MAX_BATCH_SIZE);

                    var result = await messaging.SendAllAsync(batchMessages, cancellationToken);

                    _logger.LogInformation($"Notifications batch count={batchMessages.Count()}. Success count={result.SuccessCount}, Error count={result.FailureCount}");
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.StackTrace);

                throw e;
            }
        }
    }
}
