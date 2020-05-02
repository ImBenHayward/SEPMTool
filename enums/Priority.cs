using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool
{
    public enum Priority
    {
        [Display(Name = "Very Low")]
        VeryLow,
        Low,
        Medium,
        High,
        [Display(Name = "Very High")]
        VeryHigh
    }
}
