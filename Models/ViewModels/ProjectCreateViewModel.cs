using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectCreateViewModel
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
        public List<ProjectUsersViewModel> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public List<ProjectUsersViewModel> AllUsers { get; set; }
    }
}
