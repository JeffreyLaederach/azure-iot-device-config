using System.Text.Json.Nodes;
using System.Text.Json;
using IoT.Services.IoTDevices;
using System.Device.Gpio;

namespace IoT.Models.Telemetry.ClimateTelemetry
{
    internal class Climate:Telemetry
    {
        // Public properties to store telemetry data for altitude, pressure, relative humidity, and temperature
        public Altitude Altitude { get; set; }
        public Pressure Pressure { get; set; } 
        public RelativeHumidity RelativeHumidity { get; set; }
        public Temperature Temperature { get; set; }

        public Climate(Altitude altitude, Pressure pressure, RelativeHumidity relativeHumidity, Temperature temperature)
        {
            Altitude = altitude;
            Pressure = pressure;
            RelativeHumidity = relativeHumidity;
            Temperature = temperature;
        }

        public string GetTelemetryJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // The format of the strings depends on the unit system (Metric or Imperial) provided as a parameter
        public string[] GetLCDFormattedString(IoTDevice.UnitSystem unitSystem)
        {

            string[] output = new string[2];

            if (unitSystem == IoTDevice.UnitSystem.Metric)
            {
                output[0] = $"T:{Temperature.DegreesCelsius:0.#}C P:{Pressure.Pascals:#.#}Pa";
                output[1] = $"H:{RelativeHumidity.Percent:#.#}% A:{Altitude.Meters:#}m";
            }
            else
            {
                output[0] = $"T:{Temperature.DegreesFahrenheit:0.#}F P:{Pressure.InchesOfMercury:#.#}Hg";
                output[1] = $"H:{RelativeHumidity.Percent:#.#}% A:{Altitude.Feet:#}ft";
            }

            return output;
        }
    }
}
