using IoT.Models.Telemetry.ClimateTelemetry;

namespace IoT.Services.Peripherals.ClimateSensor
{
    internal interface IClimateSensor
    {
        public Climate GetClimateReading();
        public Altitude GetAltitudeReading();
        public Pressure GetPressureReading();
        public RelativeHumidity GetHumidityReading();
        public Temperature GetTemperatureReading();
        public void Dispose();
    }
}
