using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactForm.Sample.Postgres.Models
{
    public class AppDbContext: DbContext
    {
        public DbSet<ContactModelData> Contacts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactModelData>(e =>
            {
               e.Ignore(c => c.RecaptchaResponse);
               e.Property(c => c.ContactName).IsRequired().HasMaxLength(50);
               e.Property(c => c.Category).HasMaxLength(30);
               e.Property(c => c.Email).HasMaxLength(50);
               e.Property(c => c.Phone).HasMaxLength(20);
               e.Property(c => c.Subject).HasMaxLength(50);
               e.Property(c => c.Message).HasMaxLength(1500);
               e.Property(c => c.CreationDate).HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<ContactModelData>().HasIndex(c => c.CreationDate);
        }
    }
}
