using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlannerTool.Models;
using PlannerTool.Services;

namespace PlannerTool.ViewModels;
public partial class ProjectViewModel : ViewModelBase
{
   
    private readonly Project _project;
    private readonly Action<ProjectViewModel>? _openProjectAction;
    
    public int Id => _project.Id;

    [ObservableProperty]
    private string _newTaskTitle;
    
    [ObservableProperty]
    private string _title;
    
    [ObservableProperty]
    private string _description;
    
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();

    public ProjectViewModel(Project project, Action<ProjectViewModel>? openProjectAction = null)
    {
        _project = project;
        _openProjectAction = openProjectAction;
        
        Title = project.Title;
        Description = project.Description;
        NewTaskTitle = string.Empty;
        
        foreach (ProjectTask task in project.Tasks)
        {
            Tasks.Add(new TaskViewModel(task, DeleteTask));
        }
    }

    public void OpenProject()
    {
        _openProjectAction?.Invoke(this);
    }
    
    public void AddTask()
    {
        ProjectTask newTask = new ProjectTask {Title = NewTaskTitle};
        Tasks.Add(new TaskViewModel(newTask, DeleteTask));
        NewTaskTitle = string.Empty;
    }

    private void DeleteTask(ProjectTask task)
    {
        TaskViewModel? taskViewModel = null;
        foreach (TaskViewModel t in Tasks)
        {
            if (t.Id == task.Id)
            {
                taskViewModel = t;
                break;
            }
        }
        if (taskViewModel != null)
        {
            Tasks.Remove(taskViewModel);
        }
        OnPropertyChanged(nameof(Tasks));
    }

    public void SaveProject()
    {
        _project.Tasks.Clear();
        foreach (TaskViewModel taskVm in Tasks)
        {
            _project.Tasks.Add(taskVm.Task);
        }
        DataService.Instance.SaveProject(_project);
    }
    
    partial void OnTitleChanged(string value)
    {
        if (value != _project.Title) _project.Title = value;
    }
    
    partial void OnDescriptionChanged(string value)
    {
        if (value != _project.Description) _project.Description = value;
    }
    
}