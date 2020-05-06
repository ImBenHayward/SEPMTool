using SEPMTool.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class NotificationViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public UpdateType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateTime { get; set; }
    }
}
