using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEPMTool.Alerts;
using SEPMTool.Data;
using SEPMTool.Models;
using SEPMTool.Models.ViewModels;
using cloudscribe.Pagination.Models;

namespace SEPMTool.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public ProjectsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
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

        public IActionResult Index(int pageNumber=1, int pageSize = 10)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;

            var model = new ProjectIndexViewModel();

            var projects = _context.Projects
                .Include(u => u.Users)
                .Include(r => r.ProjectRequirements)
                .Include(r => r.Tasks)
                .Skip(excludeRecords)
                .Take(pageSize);

            PagedResult<Projects> result = new PagedResult<Projects>
            {
                Data = projects.AsNoTracking().ToList(),
                TotalItems = _context.Projects.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(result);
        }

        public IActionResult Details(int id)
        {
            if (_context.Projects.Any(p => p.Id == id))
            {
                var project = _context.Projects
                    .Include(u => u.Users)
                    .Include(r => r.ProjectRequirements)
                    .Include(r => r.Tasks)
                    .First(p => p.Id == id);

                var model = new ProjectDetailsViewModel()
                {
                    Project = project
                };

                return View(model);
            }

            this.AddAlertDanger($"The project with ID: {id} was not found, are you sure the ID was correct?");
            return RedirectToAction("Index");

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

            List<ProjectUser> selectedUsers = project.AllUsers.Where(u => u.IsSelected).Select(u => new ProjectUser { UserId = u.UserId, Username = u.Username, ProjectRole = ProjectRole.Developer }).ToList();

            Projects proj = new Projects
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

            //if(!ModelState.IsValid)
            //{            
            //    this.AddAlertDanger($"{project.Name} was not created, model not valid, please try again.");
            //    return View("NewProject", project);
            //}

            if (await _context.SaveChangesAsync() > 0)
            {
                this.AddAlertSuccess($"{project.Name} was created successfully");
                return RedirectToAction("Details", new { proj.Id });
            }

            else
            {
                this.AddAlertDanger($"{project.Name} was not created, please try again later.");
                return View("NewProject", project);
            }
        }

    }
}