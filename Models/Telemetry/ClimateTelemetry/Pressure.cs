using System.Text.Json;

// Define the Pressure class within the ClimateTelemetry namespace, inheriting from the Telemetry class
namespace IoT.Models.Telemetry.ClimateTelemetry
{
    internal class Pressure:Telemetry
    {
        // Public properties to store pressure values in Pascals and Inches of Mercury
        public double Pascals { get; set; }
        public double InchesOfMercury { get; set; }

        public Pressure(UnitsNet.Pressure pressure) 
        {
            Pascals = pressure.Pascals;
            InchesOfMercury = pressure.InchesOfMercury;
        }

        public string GetTelemetryJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
