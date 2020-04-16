using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<RequirementViewModel> ProjectRequirements { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public decimal Progress { get; set; }
        public IEnumerable<ProjectUserViewModel> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public IEnumerable<ProjectUpdateViewModel> Updates { get; set; }
    }
}
