using System.Collections.ObjectModel;
using System.Linq;
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
    
    public ObservableCollection<ProjectTask> SubTasks
    {
        get => new (_task.SubTasks);
        set
        {
            if (_task.SubTasks != value.ToList())
            {
                _task.SubTasks = value.ToList();
                OnPropertyChanged();
            }
        }
    }
    
    public TaskViewModel(ProjectTask task)
    {
        _task = task;
    }
}