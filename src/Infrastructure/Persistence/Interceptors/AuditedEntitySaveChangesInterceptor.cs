using Application._Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Interceptors
{
    public class AuditedEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUserService _currentUserService;

        public AuditedEntitySaveChangesInterceptor(IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService)
        {
            _dateTimeProvider = dateTimeProvider;
            _currentUserService = currentUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext context)
        {
            if (context == null) return;

            foreach (EntityEntry<BaseAuditedEntity> entry in context.ChangeTracker.Entries<BaseAuditedEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = _dateTimeProvider.Now;
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = _dateTimeProvider.Now;
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;

                }
            }
        }
    }
}