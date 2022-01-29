using JimmyRefactoring.Domain;
using Microsoft.EntityFrameworkCore;

namespace JimmyRefactoring.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<OfferType> OfferTypes => Set<OfferType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
            .HasKey(m => m.Id);
        
        modelBuilder.Entity<Member>()
            .HasMany<Offer>(m => m.AssignedOffers)
            .WithOne(o => o.MemberAssigned);
    }
}