using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PlannerTool.Models;

namespace PlannerTool.Controls;

public partial class ProjectTab : UserControl
{
    public static readonly StyledProperty<Project> ProjectProperty =
        AvaloniaProperty.Register<ProjectTab, Project>(nameof(Project));

    public Project Project
    {
        get => GetValue(ProjectProperty);
        set => SetValue(ProjectProperty, value);
    }

    public ProjectTab()
    {
        InitializeComponent();
    }

    private void OpenProject(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
