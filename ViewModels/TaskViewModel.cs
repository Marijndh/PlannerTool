using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Models;

namespace PlannerTool.ViewModels;

public class TaskViewModel : ViewModelBase
{
    private readonly ProjectTask _task;
    
    public ProjectTask? Task => _task;

    public int Id => _task.Id;
    public string Title
    {
        get => _task.Title;
        set
        {
            if (_task.Title != value)
            {
                _task.Title = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ObservableCollection<ProjectTask> SubTasks
    {
        get => new (_task.SubTasks);
        set
        {
            if (_task.SubTasks != value.ToList())
            {
                _task.SubTasks = value.ToList();
                OnPropertyChanged();
            }
        }
    }
    
    public bool HasSubTasks => SubTasks.Count > 0;
    public bool ShowSubTasks { get; set; }
    
    public string ShowSubTasksButtonContent => ShowSubTasks ? "↑" : "↓";
    
    public IRelayCommand ShowSubTasksCommand { get; }
    
    private readonly Action<ProjectTask> _deleteTaskAction;
    
    public TaskViewModel(ProjectTask task,  Action<ProjectTask> deleteTaskAction)
    {
        _task = task;
        SubTasks.CollectionChanged += (_, _) => OnPropertyChanged(nameof(HasSubTasks));
        ShowSubTasksCommand = new RelayCommand(OpenSubtasks);
        _deleteTaskAction = deleteTaskAction;
    }
    
    private void OpenSubtasks()
    {
        ShowSubTasks = !ShowSubTasks;
        OnPropertyChanged(nameof(ShowSubTasks));
        OnPropertyChanged(nameof(ShowSubTasksButtonContent));
    }

    public void DeleteTask()
    {
        _deleteTaskAction.Invoke(_task);
    }
}