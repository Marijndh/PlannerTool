using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Services;
using System.Collections.ObjectModel;
using PlannerTool.Models;

namespace PlannerTool.ViewModels;

public class ProjectContainerViewModel : ViewModelBase
{
    public ObservableCollection<ProjectViewModel> Projects { get; } = new();

    public IRelayCommand AddProjectCommand { get; }

    public ProjectContainerViewModel()
    {
        // Load projects from DB
        foreach (var project in DataService.Instance.GetProjects())
            Projects.Add(new ProjectViewModel(project));
        AddProjectCommand = new RelayCommand(AddProject);
    }
    
    public IEnumerable<object> ProjectsWithAdd
    {
        get
        {
            foreach (var project in Projects)
                yield return project;

            // Always append a special "Add" marker
            yield return new AddProjectMarker();
        }
    }

    private void AddProject()
    {
        var newProject = DataService.Instance.AddProject("New Project");
        Projects.Add(new ProjectViewModel(newProject));
    }
}