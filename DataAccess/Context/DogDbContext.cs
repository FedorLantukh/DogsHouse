using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class DogDbContext : DbContext
    {
        public DogDbContext(DbContextOptions<DogDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Dog> Dogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .ToTable("dogs")
                .HasKey(d => d.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
