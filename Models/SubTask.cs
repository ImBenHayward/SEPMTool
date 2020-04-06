using SEPMTool.Models.ViewModels;
using System.Collections.Generic;

namespace SEPMTool.Models
{
    public class SubTask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public ProjectTask ProjectTask { get; set; }
    }
}