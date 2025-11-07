using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Enums;
using PlannerTool.Models;
using PlannerTool.Services;

namespace PlannerTool.ViewModels;

public partial class TaskViewModel : ViewModelBase
{
    private readonly ProjectTask _task;
    private readonly Action<ProjectTask> _deleteTaskAction;
    public IRelayCommand ShowSubTasksCommand { get; }

    public ProjectTask Task => _task;

    public int Id => _task.Id;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private bool _showSubTasks;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(StateColor))]
    private CompletionState _state;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DeadlineColor))]
    [NotifyPropertyChangedFor(nameof(HasDeadline))]
    private DateTime? _deadline;
    
    [ObservableProperty]
    private string _progressText;

    // When SubTasks changes, automatically notify that HasSubTasks and Padding changed.
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSubTasks))]
    private ObservableCollection<TaskViewModel> _subTasks = new();

    public bool HasSubTasks => SubTasks.Count > 0;
    
    public bool HasDeadline => Deadline != null;
    
    private int CompletedSubTasks => SubTasks.Count(t => t.State == CompletionState.Completed);
    
    public IBrush StateColor => GetStateColor();
    public IBrush DeadlineColor => GetDeadlineColor();
    public string DeadlineCountdown => GetDeadlineCountdown();

    private readonly DispatcherTimer _deadlineTimer;
    
    public TaskViewModel(ProjectTask task, Action<ProjectTask> deleteTaskAction)
    {
        _task = task;
        _deleteTaskAction = deleteTaskAction;

        Title = task.Title;
        State = task.State;
        Deadline = task.Deadline?.DateTime ?? null;
        
        SubTasks.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(HasSubTasks));
        };
        
        foreach (TaskViewModel subTaskViewModel in _task.SubTasks.Select(subTask => new TaskViewModel(subTask, DeleteSubTask)))
        {
            subTaskViewModel.PropertyChanged += SubTaskPropertyChanged;
            SubTasks.Add(subTaskViewModel);
        }
        SubTasks.CollectionChanged += SubTasksOnCollectionChanged;
        
        ShowSubTasksCommand = new RelayCommand(ToggleSubtasks);
        
        _deadlineTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _deadlineTimer.Tick += (_, _) => OnPropertyChanged(nameof(DeadlineCountdown));
        _deadlineTimer.Start();
        
        UpdateProgress();
    }

    private void ToggleSubtasks()
    {
        ShowSubTasks = !ShowSubTasks;
    }

    public void AddSubTask()
    {
        ProjectTask subTask = new ProjectTask { Title = "New..." };
        TaskViewModel subTaskViewModel = new TaskViewModel(subTask, DeleteSubTask);

        SubTasks.Add(subTaskViewModel);
        _task.SubTasks.Add(subTask);
        if (!ShowSubTasks) ToggleSubtasks();
    }

    private void DeleteSubTask(ProjectTask task)
    {
        TaskViewModel? taskViewModel = SubTasks.FirstOrDefault(t => t.Id == task.Id);
        if (taskViewModel != null)
        {
            SubTasks.Remove(taskViewModel);
            _task.SubTasks.Remove(task);
        }
    }

    public void DeleteTask()
    {
        _deleteTaskAction.Invoke(_task);
    }

    partial void OnTitleChanged(string value)
    {
        if (value != _task.Title) _task.Title = value;
    }
    
    partial void OnStateChanged(CompletionState value)
    {
        if (value != _task.State) _task.State = value;
    }

    partial void OnDeadlineChanged(DateTime? value)
    {
        if (value != null)
        {
            DateTime localDateTime = DateTime.SpecifyKind(value.Value, DateTimeKind.Local);
            _task.Deadline = new DateTimeOffset(localDateTime);
        }
        else
        {
            _task.Deadline = null;
        }
    }
    private void SubTasksOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (TaskViewModel task in e.NewItems)
                task.PropertyChanged += SubTaskPropertyChanged;
        }

        if (e.OldItems != null)
        {
            foreach (TaskViewModel task in e.OldItems)
                task.PropertyChanged -= SubTaskPropertyChanged;
        }

        UpdateProgress();
    }
    
    private void SubTaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State))
        {
            UpdateProgress();
        }
    }

    public void ChangeState()
    {
        State = State switch
        {
            CompletionState.NotStarted => CompletionState.InProgress,
            CompletionState.InProgress => CompletionState.Completed,
            _ => CompletionState.NotStarted
        };
    }
    
    private IBrush GetStateColor()
    {
        switch (State)
        {
            case CompletionState.NotStarted:
                return Brushes.Red;
            case CompletionState.InProgress:
                return Brushes.Orange;
            case CompletionState.Completed:
                return Brushes.Green;
            default:
                return Brushes.Gray;
        }
    }
    
    private IBrush GetDeadlineColor()
    {
        if (Deadline == null)
            return Brushes.Gray;

        var diff = Deadline.Value - DateTime.Now;

        if (diff.TotalSeconds < 0)
            return Brushes.DarkRed;

        switch (diff.TotalDays)
        {
            case < 1:
                return Brushes.Red;
            case < 7:
                return Brushes.DarkGoldenrod;
            default:
                return Brushes.Gray;
        }
    }
    
    private string GetDeadlineCountdown()
    {
        if (Deadline == null)
            return "No deadline set";

        var diff = Deadline.Value - DateTime.Now;

        if (diff.TotalSeconds < 0)
            return "Deadline passed";

        if (diff.TotalDays >= 1)
            return $"{(int)diff.TotalDays} days left";
        if (diff.TotalHours >= 1)
            return $"{(int)diff.TotalHours} hours left";
        if (diff.TotalMinutes >= 1)
            return $"{(int)diff.TotalMinutes} minutes left";

        return "Less than a minute left";
    }
    
    private void UpdateProgress()
    {
        if (HasSubTasks)
        {
            ProgressText = $"{CompletedSubTasks}/{SubTasks.Count}";

            // Update State based on completed subtasks
            if (CompletedSubTasks == 0)
            {
                State = CompletionState.NotStarted;
            }
            else if (CompletedSubTasks == SubTasks.Count)
            {
                State = CompletionState.Completed;
            }
            else
            {
                State = CompletionState.InProgress;
            }
        }
    }

}
