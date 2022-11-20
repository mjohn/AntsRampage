using AntsRampage.Application.Messages;
using AntsRampage.Domain.Entities;
using MassTransit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AntsRampage.UI.Consumers
{
    public class AntConsumer : IConsumer<AntWorkCompleted>
    {
        readonly ILogger<AntConsumer> _logger;

        public AntConsumer(ILogger<AntConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AntWorkCompleted> context)
        {
            //_logger.LogInformation("Received Text: {Text}", context.Message.Text);
            Console.WriteLine($"Received Text: {context.Message.Id}");
            //Console.WriteLine($"Received Body: {context.Message.ResponseBody}");
            Console.WriteLine($"Received Body: {await context.Message.ResponseBody.Value}");

        }
    }
}
