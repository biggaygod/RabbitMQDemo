using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Worker
{
    class Worker
    {
        public static void Main()
		{
			var factory = new ConnectionFactory() 
			{ 
				HostName = "111.231.134.184",
				UserName = "admin", 
				Password = "admin", 			
			};
			using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
			{
				// channel.QueueDeclare(queue: "task_queue",
				// 					 durable: false,
				// 					 exclusive: false,
				// 					 autoDelete: false,
				// 					 arguments: null);
				channel.ExchangeDeclare(exchange: "logs", type: "fanout");
				var queueName=channel.QueueDeclare().QueueName;
					channel.QueueBind(
									queue:queueName,
									exchange:"logs",
									routingKey:"");
									 
				//channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
				Console.WriteLine(" [*] Waiting for messages.");

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine(" [x] Received {0}", message);

					// 模拟耗时操作。
					int dots = message.Split('.').Length - 1;
					Thread.Sleep(dots * 1000);

					Console.WriteLine(" [x] Done");
					// 手动发送消息确认信号。
					//channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
				};
				channel.BasicConsume(queue: queueName,//"task_queue",
									 autoAck: true,//false,
									 consumer: consumer);
				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();
			}
		}
    }
}
