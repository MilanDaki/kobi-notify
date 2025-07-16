using kobi_notify.Models;
using Microsoft.EntityFrameworkCore;

namespace kobi_notify.Data
{
    public class KobiDbContext: DbContext
    {
        public KobiDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {

        }

        public DbSet<DataModel> CustomerProfiles { get; set; }
        public DbSet<FieldMapping> FieldMappings { get; set; }
        public DbSet<FallbackRule> FallbackRules { get; set; }
        public DbSet<DataSource> DataSources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataModel>()
                .HasMany(cp => cp.FieldMappings)
                .WithOne(fm => fm.CustomerProfile!)
                .HasForeignKey(fm => fm.CustomerProfileId);
        }

    }
}
 