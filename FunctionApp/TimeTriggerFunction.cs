using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class TimeTriggerFunction
    {
        private readonly ILogger _logger;

        // Constructor to initialize the logger
        public TimeTriggerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimeTriggerFunction>();
        }

        /// <summary>
        /// Function triggered by a timer every 15 seconds.
        /// The TimerTrigger attribute uses a CRON expression to define the schedule.
        /// CRON expression format: "second minute hour day month day-of-week"
        /// Example: "*/15 * * * * *" means every 15 seconds.
        /// </summary>
        /// <param name="myTimer">Timer information provided by the Azure Functions runtime.</param>
        [Function("TimeTriggerFunction")]
        public void Run([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer)
        {
            // Log the current time when the function is executed
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Log the next scheduled time if available
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
