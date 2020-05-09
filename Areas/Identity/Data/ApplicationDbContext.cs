using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SEPMTool.Models;

namespace SEPMTool.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUser { get; set; }
        public DbSet<RequirementTask> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<TaskUser> TaskUser { get; set; }
        public DbSet<ProjectRequirement> Requirements { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUser { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });
            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.Users)
                .HasForeignKey(pu => pu.ProjectId);
            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(pu => pu.UserId);

            builder.Entity<ProjectRequirement>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.ProjectRequirement);

            builder.Entity<ProjectRequirement>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Requirement);

            builder.Entity<RequirementTask>()
                .HasMany(p => p.SubTasks)
                .WithOne(s => s.ProjectTask)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RequirementTask>()
                .HasMany(p => p.Comments)
                .WithOne(s => s.Task);

            builder.Entity<TaskUser>()
                .HasKey(tu => new { tu.TaskId, tu.UserId });
            builder.Entity<TaskUser>()
                .HasOne(tu => tu.Task)
                .WithMany(t => t.Users)
                .HasForeignKey(tu => tu.TaskId);
            builder.Entity<TaskUser>()
                .HasOne(tu => tu.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(u => u.UserId);

            builder.Entity<Project>()
                .HasMany(p => p.ProjectRequirements)
                .WithOne(r => r.Project);

            builder.Entity<Project>()
                .HasMany(p => p.Updates)
                .WithOne(r => r.Project);

            builder.Entity<NotificationUser>()
                .HasKey(nu => new { nu.NotificationId, nu.UserId });
            builder.Entity<NotificationUser>()
                .HasOne(nu => nu.User)
                .WithMany(n => n.Notifications)
                .HasForeignKey(nu => nu.UserId);
            builder.Entity<NotificationUser>()
                .HasOne(nu => nu.Notification)
                .WithMany(u => u.Users)
                .HasForeignKey(nu => nu.NotificationId);

        }
    }
}
