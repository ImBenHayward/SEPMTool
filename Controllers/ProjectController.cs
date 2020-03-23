using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SEPMTool.Alerts;
using SEPMTool.Data;
using SEPMTool.Models;
using SEPMTool.Models.ViewModels;

namespace SEPMTool.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public ProjectController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<ProjectUsersViewModel> GetAllUsers()
        {
            var users = userManager.Users;
            List<ProjectUsersViewModel> projectUsersViewModel = new List<ProjectUsersViewModel>();

            foreach (var user in users)
            {
                var userModel = new ProjectUsersViewModel
                {
                    UserId = user.Id,
                    Username = user.FirstName + " " + user.LastName
                };

                projectUsersViewModel.Add(userModel);
            }

            return projectUsersViewModel;
        }

        public IActionResult NewProject()
        {
            ProjectCreateViewModel projectCreateViewModel = new ProjectCreateViewModel
            {
                AllUsers = GetAllUsers()
            };

            return View(projectCreateViewModel);
        }

        public IActionResult Kanban()
        {
            return View();
        }

        public IActionResult EditRequirement(ProjectCreateViewModel project, int i)
        {
            var newRequirement = new ProjectRequirement
            {
                Name = project.ProjectRequirements[i].Name,
                Description = project.ProjectRequirements[i].Description,
                Category = project.ProjectRequirements[i].Category,
                Priority = project.ProjectRequirements[i].Priority
            };
            //project.NewRequirement.Name = project.ProjectRequirements[i].Name;
            //project.NewRequirement.Description = project.ProjectRequirements[i].Description;
            //project.NewRequirement.Category = project.ProjectRequirements[i].Category;
            //project.NewRequirement.Priority = project.ProjectRequirements[i].Priority;

            project.NewRequirement = newRequirement;
            project.RequirementIndex = i;

            project.AllUsers = GetAllUsers();
            project.ShowRequirements = true;
            project.ShowModal = true;

            ModelState.Clear();

            return View("NewProject", project);
        }

        public IActionResult SaveRequirement(ProjectCreateViewModel project)
        {
            var projectRequirement = new ProjectRequirement
            {
                Name = project.NewRequirement.Name,
                Description = project.NewRequirement.Description,
                Category = project.NewRequirement.Category,
                Priority = project.NewRequirement.Priority
            };

            ModelState.Clear();

            project.AllUsers = GetAllUsers();
            project.ProjectRequirements[project.RequirementIndex] = projectRequirement;

            project.NewRequirement = new ProjectRequirement();

            return View("NewProject", project);
        }

        public IActionResult AddRequirement(ProjectCreateViewModel project)
        {
            var projectRequirement = new ProjectRequirement
            {
                Name = project.AddRequirement.Name,
                Description = project.AddRequirement.Description,
                Category = project.AddRequirement.Category,
                Priority = project.AddRequirement.Priority
            };

            project.AddRequirement = new ProjectRequirement();

            project.AllUsers = GetAllUsers();
            project.ProjectRequirements.Add(projectRequirement);

            ModelState.Clear();

            return View("NewProject", project);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateViewModel project)
        {

            List<ProjectUser> selectedUsers = project.AllUsers.Where(u => u.IsSelected).Select(u => new ProjectUser { UserId = u.UserId, ProjectRole = ProjectRole.Developer }).ToList();

            Project proj = new Project
            {
                Name = project.Name,
                Description = project.Description,
                Priority = project.Priority,
                Status = enums.Status.Active,
                AwardEligibility = true,
                ProjectRequirements = project.ProjectRequirements,
                Progress = 0,
                Users = selectedUsers,
                StartDate = project.StartDate,
                Deadline = project.Deadline,
                EstimatedCompletionDate = DateTime.Now
            };

            _context.Projects.Add(proj);

            //if (!ModelState.IsValid)
            //{
            //    this.AddAlertDanger($"{project.Name} was not created, model not valid, please try again.");
            //    return View("NewProject", project);
            //}

            if (await _context.SaveChangesAsync() > 0)
            {
                this.AddAlertSuccess($"{project.Name} was created successfully");
                return RedirectToAction("Index");
            }
            else
            {
                this.AddAlertDanger($"{project.Name} was not created, please try again later.");
                return View("Index");
            }
        }

    }
}