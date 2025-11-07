using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PlannerTool.ViewModels;

namespace PlannerTool.Controls;

public partial class TaskTab : UserControl
{
    public TaskTab()
    {
        InitializeComponent();
    }
    
    private void OnTabPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        // Ignore clicks if the click started inside a child control like TextBox or Button
        if (e.Source is UserControl control && control != sender)
            return;

        if (DataContext is TaskViewModel vm && vm.HasSubTasks)
        {
            vm.ShowSubTasksCommand.Execute(null);
            e.Handled = true;
        }
    }

}