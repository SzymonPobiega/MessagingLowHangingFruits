using System;

namespace Subscriber
{
    public class Order
    {
        public virtual Guid OrderId { get; set; }
        public virtual decimal Value { get; set; }
    }
}