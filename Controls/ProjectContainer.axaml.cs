using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace PlannerTool.Controls;

public partial class ProjectContainer : UserControl
{
    public ProjectContainer()
    {
        InitializeComponent();
    }

    private void UserControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Focus();
    }
}