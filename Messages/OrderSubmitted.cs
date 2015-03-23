using System;
using NServiceBus;

namespace Messages
{
    public class OrderSubmitted : IEvent
    {
        public Guid OrderId { get; set; }
        public decimal OrderValue { get; set; }
    }
}
