﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Events;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System.Threading;
using System.Threading.Tasks;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class DeleteMovementCommandHandler : IRequestHandler<DeleteMovementCommand, Movement>
    {
        protected const string DeleteMovementCommandHandler_EntityNotFoundException = "Movement (Id={0}) not found!";

        public DeleteMovementCommandHandler(DatabaseContext context, IMediator mediator)
        {
            this.DatabaseContext = context;
            this.Mediator = mediator;
        }

        protected DatabaseContext DatabaseContext { get; }
        protected IMediator Mediator { get; }

        public async Task<Movement> Handle(DeleteMovementCommand request, CancellationToken cancellationToken)
        {
            // Find Movement and throw an exception if not found
            Movement movement = await this.DatabaseContext.Movements.FindAsync(new object[] { request.Model.Id }, cancellationToken);
            if (movement == null)
            {
                throw new EntityNotFoundException(string.Format(DeleteMovementCommandHandler_EntityNotFoundException, request.Model.Id));
            }

            // Delete and Save
            movement = this.DatabaseContext.Movements.Remove(movement).Entity;
            await this.DatabaseContext.SaveChangesAsync(cancellationToken);

            // Publish DomainEvent that the Movement has been deleted
            await this.Mediator.Publish(new MovementDeletedDomainEvent(movement));

            return movement;
        }
    }
}
