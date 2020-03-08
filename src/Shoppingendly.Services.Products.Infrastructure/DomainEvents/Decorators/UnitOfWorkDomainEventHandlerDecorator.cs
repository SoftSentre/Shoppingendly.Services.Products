using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Decorators
{
    public class UnitOfWorkDomainEventHandlerDecorator<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        private readonly IDomainEventHandler<TEvent> _decorated;
        private readonly ILogger<UnitOfWorkDomainEventHandlerDecorator<TEvent>> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkDomainEventHandlerDecorator(
            IDomainEventHandler<TEvent> decorated,
            ILogger<UnitOfWorkDomainEventHandlerDecorator<TEvent>> logger, 
            IUnitOfWork unitOfWork)
        {
            _decorated = decorated.IfEmptyThenThrowAndReturnValue();
            _logger = logger.IfEmptyThenThrowAndReturnValue();
            _unitOfWork = unitOfWork.IfEmptyThenThrowAndReturnValue();
        }

        public async Task HandleAsync(TEvent @event)
        {
            await _decorated.HandleAsync(@event);
            await _unitOfWork.SaveAsync();
        }
    }
}