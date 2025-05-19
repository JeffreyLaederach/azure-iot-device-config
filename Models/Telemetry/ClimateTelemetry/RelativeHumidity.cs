using System.Text.Json;

namespace IoT.Models.Telemetry.ClimateTelemetry
{
    internal class RelativeHumidity:Telemetry
    {
        // Public property to store the relative humidity value in percent
        public double Percent { get; set; }

        // Constructor that takes a relative humidity value as a parameter and assigns a percent value
        public RelativeHumidity(UnitsNet.RelativeHumidity relativeHumidity) { Percent = relativeHumidity.Percent; }

        public string GetTelemetryJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
