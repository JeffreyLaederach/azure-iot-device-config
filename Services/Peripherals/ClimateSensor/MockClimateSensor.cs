using IoT.Models.Telemetry.ClimateTelemetry;

namespace IoT.Services.Peripherals.ClimateSensor
{
    internal class MockClimateSensor:IClimateSensor
    {
        private Random _random;

        public MockClimateSensor() 
        {
            _random = new Random(); 
            
        }
        
        public void Dispose()
        {
            Console.WriteLine("Disposing Climate Sensor");
        }

        public Climate GetClimateReading()
        {
            Altitude altitudeTelemetry = GetAltitudeReading();
            Pressure pressureTelemetry = GetPressureReading();
            Temperature temperatureTelemetry = GetTemperatureReading();
            RelativeHumidity relativeHumidityTelemetry = GetHumidityReading();

            // Return new Climate object with all telemtry readings
            return new Climate(altitudeTelemetry, pressureTelemetry, relativeHumidityTelemetry, temperatureTelemetry);
        }

        // Methods to get each reading from sensor which each stored in its own new object
        public Altitude GetAltitudeReading()
        {
            double mockAltitude = _random.NextDouble() * 1000;
            UnitsNet.Length altitude = new UnitsNet.Length(mockAltitude,UnitsNet.Units.LengthUnit.Foot);
            return new Altitude(altitude);
        }

        public Pressure GetPressureReading()
        {
            double mockPressure = _random.NextDouble() * 50;
            UnitsNet.Pressure pressure = new UnitsNet.Pressure(mockPressure,UnitsNet.Units.PressureUnit.PoundForcePerSquareInch);
            return new Pressure(pressure);
        }

        public RelativeHumidity GetHumidityReading()
        {
            double mockPressure = _random.NextDouble();
            UnitsNet.RelativeHumidity relativeHumidity = new UnitsNet.RelativeHumidity(mockPressure,UnitsNet.Units.RelativeHumidityUnit.Percent);
            return new RelativeHumidity(relativeHumidity);
        }

        public Temperature GetTemperatureReading()
        {
            double mockPressure = _random.NextDouble()*100;
            UnitsNet.Temperature temperature = new UnitsNet.Temperature(mockPressure,UnitsNet.Units.TemperatureUnit.DegreeFahrenheit);
            return new Temperature(temperature);
        }
    }
}
