using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using PlannerTool.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public string DbPath { get; }

    public DatabaseContext()
    {
        Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "plannertool.db");
    }

    // Creates the database if it doesn't exist in local folder
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}")
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectTask>()
            .HasMany(t => t.SubTasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
