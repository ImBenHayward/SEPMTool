using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskStatus = SEPMTool.enums.TaskStatus;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public ProjectViewModel Project { get; set; }
        public int ProjectId { get; set; }
        public int RequirementId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public List<SubTaskViewModel> SubTasks { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public List<TaskUsersViewModel> Users { get; set; }
        public List<ProjectUsersViewModel> AllUsers { get; set; }

        public class CreateTaskResponse
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
            public ProjectUpdate Update { get; set; }
        }

        public class UpdateSubTaskResponse
        {
            public int Id { get; set; }
            public bool IsCompleted { get; set; }
        }
    }
}
