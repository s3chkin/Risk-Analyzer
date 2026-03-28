using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiskAnalyzer.Data.Models;

namespace RiskAnalyzer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Scenarios> Scenarios { get; set; }
        public DbSet<Decisions> Decisions { get; set; }
        public DbSet<Criteria> Criteria { get; set; }
        public DbSet<RiskTypes> RiskTypes { get; set; }
        public DbSet<AppUser> AppUser { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // за по-сложни връзки
        }
    }
}
