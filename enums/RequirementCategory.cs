using System.ComponentModel.DataAnnotations;

namespace SEPMTool.Models
{
    public enum RequirementCategory
    {
        Functional,
        [Display(Name = "Non-Functional")]
        NonFunctional,
        Domain
    }
}