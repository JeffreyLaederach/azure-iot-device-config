namespace IoT.Services.IoTDevices
{
    internal interface IoTDevice
    {
        public enum UnitSystem
        {
            Imperial,
            Metric
        }
        public enum StatusCode
        {
            Completed = 200,
            InProgress = 202,
            ReportDeviceInitialProperty = 203,
            BadRequest = 400,
            NotFound = 404
        }
        public Task Init();
        public Task Disconnect();
    }
}
