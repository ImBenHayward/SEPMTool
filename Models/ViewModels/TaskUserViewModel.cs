using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class TaskUserViewModel
    {
        public int TaskId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        //public ApplicationUserViewModel User { get; set; }
    }
}
