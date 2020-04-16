using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectUpdateViewModel
    {
        public int Id { get; set; }
        //public ProjectViewModel Project { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UpdateType Type { get; set; }
        public DateTime Date { get; set; }
    }
}
