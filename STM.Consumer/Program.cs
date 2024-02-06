// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Consumer");

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://jsqjpfwe:mG9pvqLZB_-NRUz9CO40xPzDC4vFI7P8@woodpecker.rmq.cloudamqp.com/jsqjpfwe")
};

using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();

channel.BasicQos(0, 10, true);

channel.QueueDeclare("topic-queue", true, false, false, null);

channel.QueueBind("topic-queue", "topic", "#.info", null);


var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.WriteLine($"Received {message}");
    channel.BasicAck(args.DeliveryTag, false);
};

channel.BasicConsume("topic-queue", false, consumer);

Console.ReadLine();
//consumer.Received += method1;
//static void method1(Object sender, BasicDeliverEventArgs args)
//{
//    var message = Encoding.UTF8.GetString(args.Body.ToArray());
//    Console.WriteLine($"Received {message}");
//}