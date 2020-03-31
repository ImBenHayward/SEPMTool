namespace SEPMTool.Models
{
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public Projects Project { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ProjectRole ProjectRole { get; set; }
        public string Username { get; set; }
    }
}