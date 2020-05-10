using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class RequirementViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public RequirementCategory Category { get; set; }
        public IEnumerable<RequirementTaskViewModel> Tasks { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public List<CommentViewModel> CommentsTree { get; set; }
    }
}
