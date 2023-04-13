using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Zuzka.Data.Entities;

namespace Zuzka.Data
{
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions options) : base(options) { }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Zuzka.Data.Entities.Data> Data { get; set; }

        public override int SaveChanges()
        {
            ChangeTrackerSetDateForEntity();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTrackerSetDateForEntity();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //tags
            modelBuilder.Entity<Document>()
            .Property(e => e.Tags)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            //guid generation for primary keys
            foreach (var entity in modelBuilder.Model.GetEntityTypes()
                .Where(t =>
                    t.ClrType.GetProperties()
                        .Any(p => p.CustomAttributes.Any(a =>
                        a.AttributeType == typeof(DatabaseGeneratedAttribute)))))
            {
                foreach (var property in entity.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(Guid)
                    && p.CustomAttributes.Any(a => a.AttributeType == typeof(DatabaseGeneratedAttribute))))
                {
                    modelBuilder
                        .Entity(entity.ClrType)
                        .Property(property.Name)
                        .HasDefaultValueSql("newsequentialid()");
                }
            }

            //1 to 1 relationship
            modelBuilder.Entity<Document>()
             .HasOne<Zuzka.Data.Entities.Data>(p => p.Data)
             .WithOne(x => x.Document)
             .HasForeignKey<Zuzka.Data.Entities.Data>(s => s.DocumentId);
        }
        private void ChangeTrackerSetDateForEntity()
        {
            var AddedEntities = ChangeTracker.Entries<SaveConfig>().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                E.Entity.Created = DateTime.UtcNow;
            });

            var EditedEntities = ChangeTracker.Entries<SaveConfig>().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                E.Entity.LastUpdated = DateTime.UtcNow;
            });
        }
    }
}