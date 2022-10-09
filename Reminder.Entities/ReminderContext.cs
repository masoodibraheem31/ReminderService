using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reminder.Entities.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reminder.Entities
{
    public class ReminderContext : IdentityDbContext<SystemUser, SystemRole, string,
     IdentityUserClaim<string>, SystemUserRole, IdentityUserLogin<string>,
     IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ReminderContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseModel).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reminder.Entities.Models.Reminder>()
              .HasOne(c => c.User)
              .WithMany(m => m.Reminders)
              .HasForeignKey(f => f.UserId);
        }
        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:  // remove
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }

        public virtual DbSet<Reminder.Entities.Models.Reminder> Reminders { get; set; }
    }
}
