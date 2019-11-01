namespace CXDE.Server_Side
{
    public class MessageEvent
    {
        public string MessageType { get; set; }
        public Dialog Dialog { get; set; }
        public string Event { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public string errorType { get; set; }
    }
}
