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
using AutoMapper;
using static SEPMTool.Models.ViewModels.ProjectDetailsViewModel;
using SEPMTool.enums;

namespace SEPMTool.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            this.userManager = userManager;
            _context = context;
            _mapper = mapper;
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

        public List<TaskUsersViewModel> GetAllUsersInProject(int id)
        {
            var users = _context.ProjectUser.Where(p => p.ProjectId == id);
            List<TaskUsersViewModel> taskUsersViewModel = new List<TaskUsersViewModel>();

            foreach (var user in users)
            {
                var userModel = new TaskUsersViewModel
                {
                    UserId = user.UserId,
                    Username = user.Username
                };

                taskUsersViewModel.Add(userModel);
            }

            return taskUsersViewModel;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;

            var model = new ProjectIndexViewModel();

            var projects = _context.Projects
                .Include(u => u.Users)
                .Include(r => r.ProjectRequirements)
                .ThenInclude(ProjectRequirement => ProjectRequirement.Tasks).ToList();
            //.Skip(excludeRecords)
            //.Take(pageSize);

            List<ProjectViewModel> projectList = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects);

            model.Projects = projectList;

            //PagedResult<Project> result = new PagedResult<Project>
            //{
            //    Data = projects.AsNoTracking().ToList(),
            //    TotalItems = _context.Projects.Count(),
            //    PageNumber = pageNumber,
            //    PageSize = pageSize
            //};

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var allUsers = _context.Users.Where(u => !u.Projects.Any(p => p.ProjectId == id)).Select(x => new ProjectUsersViewModel { UserId = x.Id, Username = x.FirstName + " " + x.LastName }).ToList();

            if (_context.Projects.Any(p => p.Id == id))
            {                      

                var query = _context.Projects
                    .Include(u => u.Users)
                    .Include(p => p.Updates)
                    .Include(r => r.ProjectRequirements)
                    .ThenInclude(ProjectRequirement => ProjectRequirement.Tasks)
                    .ThenInclude(Task => Task.Users)
                    .Include(r => r.ProjectRequirements)
                    .ThenInclude(ProjectRequirement => ProjectRequirement.Tasks)
                    .ThenInclude(Task => Task.SubTasks)
                    .First(p => p.Id == id);

                var project = _mapper.Map<ProjectViewModel>(query);

                var projectViewModel = new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Priority = project.Priority,
                    StartDate = project.StartDate,
                    Deadline = project.Deadline,
                    Status = project.Status,
                    ProjectRequirements = project.ProjectRequirements,
                    Users = project.Users,
                    Updates = project.Updates
                };

                var model = new ProjectDetailsViewModel()
                {  
                    Project = projectViewModel,
                    Users = GetAllUsersInProject(id),
                    AllUsers = allUsers
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
            List<Models.ProjectUpdate> projectUpdates = new List<Models.ProjectUpdate>();

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
                EstimatedCompletionDate = DateTime.Now,
                Updates = projectUpdates
            };

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "Project Created",
                Description = "'" + proj.Name + "' was created",
                Project = proj,
                Date = DateTime.UtcNow,
                Type = UpdateType.Add
            };

            projectUpdates.Add(projectUpdate);

            _context.Projects.Add(proj);

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

        [HttpPost]
        public async Task<IActionResult> CreateRequirement([FromBody] ProjectDetailsViewModel projectRequirement)
        {
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectRequirement.ProjectId);

            ProjectRequirement requirement = new ProjectRequirement
            {
                Project = project,
                Name = projectRequirement.RequirementName,
                Description = projectRequirement.RequirementDescription,
                Priority = projectRequirement.RequirementPriority,
                Category = projectRequirement.RequirementCategory
            };

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "New Requirement Added",
                Description = "'" + projectRequirement.RequirementName + "' was added.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Add
            };
            
            project.Updates.Add(projectUpdate);
            _context.Requirements.Add(requirement);

            //_context.Requirements.Add(requirement);

            await _context.SaveChangesAsync();

            var requirementVm = _mapper.Map<RequirementViewModel>(requirement);

            return Ok(new UpdateReqResponse
            {
                Requirement = requirementVm
            });

        }

        [HttpPost]
        public async Task<IActionResult> UpdateRequirement([FromBody] ProjectDetailsViewModel projectTask)
        {

            var requirement = _context.Requirements.Include(t => t.Tasks).FirstOrDefault(x => x.Id == projectTask.RequirementId);
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectTask.ProjectId);

            requirement.Name = projectTask.RequirementName;
            requirement.Description = projectTask.RequirementDescription;
            requirement.Priority = projectTask.RequirementPriority;
            requirement.Category = projectTask.RequirementCategory;

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "Requirement Edited",
                Description = "'" + projectTask.RequirementName + "' was edited.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Edit
            };

            var requirementVm = _mapper.Map<RequirementViewModel>(requirement); 

            project.Updates.Add(projectUpdate);

            await _context.SaveChangesAsync();

            return Ok(new UpdateReqResponse
            {
                Requirement = requirementVm
            });

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequirement(int reqId, int projectId)
        {
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectId);
            var requirement = _context.Requirements.Find(reqId);

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "Requirement Deleted",
                Description = "'" + requirement.Name + "' was Deleted.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Remove
            };

            project.Updates.Add(projectUpdate);
            _context.Requirements.Remove(requirement);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] ProjectDetailsViewModel projectTask)
        {
            _ = ModelState;

            List<TaskUser> selectedUsers = new List<TaskUser>();

            if (projectTask.Users != null)
            {
                selectedUsers = projectTask.Users.Where(u => u.IsSelected).Select(u => new TaskUser { UserId = u.UserId, Username = u.Username }).ToList();
            }

            ICollection<SubTask> requirementTasks = _mapper.Map<List<SubTaskViewModel>, ICollection<SubTask>>(projectTask.SubTasks);            

            RequirementTask projTask = new RequirementTask
            {
                Name = projectTask.TaskName,
                Description = projectTask.TaskDescription,
                Users = selectedUsers,
                ProjectRequirementId = projectTask.RequirementId,
                SubTasks = requirementTasks
            };

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "New Task Added",
                Description = "'" + projTask.Name + "' was added.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Add
            };

            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectTask.ProjectId);

            project.Updates.Add(projectUpdate);

            _context.Tasks.Add(projTask);

            await _context.SaveChangesAsync();

            List<SubTaskViewModel> subTasks = _mapper.Map<ICollection<SubTask>, List<SubTaskViewModel>>(projTask.SubTasks);
            List<TaskUserViewModel> users = _mapper.Map<ICollection<TaskUser>, List<TaskUserViewModel>>(projTask.Users);

            return Ok(new CreateTaskResponse
            {
                Id = projTask.Id,
                Name = projTask.Name,
                Description = projTask.Description,
                ProjectRequirementId = projTask.ProjectRequirementId,
                IsCompleted = false,
                Status = projTask.Status,
                SubTasks = subTasks,
                Users = users,
                //Update = projectUpdate
            });

        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask([FromBody] ProjectDetailsViewModel projectTask)
        {
            _ = ModelState;

            var task = _context.Tasks.Include(t => t.SubTasks).Include(u => u.Users).FirstOrDefault(ta => ta.Id == projectTask.TaskId);
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectTask.ProjectId);

            List<TaskUser> selectedUsers = new List<TaskUser>();

            if (projectTask.Users != null)
            {
                selectedUsers = projectTask.Users.Where(u => u.IsSelected).Select(u => new TaskUser { UserId = u.UserId, Username = u.Username }).ToList();
            }

            //List<SubTask> subTasksDb = _mapper.Map<ICollection<SubTaskViewModel>, List<SubTask>>(projectTask.SubTasks);

            foreach(var st in projectTask.SubTasks)
            {
                if(st.Id == 0)
                {
                    var subTask = _mapper.Map<SubTask>(st);

                    task.SubTasks.Add(subTask);
                }
            }

            task.Name = projectTask.TaskName;
            task.Description = projectTask.TaskDescription;
            //task.SubTasks = subTasksDb;
            task.Users = selectedUsers;

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "Task Edited",
                Description = "'" + projectTask.TaskName + "' was edited.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Edit
            };

            var requirementTask = _mapper.Map<RequirementTaskViewModel>(task);
            List<SubTaskViewModel> subTasks = _mapper.Map<ICollection<SubTask>, List<SubTaskViewModel>>(task.SubTasks);
            List<TaskUserViewModel> users = _mapper.Map<ICollection<TaskUser>, List<TaskUserViewModel>>(task.Users);

            project.Updates.Add(projectUpdate);

            await _context.SaveChangesAsync();

            return Ok(new CreateTaskResponse
            {
                Id = requirementTask.Id,
                Name = requirementTask.Name,
                Description = requirementTask.Description,
                ProjectRequirementId = requirementTask.ProjectRequirementId,
                Status = requirementTask.Status,
                SubTasks = subTasks,
                Users = users
            });

        }
               
        public async Task<IActionResult> ToggleTaskComplete(int taskId)
        {
            var task = _context.Tasks.Find(taskId);

            task.IsCompleted = !task.IsCompleted;

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> UpdateSubTask(int taskId)
        {
            _ = ModelState;

            var subTask = _context.SubTasks.Find(taskId);

            subTask.IsCompleted = !subTask.IsCompleted;

            await _context.SaveChangesAsync();

            return Ok(new UpdateSubTaskResponse
            {
                Id = taskId
            });
        }

        public async Task<IActionResult> UpdateProjectProgress(int projectId, decimal progress)
        {
            _ = ModelState;

            var project = _context.Projects.Find(projectId);

            project.Progress = progress;

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> KanbanMoveItem([FromBody] KanbanMoveItemViewModel viewModel)        
        {
            _ = ModelState;

            var task = _context.Tasks.Find(viewModel.TaskId);

            task.Status = (enums.TaskStatus)viewModel.Status;

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int taskId, int projectId)
        {
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectId);
            var task = _context.Tasks.Find(taskId);            

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "Task Deleted",
                Description = "'" + task.Name + "' was Deleted.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Remove
            };

            project.Updates.Add(projectUpdate);
            _context.Tasks.Remove(task);            

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddUsers(ProjectDetailsViewModel projectUpdate)
        {
            List<ProjectUser> selectedUsers = projectUpdate.AllUsers.Where(u => u.IsSelected).Select(u => new ProjectUser { UserId = u.UserId, Username = u.Username, ProjectRole = ProjectRole.Developer, ProjectId = projectUpdate.ProjectId }).ToList();
            List<string> Users = new List<String>();

            foreach (var user in selectedUsers)
            {
                _context.ProjectUser.Add(user);
                Users.Add(user.Username);
            }


            if (await _context.SaveChangesAsync() > 0)
            {
                string users = string.Join(", ", Users);
                this.AddAlertSuccess($"{users} added successfully");
                return RedirectToAction("Details", new { id = projectUpdate.ProjectId });
            }

            else
            {
                string users = string.Join(",", Users);
                this.AddAlertDanger($"Users were not added successfully.");
                return RedirectToAction("Details", new { id = projectUpdate.ProjectId });
            }
        }

    }
}