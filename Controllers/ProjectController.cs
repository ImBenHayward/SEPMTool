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

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateViewModel project)
        {

            List<ProjectUser> selectedUsers = project.AllUsers.Where(u => u.IsSelected).Select(u => new ProjectUser { UserId = u.UserId }).ToList();

            Project proj = new Project
            {
                Name = project.Name,
                Description = project.Description,
                Priority = project.Priority,
                Status = enums.Status.Active,
                AwardEligibility = true,
                Progress = 0,
                Users = selectedUsers,
                StartDate = project.StartDate,
                Deadline = project.Deadline,
                EstimatedCompletionDate = DateTime.Now
            };

            _context.Projects.Add(proj);

            if(await _context.SaveChangesAsync() > 0)
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