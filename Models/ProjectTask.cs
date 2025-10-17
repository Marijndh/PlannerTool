using System.Collections.Generic;

namespace PlannerTool.Models;

public class ProjectTask
{
    public string Title { get; set; }
    
    public List<ProjectTask> SubTasks { get; set; }
    
    public ProjectTask(string title)
    {
        Title = title;
        SubTasks = new List<ProjectTask>();
    }
}