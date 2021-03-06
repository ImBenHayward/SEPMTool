﻿using SEPMTool.enums;
using System;
using System.Collections.Generic;

namespace SEPMTool.Models
{
    public class RequirementTask
    {
        public int Id { get; set; }
        public int ProjectRequirementId { get; set; }
        public ProjectRequirement ProjectRequirement { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public bool AwardEligbility { get; set; }
        public ICollection<SubTask> SubTasks { get; set; }
        public decimal Progress { get; set; }
        public ICollection<TaskUser> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public bool IsCompleted { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}