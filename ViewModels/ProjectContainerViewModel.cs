using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Models;
using PlannerTool.Services;

namespace PlannerTool.ViewModels;

public class ProjectContainerViewModel : ViewModelBase
{
    public ObservableCollection<ProjectViewModel> Projects { get; } = new();

    private string _newProjectTitle = string.Empty;
    public string NewProjectTitle
    {
        get => _newProjectTitle;
        set => SetProperty(ref _newProjectTitle, value); // Assuming SetProperty is implemented in ViewModelBase
    }

    public ICommand AddProjectCommand { get; }

    public ProjectContainerViewModel()
    {
        // Load projects from DB
        foreach (Project project in DataService.Instance.GetProjects())
            Projects.Add(new ProjectViewModel(project));

        AddProjectCommand = new RelayCommand(AddProject, CanAddProject);
    }

    private bool CanAddProject() => !string.IsNullOrWhiteSpace(NewProjectTitle);

    private void AddProject()
    {
        var newProject = DataService.Instance.AddProject(NewProjectTitle);
        Projects.Add(new ProjectViewModel(newProject));
        NewProjectTitle = string.Empty; // Clear textbox after adding
    }
}