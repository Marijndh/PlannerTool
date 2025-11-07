using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Models;
using PlannerTool.Services;

namespace PlannerTool.ViewModels;

public partial class ProjectContainerViewModel : ViewModelBase
{
    private readonly Action<ProjectViewModel> _openProjectAction;
    
    private Action<ProjectViewModel> DeleteProjectAction => DeleteProject;

    [ObservableProperty] 
    private ObservableCollection<ProjectViewModel> _projects;

    [ObservableProperty]
    private string _newProjectTitle;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ModeIcon))]
    private bool _deleteEnabled;

    public string ModeIcon => DeleteEnabled ? "\xEBA6" : "\xE4A6";

    public ProjectContainerViewModel(Action<ProjectViewModel> openProjectAction)
    {
        NewProjectTitle = string.Empty;
        _openProjectAction = openProjectAction;
        Projects = new ObservableCollection<ProjectViewModel>();
        DeleteEnabled = false;

        foreach (Project project in DataService.Instance.GetProjects())
        {
            Projects.Add(new ProjectViewModel(project, _openProjectAction, DeleteProjectAction));
        }
    }

    private bool CanAddProject() => !string.IsNullOrWhiteSpace(NewProjectTitle);

    public void AddProject()
    {
        Project newProject = DataService.Instance.AddProject(NewProjectTitle);
        Projects.Add(new ProjectViewModel(newProject, _openProjectAction, DeleteProjectAction));
        NewProjectTitle = string.Empty;
    }
    
    private void DeleteProject(ProjectViewModel projectVm)
    {
        DataService.Instance.DeleteProject(projectVm.Id);
        Projects.Remove(projectVm);
    }
    
    public void ChangeMode()
    {
        DeleteEnabled = !DeleteEnabled;
    }

    partial void OnDeleteEnabledChanged(bool value)
    {
        foreach (ProjectViewModel project in Projects)
            project.DeleteEnabled = value;
    }
}