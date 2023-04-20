using Application._Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly AuditedEntitySaveChangesInterceptor _auditedEntitySaveChangesInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    AuditedEntitySaveChangesInterceptor auditedEntitySaveChangesInterceptor)
                                    : base(options)
        {
            _auditedEntitySaveChangesInterceptor = auditedEntitySaveChangesInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditedEntitySaveChangesInterceptor);
        }

        public DbSet<User> Users => Set<User>();
    }
}