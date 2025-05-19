using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.PlugAndPlay;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Client;
using IoT.Services.IoTDevices.ClimateSensorArray;
using IoT.Services.Logging;

namespace IoT.Services.IoTDevices
{
    class IoTDeviceFactory
    {
        // Parameters for IoTDeviceFactory constructor
        private string _modelId;
        private string _deviceId;
        private string _deviceSymmetricKey;
        private string _dpsEndpoint;
        private string _dpsIdScope;
        private bool _mockData;

        // Logger
        private Logger _logger = new Logger();

        public IoTDeviceFactory(string modelId, string deviceId, string deviceSymmetricKey, string dpsEndpoint, string dpsIdScope, bool mockData)
        {
            _modelId = modelId;
            _deviceId = deviceId;
            _deviceSymmetricKey = deviceSymmetricKey;
            _dpsEndpoint = dpsEndpoint;
            _dpsIdScope = dpsIdScope;
            _mockData = mockData;
        }
        public async Task<IoTDevice> getIoTDevice()
        {
            IoTDevice device;
            var cts = new CancellationTokenSource();

            DeviceClient deviceClient = await ProvisionDeviceAsync(cts.Token);
            switch (_modelId)
            {
                case "dtmi:company:climate_sensor_array;1.0":
                    device = new ClimateSensorArrayV1(deviceClient, cts, IoTDevice.UnitSystem.Imperial, 2000, _mockData);
                    break;
                default:
                    throw new Exception($"Device Provisioning Failed: Invalid Model ID {_modelId}");
            }
            return device;
        }

        // Method to provision the IoT device
        // Parameters:
        // cancellationToken: The cancellation token used to cancel the task

        private async Task<DeviceClient> ProvisionDeviceAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Provisioning IoT Device");
            _logger.Log("Provisioning IoT Device");
            using SecurityProvider symmetricKeyProvider = new SecurityProviderSymmetricKey(_deviceId, _deviceSymmetricKey, null);
            using ProvisioningTransportHandler mqttTransportHandler = new ProvisioningTransportHandlerMqtt();
            ProvisioningDeviceClient pdc = ProvisioningDeviceClient.Create(_dpsEndpoint, _dpsIdScope, symmetricKeyProvider, mqttTransportHandler);

            var pnpPayload = new ProvisioningRegistrationAdditionalData
            {
                JsonData = PnpConvention.CreateDpsPayload(_modelId),
            };
            DeviceRegistrationResult dpsRegistrationResult = await pdc.RegisterAsync(pnpPayload, cancellationToken);

            var authMethod = new DeviceAuthenticationWithRegistrySymmetricKey(_deviceId, _deviceSymmetricKey);
            DeviceClient deviceClient = InitializeDeviceClient(dpsRegistrationResult.AssignedHub, authMethod, _modelId);
            Console.WriteLine("IoT Device Provisioned");
            _logger.Log("IoT Device Provisioned");

            return deviceClient;
        }

        // Parameters:
        //  hostname: The hostname of the assigned hub
        //  authenticationMethod: The authentication method for the device
        //  ModelId: The model ID of the device
        
        private static DeviceClient InitializeDeviceClient(string hostname, IAuthenticationMethod authenticationMethod, string ModelId)
        {
            var options = new ClientOptions
            {
                ModelId = ModelId,
            };

            DeviceClient deviceClient = DeviceClient.Create(hostname, authenticationMethod, TransportType.Mqtt, options);

            return deviceClient;
        }
    }
}
