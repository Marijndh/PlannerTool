using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;

namespace PlannerTool.Controls;

public partial class TitleBar : UserControl
{
    public TitleBar()
    {
        InitializeComponent();
        PointerPressed += (_, e) =>
        {
            (VisualRoot as Window)?.BeginMoveDrag(e);
        };
    }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        (VisualRoot as Window)?.Close();
    }
}