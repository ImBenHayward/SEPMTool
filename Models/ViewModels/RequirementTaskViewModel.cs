using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskStatus = SEPMTool.enums.TaskStatus;

namespace SEPMTool.Models.ViewModels
{
    public class RequirementTaskViewModel
    {
        public int Id { get; set; }
        public int ProjectRequirementId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public IEnumerable<SubTaskViewModel> SubTasks { get; set; }
        public decimal Progress { get; set; }
        public IEnumerable<TaskUserViewModel> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
    }
}
