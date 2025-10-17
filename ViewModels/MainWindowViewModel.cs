using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using PlannerTool.Models;

namespace PlannerTool.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ProjectContainerViewModel ProjectContainer { get; } = new();

    public MainWindowViewModel()
    {

    }
}
