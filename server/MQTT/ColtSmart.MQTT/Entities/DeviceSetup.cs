namespace ColtSmart.MQTT.Entities
{
    public class DeviceSetup
    {
        public bool IsGateway { get; set; }

        public string DevType { get; set; }

        public string Version { get; set; }

        public int ComPortNum { get; set; }

        public string GPS { get; set; }
    }
}
