using HardwareStore_Application.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<RAMDetails> RAMDetails { get; set; }
    public DbSet<CPUDetails> CPUDetails { get; set; }
    public DbSet<SSDDetails> SSDDetails { get; set; }
    public DbSet<VGADetails> VGADetails { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Optional: configure relationships or constraints here

        base.OnModelCreating(modelBuilder);
    }
}
