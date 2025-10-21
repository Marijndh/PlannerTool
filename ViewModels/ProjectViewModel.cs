using System;
using System.Collections.ObjectModel;
using PlannerTool.Models;
using PlannerTool.Services;

namespace PlannerTool.ViewModels;
public class ProjectViewModel : ViewModelBase
{
   
    private readonly Project _project;
    private readonly Action<ProjectViewModel>? _openProjectAction;
    
    public int Id => _project.Id;
    
    private string _newTaskTitle = string.Empty;
    public string NewTaskTitle
    {
        get => _newTaskTitle;
        set => SetProperty(ref _newTaskTitle, value);
    }
    public string Title
    {
        get => _project.Title;
        set
        {
            if (_project.Title != value)
            {
                _project.Title = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();
    
    public ProjectViewModel(Project project, Action<ProjectViewModel>? openProjectAction = null)
    {
        _project = project;
        _openProjectAction = openProjectAction;
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
            if (taskVm.Task != null) _project.Tasks.Add(taskVm.Task);
        }
        DataService.Instance.SaveProject(_project);
    }
    
}