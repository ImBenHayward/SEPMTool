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

        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUser { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<TaskUser> TaskUser { get; set; }
        public DbSet<ProjectRequirement> Requirements { get; set; }

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

            builder.Entity<Projects>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project);

            builder.Entity<ProjectTask>()
                .HasMany(p => p.SubTasks)
                .WithOne(s => s.ProjectTask);

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

            builder.Entity<Projects>()
                .HasMany(p => p.ProjectRequirements)
                .WithOne(r => r.Project);


        }
    }
}
