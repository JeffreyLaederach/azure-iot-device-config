using IoT.Models.Telemetry.ClimateTelemetry;
using IoT.Services.Peripherals.ClimateSensor;
using IoT.Services.Peripherals.LCD;
using IoT.Services.Logging;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Iot.Device.Common;

namespace IoT.Services.IoTDevices.ClimateSensorArray
{
    class ClimateSensorArrayV1 : IoTDevice
    {
        private DeviceClient _deviceClient;
        private IoTDevice.UnitSystem _unitSystem;
        private CancellationTokenSource _cts;
        private int _measurementInterval;

        // Hardware stack
        private IClimateSensor _climateSensor;
        private ILCD _lcd;

        // Logger
        private Logger _logger = new Logger();

        // Constructor to initialize the climate sensor array
        // Parameters:
        //   deviceClient: The client to communicate with the IoT hub
        //   cancellationTokenSource: The token source used to cancel tasks
        //   unitSystem: The unit system (metric or imperial)
        //   measurementInterval: The interval between measurements
        //   mockData: Boolean flag to use mock data or real data

        public ClimateSensorArrayV1(DeviceClient deviceClient, CancellationTokenSource cancellationTokenSource, IoTDevice.UnitSystem unitSystem, int measurementInterval, bool mockData)
        {
            _deviceClient = deviceClient;
            _cts = cancellationTokenSource;
            _unitSystem = unitSystem;
            _measurementInterval = measurementInterval;

            // Initializing climate sensor and lcd
            if (mockData)
            {
                _climateSensor = new MockClimateSensor();
                _lcd = new MockLCD();
            }
            else
            {
                _climateSensor = new ClimateSensor();
                _lcd = new LCD();
            }
        }

        public async Task Init()
        {
            Console.WriteLine("Climate Sensor Array Initializing");
            _logger.Log("Climate Sensor Array Initializing");

            // Add event handler to swap between imperial and metric
            await _deviceClient.SetMethodHandlerAsync("switch-units", HandleSwapUnitSystem, "lcd-screen");
            Console.WriteLine("Climate Sensor Array Initialized");
            _logger.Log("Climate Sensor Array Initialized");
            while (!_cts.Token.IsCancellationRequested)
            {
              
                    await SendClimateTelemetry();
                    await Task.Delay(_measurementInterval);
            }
            Console.WriteLine("Climate Sensor Operations Canceled");
            _logger.Log("Climate Sensor Operations Canceled");
        }

        public async Task Disconnect()
        {
            // Tearing down device
            Console.WriteLine("Climate Sensor Array Disconnecting");
            _logger.Log("Climate Sensor Array Disconnecting");
            _lcd.Clear();
            _cts.Cancel();
            await _deviceClient.CloseAsync();
            _deviceClient.Dispose();
            _climateSensor.Dispose();
            _lcd.Dispose();
            _cts.Dispose();
            Console.WriteLine("Climate Sensor Array Disconnected");
            _logger.Log("Climate Sensor Array Disconnected");
        }

        // Event handler for switching unit system
        // Parameters:
        //   methodRequest: The method request that triggered the event
        //   userContext: User context object

        private Task<MethodResponse> HandleSwapUnitSystem(MethodRequest methodRequest, object userContext)
        {
            try
            {
                SwapLCDUnitSystem();
                return Task.FromResult(new MethodResponse((int)IoTDevice.StatusCode.Completed));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new MethodResponse((int)IoTDevice.StatusCode.BadRequest));
            }
        }

        private async Task SendClimateTelemetry()
        {
            // Getting climate reading
            Climate climateTelemetry = _climateSensor.GetClimateReading();
            string climateTelemetryString = climateTelemetry.GetTelemetryJson();

            // Encoding for transport
            Message msg = new Message(Encoding.ASCII.GetBytes(climateTelemetryString));
            msg.ContentType = "application/json";
            msg.ContentEncoding = "utf-8";

            // IO
            await _deviceClient.SendEventAsync(msg, _cts.Token);
            WriteToLCD(climateTelemetry);
            Console.WriteLine($"Telemetry sent: {climateTelemetryString}");
            _logger.Log($"Telemetry sent: {climateTelemetryString}");
        }

        private void SwapLCDUnitSystem()
        {
            if (_unitSystem == IoTDevice.UnitSystem.Metric)
            {
                _unitSystem = IoTDevice.UnitSystem.Imperial;
                Console.WriteLine("Swapping LCD Unit System to Imperial");
                _logger.Log("Swapping LCD Unit System to Imperial");
            }
            else
            {
                _unitSystem = IoTDevice.UnitSystem.Metric;
                Console.WriteLine("Swapping LCD Unit System to Metric");
                _logger.Log("Swapping LCD Unit System to Metric");
            }

            Climate climateTelemetry = _climateSensor.GetClimateReading();
            WriteToLCD(climateTelemetry);
        }

        // Method to write climate data to the LCD
        // Parameters:
        //   climateTelemetry: The climate telemetry data to display
        
        private void WriteToLCD(Climate climateTelemetry)
        {
            string[] lcdFormattedString = climateTelemetry.GetLCDFormattedString(_unitSystem);
            _lcd.Clear();
            _lcd.Write(0, 0, lcdFormattedString[0]);
            _lcd.Write(0, 1, lcdFormattedString[1]);
        }
    }
}
