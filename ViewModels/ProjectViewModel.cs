using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Models;

namespace PlannerTool.ViewModels;
public class ProjectViewModel : ViewModelBase
{
    public IRelayCommand OpenProjectCommand { get; }
    
    private readonly Project _project;
    private readonly Action<ProjectViewModel>? _openProjectAction;

    public int Id => _project.Id;
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
    
    public List<ProjectTask> Tasks => _project.Tasks;
    
    public ProjectViewModel(Project project, Action<ProjectViewModel>? openProjectAction = null)
    {
        _project = project;
        _openProjectAction = openProjectAction;
        OpenProjectCommand = new RelayCommand(OpenProject);
    }

    // Constructor for default project
    public ProjectViewModel()
    {
        _project = new Project
        {
            Title = "New Project"
        };
        OpenProjectCommand = new RelayCommand(OpenProject);
    }

    public void OpenProject()
    {
        _openProjectAction?.Invoke(this);
    }
}