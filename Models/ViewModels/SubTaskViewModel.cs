using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class SubTaskViewModel
    {
        //public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectTaskId { get; set; }
    }
}
