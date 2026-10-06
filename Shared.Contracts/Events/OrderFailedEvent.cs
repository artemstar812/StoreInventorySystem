using Shared.Contracts.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts.Events
{
    public class OrderFailedEvent : IIntegrationEvent
    {
        public Guid EventId { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }

        public string Topic => "order-failed";
    }
}
