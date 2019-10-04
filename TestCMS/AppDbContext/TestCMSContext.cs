using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using TestCMS.AppDbContext.Models;

namespace TestCMS.AppDbContext
{
    public partial class TestCMSContext : DbContext
    {
        IConfiguration _configuration;

        public TestCMSContext(DbContextOptions<TestCMSContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<MenuItems> MenuItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CMS"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<MenuItems>(entity =>
            {
                // entity.ToTable("");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_MenuItems_MenuItems");
            });
        }
    }
}
