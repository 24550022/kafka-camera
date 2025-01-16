using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace ApacheKafkaService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IProducer<string, byte[]> _producer;
        private const string Topic = "esp32-images";
        public HomeController()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "54.255.41.66:9092"
            };
            _producer = new ProducerBuilder<string, byte[]>(config).Build();
        }
        [HttpGet(Name = "Index")]
        public string Index()
        {
            return "Hi";
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            using var memoryStream = new MemoryStream();
            await Request.Body.CopyToAsync(memoryStream);
            var data = memoryStream.ToArray();

            var message = new Message<string, byte[]>
            {
                Key = "esp32-photo",
                Value = data
            };

            await _producer.ProduceAsync(Topic, message);
            return Ok("Photo sent to Kafka");
        }
    }
}
