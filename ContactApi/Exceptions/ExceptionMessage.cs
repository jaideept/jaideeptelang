using Newtonsoft.Json;

namespace ContactApi.Exceptions
{
    public class ExceptionMessage
    {
        public string Message { get; set; }

        public ExceptionMessage() { }

        public ExceptionMessage(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new { message = new string(Message) });
        }
    }
}
