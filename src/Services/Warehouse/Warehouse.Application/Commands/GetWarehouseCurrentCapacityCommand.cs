﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Models;

namespace Restmium.ERP.Services.Warehouse.Application.Commands
{
    public class GetWarehouseCurrentCapacityCommand : IRequest<WarehouseCapacity>
    {
        public GetWarehouseCurrentCapacityCommand(int warehouseId)
        {
            this.WarehouseId = warehouseId;
        }

        public int WarehouseId { get; }
    }
}