using System.Text.Json.Nodes;

namespace IoT.Models.Telemetry
{
    internal interface Telemetry
    {
        public string GetTelemetryJson();

    }
}
