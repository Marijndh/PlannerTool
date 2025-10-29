using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using PlannerTool.Models;

namespace PlannerTool.Services;

public sealed class DataService
{
    // --- Singleton setup ---
    private static readonly Lazy<DataService> _instance =
        new(() => new DataService(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static DataService Instance => _instance.Value;
    
    private DataService() { }

    public Project AddProject(string title, string description = "")
    {
        using DatabaseContext db = new DatabaseContext();
        Project result = db.Projects.Add(new Project { Title = title, Description = description }).Entity;
        db.SaveChanges();
        return result;
    }

    public List<Project> GetProjects()
    {
        using DatabaseContext db = new DatabaseContext();
        List<Project> projects = db.Projects
            .Include(p => p.Tasks)
            .ThenInclude(t => t.SubTasks)
            .ToList();
        return projects;
    }
    
    private static Project? LoadProjectWithRelations(DatabaseContext db, int projectId)
    {
        return db.Projects
            .Include(p => p.Tasks)
            .ThenInclude(t => t.SubTasks)
            .FirstOrDefault(p => p.Id == projectId);
    }
    
    private static void UpdateProjectProperties(Project existingProject, Project updatedProject)
    {
        existingProject.Title = updatedProject.Title;
        existingProject.Description = updatedProject.Description;
    }
    
    private static void SyncTasks(DatabaseContext db, Project existingProject, Project updatedProject)
    {
        // Remove deleted tasks
        List<ProjectTask> deletedTasks = existingProject.Tasks
            .Where(oldTask => updatedProject.Tasks.All(newTask => newTask.Id != oldTask.Id))
            .ToList();

        foreach (ProjectTask task in deletedTasks)
            db.ProjectTasks.Remove(task);

        // Add or update tasks and subtasks
        foreach (ProjectTask newTask in updatedProject.Tasks)
        {
            ProjectTask? existingTask = existingProject.Tasks.FirstOrDefault(t => t.Id == newTask.Id);

            if (existingTask == null)
            {
                AddNewTask(existingProject, newTask);
            }
            else
            {
                UpdateExistingTask(db, existingTask, newTask);
            }
        }
    }
    
    private static void AddNewTask(Project existingProject, ProjectTask newTask)
    {
        ProjectTask taskToAdd = new ProjectTask
        {
            Title = newTask.Title,
            SubTasks = newTask.SubTasks
                .Select(st => new ProjectTask { Title = st.Title })
                .ToList()
        };

        existingProject.Tasks.Add(taskToAdd);
    }
    
    private static void UpdateExistingTask(DatabaseContext db, ProjectTask existingTask, ProjectTask newTask)
    {
        existingTask.Title = newTask.Title;

        // Remove deleted subtasks
        List<ProjectTask> deletedSubs = existingTask.SubTasks
            .Where(oldSub => newTask.SubTasks.All(newSub => newSub.Id != oldSub.Id))
            .ToList();
        foreach (ProjectTask st in deletedSubs)
            db.ProjectTasks.Remove(st);

        // Add or update subtasks
        foreach (ProjectTask newSub in newTask.SubTasks)
        {
            ProjectTask? existingSub = existingTask.SubTasks.FirstOrDefault(st => st.Id == newSub.Id);
            if (existingSub == null)
            {
                existingTask.SubTasks.Add(new ProjectTask { Title = newSub.Title });
            }
            else
            {
                existingSub.Title = newSub.Title;
            }
        }
    }

    
    public void SaveProject(Project updatedProject)
    {
        using DatabaseContext db = new DatabaseContext();

        Project? existingProject = LoadProjectWithRelations(db, updatedProject.Id);
        if (existingProject == null) return;

        UpdateProjectProperties(existingProject, updatedProject);
        SyncTasks(db, existingProject, updatedProject);

        db.SaveChanges();
    }


    public void DeleteProject(int projectId)
    {
        using DatabaseContext db = new DatabaseContext();
        Project? project = db.Projects.FirstOrDefault(p => p.Id == projectId);
        if (project != null)
        {
            db.Projects.Remove(project);
            db.SaveChanges();
        }
    }
}