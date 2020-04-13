using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectUserViewModel
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public ProjectRole ProjectRole { get; set; }
        public string Username { get; set; }
    }
}
