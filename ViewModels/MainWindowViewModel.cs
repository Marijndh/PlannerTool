using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PlannerTool.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanClosePage))]
    private ViewModelBase _currentViewModel;

    public ProjectContainerViewModel ProjectContainer { get; }

    public IRelayCommand CloseProjectCommand { get; }
    
    public bool CanClosePage => CurrentViewModel != ProjectContainer;

    public MainWindowViewModel()
    {
        ProjectContainer = new ProjectContainerViewModel(OpenProject);
        CurrentViewModel = ProjectContainer;

        CloseProjectCommand = new RelayCommand(CloseProject);
    }

    private void OpenProject(ProjectViewModel project)
    {
        CurrentViewModel = project;
    }

    private void CloseProject()
    {
        CurrentViewModel = ProjectContainer;
    }
}