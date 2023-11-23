namespace EndpointManager.Model
{
    public class EndpointModel
    {
        public string EndpointSerialNumber { get; set; }
        public int MeterModelId { get; set; }
        public int MeterNumber { get; set; }
        public string MeterFirmwareVersion { get; set; }
        public int SwitchState { get; set; }

        public static int SetMeterModelId(string meterModel)
        {
            return meterModel switch
            {
                "NSX1P2W" => 16,
                "NSX1P3W" => 17,
                "NSX2P3W" => 18,
                "NSX3P4W" => 19,
                _ => -1
            };
        }

        public static int SetSwtichState(string switchState)
        {
            return switchState switch
            {
                "Disconnected" => 0,
                "Connected" => 1,
                "Armed" => 2,
                _ => -1
            };
        }
    }
}
