using System;
using Microsoft.EntityFrameworkCore;
using SME.Models;
namespace SME.Persistence
{
    public class SMEContext : DbContext
    {
        public DbSet<Technology> Technologies {get; set;}
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=SME;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

