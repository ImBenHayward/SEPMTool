using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public UpdateType Type { get; set; }
        public ICollection<NotificationUser> Users { get; set; }
        public DateTime DateTime { get; set; }
        public string UserLink { get; set; }
        public int ProjectLink { get; set; }

    }
}
