using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PlannerTool.Models;
using PlannerTool.ViewModels;

namespace PlannerTool.Controls;

public partial class ProjectContainer : UserControl
{
    public ProjectContainer()
    {
        InitializeComponent();
    }

    private void AddProjectClicked(object? sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel)DataContext!;
        vm.Projects.Add(new Project { Title = "New Project", Description = ""});
    }
}