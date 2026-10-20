using FinanceTools.Data.UserItems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceTools.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<EventBuy> BuyEvents => Set<EventBuy>();
        public DbSet<EventSell> SellEvents => Set<EventSell>();
        public DbSet<EventSellAllocation> EventSellAllocations => Set<EventSellAllocation>();
        public DbSet<Broker> Brokers => Set<Broker>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventSellAllocation>()
                .HasKey(a => new { a.EventBuyId, a.EventSellId });

            modelBuilder.Entity<EventBuy>()
                .HasOne(buyEvent => buyEvent.Broker)
                .WithMany(broker => broker.EventBuys)
                .HasForeignKey(buyEvent => buyEvent.BrokerId);

            modelBuilder.Entity<EventSell>()
                .HasOne(sellEvent => sellEvent.Broker)
                .WithMany(broker => broker.EventSells)
                .HasForeignKey(sellEvent => sellEvent.BrokerId);
        }
    }
}
