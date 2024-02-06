// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using System.Text;
using RabbitMQ.Client;
using STM.Producer;

Console.WriteLine("Producer");


var orderCreatedEvent = new OrderCreatedEvent() { OrderCode = "abc", Details = "def" };

Calculate(orderCreatedEvent)
    ;


static void Calculate(OrderCreatedEvent orderCreatedEvent)
{
    var orderNew = orderCreatedEvent with { OrderCode = "new", Details = "aaa" };
}


var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://jsqjpfwe:mG9pvqLZB_-NRUz9CO40xPzDC4vFI7P8@woodpecker.rmq.cloudamqp.com/jsqjpfwe")
};

using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();
channel.ConfirmSelect();

channel.BasicAcks += Channel_BasicAcks;

void Channel_BasicAcks(object? sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
{
    Console.WriteLine($"{e.DeliveryTag}");
}

channel.ExchangeDeclare("topic", ExchangeType.Topic, true, false, null);

Enumerable.Range(1, 10).ToList().ForEach(i =>
{
    var message = $"Message {i}";
    var body = Encoding.UTF8.GetBytes(message);

    var properties = channel.CreateBasicProperties();

    var headers = new Dictionary<string, object>();
    headers.Add("event.id", Guid.NewGuid());

    properties.Headers = headers;
    properties.Persistent = true;


    channel.BasicPublish("topic", "critical.warning.info", properties, body);


    Console.WriteLine($"Sent {message}");
});
channel.WaitForConfirmsOrDie();

Console.WriteLine("Mesaj gönderme tamamlandı.");