using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Models;
using PlannerTool.Services;

namespace PlannerTool.ViewModels;

public class ProjectContainerViewModel : ViewModelBase
{
    public ObservableCollection<ProjectViewModel> Projects { get; } = new();
    
    public ObservableCollection<object> ProjectsWithAdd { get; } = new();

    public IRelayCommand AddProjectCommand { get; }

    public ProjectContainerViewModel()
    {
        AddProjectCommand = new RelayCommand(AddProject);
        // Load projects from DB
        foreach (Project project in DataService.Instance.GetProjects())
            Projects.Add(new ProjectViewModel(project));

        AddProjectCommand = new RelayCommand(AddProject);

        // Initialize ProjectsWithAdd
        RefreshProjectsWithAdd();

        // Subscribe to Projects changes to update ProjectsWithAdd
        Projects.CollectionChanged += (s, e) => RefreshProjectsWithAdd();
    }

    private void RefreshProjectsWithAdd()
    {
        ProjectsWithAdd.Clear();

        foreach (var project in Projects)
            ProjectsWithAdd.Add(project);

        ProjectsWithAdd.Add(new AddProjectMarker(AddProjectCommand));
    }

    private void AddProject()
    {
        Project newProject = DataService.Instance.AddProject("New Project");
        Projects.Add(new ProjectViewModel(newProject));
    }
}