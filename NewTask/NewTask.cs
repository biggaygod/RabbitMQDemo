using System;
using RabbitMQ.Client;
using System.Text;

namespace NewTask
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory=new ConnectionFactory();
				factory.UserName = "admin"; 
				factory.Password = "admin"; 
				factory.Port = 5672; 
				factory.HostName = "111.231.134.184"; 
			using (var connection=factory.CreateConnection())
			{
				using(var channel=connection.CreateModel())
				{
					// channel.QueueDeclare(
					// 					queue:"task_queue",
					// 					durable:true,
					// 					exclusive:false,
					// 					autoDelete:false,
					// 					arguments:null
					// 					);
					channel.ExchangeDeclare(exchange: "logs", type: "fanout");
					var message = GetMessage(args);
					var body = Encoding.UTF8.GetBytes(message);

					//var properties = channel.CreateBasicProperties();
					//properties.Persistent = true;

					channel.BasicPublish(exchange: "logs",//"task_queue",
										 routingKey:"", //"task_queue",
										 basicProperties: null,//properties,
										 body: body);
					
					Console.WriteLine(" [x] Sent {0}", message);					 
				}
			}
			Console.WriteLine(" Press [enter] to exit.");
			Console.ReadLine();
        }
		
		private static string GetMessage(string[] args)
		{
			return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
		}
    }
}