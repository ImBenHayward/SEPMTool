using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AwardPoints { get; set; }
        public ICollection<ProjectUser> Projects { get; set; }
        public ICollection<TaskUser> Tasks { get; set; }
        public ICollection<NotificationUser> Notifications { get; set; }

    }
}
