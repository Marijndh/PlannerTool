using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlannerTool.Models;

namespace PlannerTool.ViewModels;
public class ProjectViewModel : ViewModelBase
{
    public IRelayCommand OpenProjectCommand { get; }
    
    private readonly Project _project;

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
    
    public ProjectViewModel(Project project)
    {
        _project = project;
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

    private void OpenProject()
    {
        throw new System.NotImplementedException();
    }
}