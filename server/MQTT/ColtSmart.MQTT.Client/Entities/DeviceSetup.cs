namespace ColtSmart.MQTT.Client.Entities
{
    public class DeviceSetup: MessageBase
    {
        public bool IsGateway { get; set; }

        public string DevType { get; set; }

        public string Version { get; set; }

        public int ComPortNum { get; set; }

        public string GPS { get; set; }
    }

    public class MessageBase
    {
        public string DeviceId { get; set; }
    }
}
