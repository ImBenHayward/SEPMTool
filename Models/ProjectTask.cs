﻿using SEPMTool.enums;
using System;
using System.Collections.Generic;

namespace SEPMTool.Models
{
    public class ProjectTask
    {
        public int ProjectTaskId { get; set; }
        public Projects Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public bool AwardEligbility { get; set; }
        public decimal Progress { get; set; }
        public ICollection<TaskUser> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
    }
}