using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlannerTool.Models;

namespace PlannerTool.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Project> Projects { get; } = new ObservableCollection<Project>
    {
        new Project { Title = "Project Alpha", Description = "Description for Project Alpha" },
        new Project { Title = "Project Beta", Description = "Description for Project Beta" },
        new Project { Title = "Project Gamma", Description = "Description for Project Gamma" }
    };
    
    public IEnumerable<object> ProjectsWithAdd
    {
        get
        {
            foreach (var project in Projects)
                yield return project;

            // Always append a special "Add" marker
            yield return new AddProjectMarker();
        }
    }

    public void AddProject()
    {
        Projects.Add(new Project { Title = "New Project" });
        // Notify ProjectsWithAdd changed if needed (INotifyPropertyChanged)
        OnPropertyChanged(nameof(ProjectsWithAdd));
    }
}
