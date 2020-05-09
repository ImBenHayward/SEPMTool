using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string UserId { get; set; }
        public string CommentBody { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
        public ProjectRequirement Requirement { get; set; }
        public RequirementTask Task { get; set; }

    }
}
