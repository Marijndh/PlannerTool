using System.Collections.Generic;

namespace PlannerTool.Models;

public class Project
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<ProjectTask> Tasks { get; set; }
    
}