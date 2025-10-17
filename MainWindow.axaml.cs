using Avalonia;
using Avalonia.Controls;

namespace PlannerTool.Views;

public partial class MainWindow : Window
{
    private const double aspectRatio = 2.0 / 3.0;

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        if (e.WidthChanged)
        {
            ClientSize = new Size(e.NewSize.Width, e.NewSize.Width / aspectRatio);
        }
        else if (e.HeightChanged)
        {
            ClientSize = new Size(e.NewSize.Height * aspectRatio, e.NewSize.Height);
        }
    }

    public MainWindow()
    {
        InitializeComponent();
    }
}