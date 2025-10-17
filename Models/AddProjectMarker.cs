using CommunityToolkit.Mvvm.Input;

namespace PlannerTool.Models;

public class AddProjectMarker
{
    public IRelayCommand AddProjectCommand { get; }
    
    public AddProjectMarker(IRelayCommand addProjectCommand)
    {
        AddProjectCommand = addProjectCommand;
    }
}