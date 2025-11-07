using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using PlannerTool.ViewModels;

namespace PlannerTool;

public partial class MainWindow : Window
{
    private const double AspectRatio = 2.0 / 3.0;

    public MainWindow()
    {
        InitializeComponent();

        // Handle DataContext changes safely
        DataContextChanged += (_, _) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                // Unsubscribe previous handlers if necessary
                vm.RequestClose -= OnRequestClose;
                vm.RequestClose += OnRequestClose;
            }
        };
    }

    private void OnRequestClose()
    {
        // Always close on the UI thread
        Dispatcher.UIThread.Post(() => Close());
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        if (e.WidthChanged)
        {
            ClientSize = new Size(e.NewSize.Width, e.NewSize.Width / AspectRatio);
        }
        else if (e.HeightChanged)
        {
            ClientSize = new Size(e.NewSize.Height * AspectRatio, e.NewSize.Height);
        }
    }
}