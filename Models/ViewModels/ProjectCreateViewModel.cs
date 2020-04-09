using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class ProjectCreateViewModel
    {
        public ProjectCreateViewModel()
        {
            ProjectRequirements = new List<ProjectRequirement>();
        }
        public int ProjectId { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required(ErrorMessage = "Project name is required")]
        public string Name { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        //[Required(ErrorMessage = "Project requirements are required")]
        public List<ProjectRequirement> ProjectRequirements { get; set; }
        public Priority Priority { get; set; }
        public List<ProjectUsersViewModel> Users { get; set; }
        [Required(ErrorMessage = "Project start date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Project deadline is required")]
        public DateTime Deadline { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public List<ProjectUsersViewModel> AllUsers { get; set; }
        public int RequirementIndex { get; set; }
    }
}
