using MediatR;
using Shop.Infrastructure.Data;
using Shop.Module.Catalog.Entities;
using Shop.Module.Core.Events;
using Shop.Module.Core.Extensions;

namespace Shop.Module.Catalog.Handlers
{
    public class EntityViewedHandler : INotificationHandler<EntityViewed>
    {
        private readonly IRepository<ProductRecentlyViewed> _recentlyViewedProductRepository;
        private readonly IWorkContext _workContext;

        public EntityViewedHandler(
            IRepository<ProductRecentlyViewed> recentlyViewedProductRepository,
            IWorkContext workcontext)
        {
            _recentlyViewedProductRepository = recentlyViewedProductRepository;
            _workContext = workcontext;
        }

        public async Task Handle(EntityViewed notification, CancellationToken cancellationToken)
        {
        }
    }
}
