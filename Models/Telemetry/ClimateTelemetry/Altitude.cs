using System.Text.Json;

namespace IoT.Models.Telemetry.ClimateTelemetry
{
    internal class Altitude:Telemetry
    {
        // Public properties to store altitude values in Meters and Feet
        public double Meters { get; set; }
        public double Feet { get; set; }

        // Constructor that takes an altitude value as a parameter of length in Meters or Feet
        public Altitude(UnitsNet.Length altitude)
        {
            Meters = altitude.Meters;
            Feet = altitude.Feet;
        }

        public string GetTelemetryJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
