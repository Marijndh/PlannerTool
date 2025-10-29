using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PlannerTool.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanClosePage))]
    private ViewModelBase _currentViewModel;

    private ProjectContainerViewModel ProjectContainer { get; }

    public IRelayCommand CloseProjectCommand { get; }
    public IRelayCommand CloseWindowCommand { get; }
    
    public event Action? RequestClose;

    [RelayCommand]
    private void Close()
    {
        RequestClose?.Invoke();
    }
    
    public bool CanClosePage => CurrentViewModel != ProjectContainer;

    public MainWindowViewModel()
    {
        ProjectContainer = new ProjectContainerViewModel(OpenProject);
        CurrentViewModel = ProjectContainer;

        CloseProjectCommand = new RelayCommand(CloseProject);
        CloseWindowCommand = new RelayCommand(() =>
        {
            OnClosing();
            Close();
        });
    }

    private void OpenProject(ProjectViewModel project)
    {
        CurrentViewModel = project;
    }

    private void CloseProject()
    {
        OnClosing();
        CurrentViewModel = ProjectContainer;
    }
    
    private void OnClosing()
    {
        if (CurrentViewModel is ProjectViewModel projectVm)
        {
            projectVm.SaveProject(); 
        }
    }
}