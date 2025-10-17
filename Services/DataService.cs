using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PlannerTool.Models;

namespace PlannerTool.Services;

public sealed class DataService
{
    // --- Singleton setup ---
    private static readonly Lazy<DataService> _instance =
        new(() => new DataService(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static DataService Instance => _instance.Value;

    // Private constructor so no one else can create it
    private DataService() { }

    // --- CRUD operations ---

    public Project AddProject(string title, string description = "")
    {
        using var db = new DatabaseContext();
        Project result = db.Projects.Add(new Project { Title = title, Description = description }).Entity;
        db.SaveChanges();
        return result;
    }

    public List<Project> GetProjects()
    {
        using var db = new DatabaseContext();
        return db.Projects.ToList();
    }

    public void DeleteProject(int projectId)
    {
        using var db = new DatabaseContext();
        var project = db.Projects.FirstOrDefault(p => p.Id == projectId);
        if (project != null)
        {
            db.Projects.Remove(project);
            db.SaveChanges();
        }
    }

    public void AddTaskToProject(int projectId, string taskTitle)
    {
        using var db = new DatabaseContext();
        var project = db.Projects.FirstOrDefault(p => p.Id == projectId);
        if (project == null) return;

        var newTask = new ProjectTask { Title = taskTitle };
        project.Tasks ??= new List<ProjectTask>();
        project.Tasks.Add(newTask);

        db.SaveChanges();
    }
}