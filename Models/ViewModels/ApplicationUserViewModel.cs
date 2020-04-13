using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<ProjectUserViewModel> Projects { get; set; }
        public IEnumerable<TaskUserViewModel> Tasks { get; set; }
    }
}
