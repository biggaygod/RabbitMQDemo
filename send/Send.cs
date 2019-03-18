using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
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
					channel.QueueDeclare(
										queue:"hello",
										durable:false,
										exclusive:false,
										autoDelete:false,
										arguments:null
										);
					string message = "Hello World!";
					var body = Encoding.UTF8.GetBytes(message);

					channel.BasicPublish(exchange: "",
										 routingKey: "hello",
										 basicProperties: null,
										 body: body);
					Console.WriteLine(" [x] Sent {0}", message);
					Console.WriteLine(" Press [enter] to exit.");
					Console.ReadLine();
				}
			}
        }
    }
}
