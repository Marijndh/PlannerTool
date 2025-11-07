using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlannerTool.Enums;
using PlannerTool.Models;
using PlannerTool.Services;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;

namespace PlannerTool.ViewModels;
public partial class ProjectViewModel : ViewModelBase
{
   
    private readonly Project _project;
    private readonly Action<ProjectViewModel>? _openProjectAction;
    private readonly Action<ProjectViewModel>? _deleteProjectAction;
    
    public int Id => _project.Id;
    
    [ObservableProperty]
    private string _newTaskTitle;
    
    [ObservableProperty]
    private string _title;
    
    [ObservableProperty]
    private string _description;
    
    [ObservableProperty]
    private ObservableCollection<TaskViewModel> _tasks;
    
    [ObservableProperty]
    private string _progressText;

    [ObservableProperty]
    private IBrush _progressColor;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTasks))]
    private bool _deleteEnabled;
    
    private int CompletedTasks => Tasks.Count(t => t.State == CompletionState.Completed);
    
    public bool HasTasks => Tasks.Count > 0 && !DeleteEnabled;

    public ProjectViewModel(Project project,
        Action<ProjectViewModel> openProjectAction,  Action<ProjectViewModel> deleteProjectAction)
    {
        _project = project;
        _openProjectAction = openProjectAction;
        _deleteProjectAction = deleteProjectAction;
        
        Title = project.Title;
        Description = project.Description;
        Tasks = new ObservableCollection<TaskViewModel>();
        NewTaskTitle = string.Empty;
        
        foreach (ProjectTask task in project.Tasks)
        {
            TaskViewModel taskVm = new TaskViewModel(task, DeleteTask);
            taskVm.PropertyChanged += TaskPropertyChanged;
            Tasks.Add(taskVm);
        }
        Tasks.CollectionChanged += TasksOnCollectionChanged;

        // Initialize progress
        UpdateProgress();
    }
    
    private void TasksOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (TaskViewModel task in e.NewItems)
                task.PropertyChanged += TaskPropertyChanged;
        }

        if (e.OldItems != null)
        {
            foreach (TaskViewModel task in e.OldItems)
                task.PropertyChanged -= TaskPropertyChanged;
        }

        UpdateProgress();
    }

    private void TaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TaskViewModel.State))
        {
            UpdateProgress();
        }
    }

    public void OpenProject()
    {
        _openProjectAction?.Invoke(this);
    }

    public void DeleteProject()
    {
        _deleteProjectAction.Invoke(this);
    }
    
    public void AddTask()
    {
        ProjectTask newTask = new ProjectTask {Title = NewTaskTitle};
        Tasks.Add(new TaskViewModel(newTask,DeleteTask));
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
    
    private void UpdateProgress()
    {
        if (Tasks.Count == 0)
        {
            ProgressText = string.Empty;
            ProgressColor = Brushes.Gray;
            return;
        }

        ProgressText = $"{CompletedTasks}/{Tasks.Count}";

        double half = Tasks.Count / 2.0;

        if (CompletedTasks == Tasks.Count)
            ProgressColor = Brushes.Green;
        else if (CompletedTasks >= half)
            ProgressColor = Brushes.Orange;
        else
            ProgressColor = Brushes.Red;
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