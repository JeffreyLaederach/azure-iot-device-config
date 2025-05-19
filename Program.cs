using IoT.Services.IoTDevices;

public class Program
{
    private static IoTDevice _device;

    public static async Task Main(string[] args)
    {
        try
        {
            // format for modelId -> dtmi:company:<unique-model-identifier>;<model-version-number>
            String ModelId = Environment.GetEnvironmentVariable("IOT_DEVICE_MODEL_ID") ?? "dtmi:company:climate_sensor_array;1.0";
            String DeviceId = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_DEVICE_ID") ?? "DPS_DEVICE_ID_GOES_HERE";
            String DeviceSymmetricKey = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_DEVICE_KEY") ?? "DPS_DEVICE_KEY_GOES_HERE";
            String DpsEndpoint = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_ENDPOINT") ?? "global.azure-devices-provisioning.net";
            String DpsIdScope = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_ID_SCOPE") ?? "DPS_DEVICE_ID_SCOPE_GOES_HERE";
            bool MockData = Boolean.Parse(Environment.GetEnvironmentVariable("MOCK_DATA") ?? "True");

            try
            {
                IoTDeviceFactory deviceFactory = new IoTDeviceFactory(ModelId,DeviceId,DeviceSymmetricKey,DpsEndpoint,DpsIdScope,MockData);
                _device = await deviceFactory.getIoTDevice();
                Console.CancelKeyPress += new ConsoleCancelEventHandler(HandleCancelKeyPress);
                AppDomain.CurrentDomain.UnhandledException += HandleUnhandleException;
                await _device.Init();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static async void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs args)
    {
        Console.WriteLine("Ctrl+C pressed");
        await _device.Disconnect();
        Environment.Exit(0);
    }
    private static async void HandleUnhandleException(object sender, UnhandledExceptionEventArgs e)
    {
        Console.WriteLine("Unhandled exception occurred: " + e.ExceptionObject.ToString());
        await _device.Disconnect();
        Environment.Exit(1);
    }
}
