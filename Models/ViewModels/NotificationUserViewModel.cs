using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class NotificationUserViewModel
    {
        public string UserId { get; set; }
        public int NotificationId { get; set; }
        public NotificationViewModel Notification { get; set; }
        public bool IsRead { get; set; }
    }
}
