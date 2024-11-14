using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class BloggingContext : DbContext
{
    public DbSet<Todo> Todo { get; set; }
    public DbSet<Task> Task { get; set; }
    public DbSet<Team> Team { get; set; }
    public DbSet<Worker> Worker { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>()
            .HasMany(t => t.Workers)
            .WithMany(w => w.Teams)
            .UsingEntity(j => j.ToTable("TeamWorkers"));

        modelBuilder.Entity<Task>()
            .HasMany(t => t.Todos)
            .WithOne(t => t.Task)
            .HasForeignKey(t => t.TaskId);

    }

    public string DbPath { get; }
    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Todo
{
    public int TodoId { get; set; }
    public string? Name { get; set; }
    public bool? IsComplete { get; set; }

    public Task? Task { get; set; }
    public int TaskId { get; set; }
}

public class Task
{
    public int TaskId { get; set; }
    public string? Name { get; set; }
    public List<Todo> Todos { get; set; } = new();

}

public class Team
{
    public int TeamId { get; set; }
    public string? Name { get; set; }

    public int? CurrentTaskId { get; set; }
    public Task? CurrentTask { get; set; }

    public List<Worker> Workers { get; set; } = new();
    public List<Task> Tasks { get; set; } = new();
}

public class Worker
{
    public int WorkerId { get; set; }
    public string? Name { get; set; }
    public Todo? CurrentTodo { get; set; }

    public List<Team> Teams { get; set; } = new();
    public List<Todo> Todos { get; set; } = new();
}