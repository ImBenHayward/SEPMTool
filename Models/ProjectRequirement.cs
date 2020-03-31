namespace SEPMTool.Models
{
    public class ProjectRequirement
    {
        public int ProjectRequirementId { get; set; }
        public Projects Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public RequirementCategory Category { get; set; }
    }
}