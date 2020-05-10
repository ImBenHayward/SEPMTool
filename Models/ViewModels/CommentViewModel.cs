using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string UserId { get; set; }
        public string CurrentUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CommentBody { get; set; }
        public DateTime DateTime { get; set; }
    }
}
