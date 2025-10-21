using Avalonia.Controls;
using Avalonia.Interactivity;
using PlannerTool.ViewModels;

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
}