using System;
using System.Collections.Generic;
using PlannerTool.Enums;
using PlannerTool.Services;

namespace PlannerTool.Models;

public class ProjectTask
{
    public int Id { get; private set; }
    public string Title { get; set; }
    
    public List<ProjectTask> SubTasks { get; set; } = new();

    public CompletionState State { get; set; } = CompletionState.NotStarted;

    public DateTimeOffset? Deadline { get; set; }
    
}