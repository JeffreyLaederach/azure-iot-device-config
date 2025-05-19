using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using IoT.Models.Telemetry.ClimateTelemetry;
using System;
using System.Device.I2c;

namespace IoT.Services.Peripherals.ClimateSensor
{
    internal class ClimateSensor:IClimateSensor
    {
        private Bme280 _climateSensor;

        // Constructor to initialize the climate sensor
        public ClimateSensor() 
        {
            // Initializing climate sensor
            I2cConnectionSettings sensorSettings = new I2cConnectionSettings(1, Bme280.DefaultI2cAddress);
            I2cDevice sensorI2cDevice = I2cDevice.Create(sensorSettings);
            _climateSensor = new Bme280(sensorI2cDevice);
        }

        public void Dispose()
        {
            _climateSensor.Dispose();
        }

        public Climate GetClimateReading()
        {
            _climateSensor.SetPowerMode(Bmx280PowerMode.Forced);

            Altitude altitudeTelemetry = GetAltitudeReading();
            Pressure pressureTelemetry = GetPressureReading();
            Temperature temperatureTelemetry = GetTemperatureReading();
            RelativeHumidity relativeHumidityTelemetry = GetHumidityReading();

            // Return a new Climate object with all the telemetry data
            return new Climate(altitudeTelemetry, pressureTelemetry, relativeHumidityTelemetry, temperatureTelemetry);
        }

        // Methods to get each reading from sensor which each reading stored in its own new object
        public Altitude GetAltitudeReading()
        {
            UnitsNet.Length altitude;
            _climateSensor.TryReadAltitude(out altitude);
            return new Altitude(altitude);
        }

        public Pressure GetPressureReading()
        {
            UnitsNet.Pressure pressure;
            _climateSensor.TryReadPressure(out pressure);
            return new Pressure(pressure);
        }

        public RelativeHumidity GetHumidityReading()
        {
            UnitsNet.RelativeHumidity relativeHumidity;
            _climateSensor.TryReadHumidity(out relativeHumidity);
            return new RelativeHumidity(relativeHumidity);
        }

        public Temperature GetTemperatureReading()
        {
            UnitsNet.Temperature temperature;
            _climateSensor.TryReadTemperature(out temperature);
            return new Temperature(temperature);
        }
    }
}
