using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Alerts
{
    public class Alert
    {
        public string Message;
        public string Type;

        public Alert(string message, string type)
        {
            Message = message;
            Type = type;
        }
    }
}
