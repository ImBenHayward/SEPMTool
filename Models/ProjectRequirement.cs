using System.Collections.Generic;

namespace SEPMTool.Models
{
    public class ProjectRequirement
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public RequirementCategory Category { get; set; }
        public ICollection<RequirementTask> Tasks { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}