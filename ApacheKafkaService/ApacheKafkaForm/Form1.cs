using Confluent.Kafka;

namespace ApacheKafkaForm
{
    public partial class Form1 : Form
    {
        private readonly IConsumer<string, byte[]> _consumer;
        private bool _running = true;

        public Form1()
        {
            InitializeComponent();

            var config = new ConsumerConfig
            {
                BootstrapServers = "54.255.41.66:9092",
                GroupId = "winforms-consumer" + Guid.NewGuid(),
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            _consumer = new ConsumerBuilder<string, byte[]>(config).Build();
            _consumer.Subscribe("esp32-images");

            Task.Run(() => StartKafkaConsumer());
        }

        private void StartKafkaConsumer()
        {
            while (_running)
            {
                try
                {
                    var cr = _consumer.Consume();
                    byte[] imageData = cr.Value;
                    Invoke(new Action(() =>
                    {
                        using var ms = new MemoryStream(imageData);
                        pictureBox1.Image = Image.FromStream(ms);
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _running = false;
            _consumer.Close();
            base.OnFormClosing(e);
        }
    }
}
