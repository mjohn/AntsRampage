using MassTransit;

namespace AntsRampage.Application.Messages
{
    public class AntWorkCompleted
    {
        public Guid Id { get; set; }
        public DateTime RequestedAt { get; set; }
        public bool Succeed { get; set; }
        public MessageData<string> ResponseBody { get; set; }
    }
}
