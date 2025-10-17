using System.Collections.Generic;

namespace PlannerTool.Models;

public class Project
{
    public int Id { get; private set; } 
    public string Title { get; set; }
    public string Description { get; set; } = "";
    public List<ProjectTask> Tasks { get; set; }
    
}