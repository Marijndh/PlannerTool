using System.Threading.Tasks;
using PlannerTool.Models;

namespace PlannerTool.ViewModels;

public class TaskViewModel : ViewModelBase
{
    private readonly ProjectTask _task;

    public int Id => _task.Id;
    public string Title
    {
        get => _task.Title;
        set
        {
            if (_task.Title != value)
            {
                _task.Title = value;
                OnPropertyChanged();
            }
        }
    }
    
    public TaskViewModel(ProjectTask task)
    {
        _task = task;
    }
}