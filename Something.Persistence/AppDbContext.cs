﻿using Microsoft.EntityFrameworkCore;
using Something.Domain.Models;

namespace Something.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Domain.Models.Something> Somethings { get; set; }
        public DbSet<Domain.Models.SomethingElse> SomethingElses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Models.Something>().Property<long>("Id");
            modelBuilder.Entity<Domain.Models.SomethingElse>().Property<long>("Id");
        }
    }
}
