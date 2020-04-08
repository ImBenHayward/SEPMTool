using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public int RequirementId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public List<SubTask> SubTasks { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public List<TaskUsersViewModel> Users { get; set; }
        public List<ProjectUsersViewModel> AllUsers { get; set; }
    }
}
