namespace Clickatell.Services.Data
{
    public class MessageRequest
    {
        public MessageRequest(params string[] phoneNumber)
        {
            PhoneNumbers = phoneNumber;
        }

        public MessageRequest() { }

        public string[] PhoneNumbers { get; set; }
    }
}
