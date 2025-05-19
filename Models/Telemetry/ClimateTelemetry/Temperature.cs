using System.Text.Json;

namespace IoT.Models.Telemetry.ClimateTelemetry
{
    internal class Temperature:Telemetry
    {
        // Public properties to store temperature values in Celsius and Fahrenheit
        public double DegreesCelsius { get; set; }
        public double DegreesFahrenheit { get; set; }

        public Temperature(UnitsNet.Temperature temperature) 
        {  
            DegreesCelsius = temperature.DegreesCelsius; 
            DegreesFahrenheit = temperature.DegreesFahrenheit;
        }

        public string GetTelemetryJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
