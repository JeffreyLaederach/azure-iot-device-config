using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Services.Logging
{
    internal class Logger
    {
        public Logger(){
            
        }

        public void Log(String message)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = factory.CreateLogger<Program>();
            logger.LogInformation(
                "Timestamp: {time} UTC, " +
                "DeviceID: {deviceId}, " +
                "DeviceModelID: {DeviceModelID}, " +
                "ErrorMessage: {message}",
                DateTime.UtcNow, "DEVICE_ID", "dtmi:company:climate_sensor_array; 1.0", message);
        }
    }
}
