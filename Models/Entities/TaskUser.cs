﻿namespace SEPMTool.Models
{
    public class TaskUser
    {
        public int TaskId { get; set; }
        public RequirementTask Task { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public ApplicationUser User { get; set; }
    }
}