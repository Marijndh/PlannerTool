using System.Collections.Generic;

namespace PlannerTool.Models;

public class ProjectTask
{
    public int Id { get; private set; }
    public string Title { get; set; }
    
    public List<ProjectTask> SubTasks { get; set; }
    
}