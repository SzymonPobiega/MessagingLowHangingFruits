using System;
using Messages;
using NServiceBus;

namespace Publisher
{
    class Program
    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=kgdnet;Integrated Security=True";

        static void Main(string[] args)
        {
            var random = new Random();

            var busConfig = new BusConfiguration();
            busConfig.UsePersistence<InMemoryPersistence>();
            busConfig.UseTransport<SqlServerTransport>().ConnectionString(ConnectionString);

            using (var bus = Bus.Create(busConfig).Start())
            {
                while (true)
                {
                    Console.WriteLine("Press <enter> to submit an order");
                    Console.ReadLine();

                    var orderMessage = new OrderSubmitted
                    {
                        OrderId = Guid.NewGuid(),
                        OrderValue = random.Next(200)
                    };
                    bus.Publish(orderMessage);

                    Console.WriteLine("Order {0} worth {1} submitted.", orderMessage.OrderId, orderMessage.OrderValue);
                }
            }
        }
    }

}
