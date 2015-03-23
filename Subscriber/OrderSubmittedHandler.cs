using System;
using Messages;
using NHibernate;
using NServiceBus;

namespace Subscriber
{
    public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
    {
        static readonly Random ChaosGenerator = new Random();

        private readonly ISessionFactory sessionFactory;

        public OrderSubmittedHandler(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Handle(OrderSubmitted message)
        {
            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                Console.WriteLine("Received order {0} for {1}.", message.OrderId, message.OrderValue);
                var order = new Order
                {
                    OrderId = message.OrderId,
                    Value = message.OrderValue
                };
                session.Save(order);
                tx.Commit();

                if (ChaosGenerator.Next(2) == 0)
                {
                    //Generate chaos
                    throw new Exception("Chaos unleashed!");
                }
            }
        }
    }
}