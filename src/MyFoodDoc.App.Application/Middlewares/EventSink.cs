using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Middlewares
{
    public class EventSink : IEventSink
    {
        private readonly ILogger<EventSink> _logger;
        public EventSink(ILogger<EventSink> logger)
        {
            _logger = logger;
        }

        public Task PersistAsync(Event _event)
        {
            switch (_event.EventType)
            {
                case EventTypes.Error:
                case EventTypes.Failure:
                    _logger.LogError(_event.ToString());
                    break;
                case EventTypes.Success:
                case EventTypes.Information:

                default:
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
