using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SEPMTool.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProjectRequirement> ProjectRequirements { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public bool AwardEligibility { get; set; }
        public decimal Progress { get; set; }
        public ICollection<ProjectUser> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
    }
}
