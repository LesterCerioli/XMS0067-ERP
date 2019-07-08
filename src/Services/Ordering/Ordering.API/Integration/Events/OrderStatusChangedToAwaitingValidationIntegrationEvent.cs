﻿using Restmium.ERP.BuildingBlocks.EventBus.Events;
using System.Collections.Generic;

namespace Ordering.API.Integration.Events
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
    {
        public OrderStatusChangedToAwaitingValidationIntegrationEvent(long orderId, List<OrderItem> items) : base()
        {
            this.OrderId = orderId;
            this.OrderItems = items;
        }

        public long OrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public class OrderItem
        {
            public OrderItem(int productId, int units)
            {
                this.ProductId = productId;
                this.Units = units;
            }

            public int ProductId { get; set; }
            public int Units { get; set; }
        }
    }
}