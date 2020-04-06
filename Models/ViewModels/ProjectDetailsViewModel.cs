using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public Projects Project { get; set; }
        public int ProjectId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public List<SubTask> SubTasks { get; set; }
        public List<TaskUsersViewModel> Users { get; set; }
    }
}
