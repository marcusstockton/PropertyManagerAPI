using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyManagerApi.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagerApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Overrides the save changes async to set the created date / updated dates
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var AddedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Added)
                .Where(E=>E.Entity.GetType().BaseType.Name == "Base")
                .ToList();
            AddedEntities.ForEach(E =>
            {
                E.Property("CreatedDateTime").CurrentValue = DateTime.Now;
                E.Property("IsActive").CurrentValue = true;
            });

            var EditedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Modified)
                .Where(E => E.Entity.GetType().BaseType.Name == "Base")
                .ToList();
            EditedEntities.ForEach(E =>
            {
                E.Property("UpdatedDateTime").CurrentValue = DateTime.Now;
            });

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
