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
using SmartBreadcrumbs.Attributes;

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

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        [Breadcrumb("Projects List")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            int excludeRecords = (pageSize * pageNumber) - pageSize;

            var model = new ProjectIndexViewModel();
            var user = await GetCurrentUserAsync();

            model.ActiveUserId = user.Id;

            var projects = _context.Projects
                .Include(u => u.Users)
                .Include(r => r.ProjectRequirements)
                .ThenInclude(ProjectRequirement => ProjectRequirement.Tasks).ToList();

            List<ProjectViewModel> projectList = _mapper.Map<List<Project>, List<ProjectViewModel>>(projects);

            model.Projects = projectList;

            return View(model);
        }

        [Breadcrumb("Project")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var currentUser = await GetCurrentUserAsync();
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
                    .ThenInclude(Task => Task.Comments)
                    .Include(r => r.ProjectRequirements)
                    .ThenInclude(ProjectRequirement => ProjectRequirement.Tasks)
                    .ThenInclude(Task => Task.SubTasks)
                    .Include(r => r.ProjectRequirements)
                    .ThenInclude(ProjectRequirement => ProjectRequirement.Comments)
                    .First(p => p.Id == id);

                var project = _mapper.Map<ProjectViewModel>(query);

                foreach(var req in project.ProjectRequirements)
                {
                    foreach(var comment in req.Comments)
                    {
                        var userId = comment.UserId;
                        var commentPoster = _context.Users.FirstOrDefault(x => x.Id == userId);

                        comment.FirstName = commentPoster.FirstName;
                        comment.LastName = commentPoster.LastName;
                        comment.CurrentUser = currentUser.Id;
                    }
                }

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
                    AllUsers = allUsers,
                    CurrentUser = currentUser.Id                    
                };

                return View(model);
            }

            this.AddAlertDanger($"The project with ID: {id} was not found, are you sure the ID was correct?");
            return RedirectToAction("Index");

        }

        [Breadcrumb("New Project")]
        public IActionResult NewProject()
        {
            ProjectCreateViewModel projectCreateViewModel = new ProjectCreateViewModel
            {
                AllUsers = GetAllUsers()
            };

            return View(projectCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateViewModel project)
        {
            var user = await GetCurrentUserAsync();
            List<ProjectUser> selectedUsers = project.AllUsers.Where(u => u.IsSelected).Select(u => new ProjectUser { UserId = u.UserId, Username = u.Username, ProjectRole = ProjectRole.Developer }).ToList();
            List<NotificationUser> notificationUsers = project.AllUsers.Where(u => u.IsSelected).Select(u => new NotificationUser { UserId = u.UserId }).ToList();
            List<ProjectUpdate> projectUpdates = new List<ProjectUpdate>();

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

                Notification notification = new Notification
                {
                    Title = user.FirstName + " added you to a project",
                    Body = user.FirstName + " " + user.LastName + " added you to the " + project.Name + " project as a team member.",
                    Type = UpdateType.ProjectAdd,
                    Users = notificationUsers,
                    UserLink = user.Id,
                    ProjectLink =  proj.Id,
                    DateTime = DateTime.Now
                };

                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { proj.Id });
            }

            else
            {
                this.AddAlertDanger($"{project.Name} was not created, please try again later.");
                return View("NewProject", project);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int projectId, int requirementId, int? parentId, string commentBody, string posterId)
        {
            var user = await GetCurrentUserAsync();
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectId);
            var requirement = _context.Requirements.FirstOrDefault(x => x.Id == requirementId);

            Comment comment = new Comment
            {
                UserId = user.Id,
                DateTime = DateTime.Now,
                CommentBody = commentBody,
                ParentId = parentId,
                Requirement = requirement             
            };

            //NotificationUser notificationUser = new NotificationUser()
            //{
            //    UserId = posterId
            //};

            //if(posterId != null)
            //{
            //    Notification notification = new Notification
            //    {
            //        Title = user.FirstName + " added you to a project",
            //        Body = user.FirstName + " " + user.LastName + " added you to the " + project.Name + " project as a team member.",
            //        Type = UpdateType.ProjectAdd,
            //        Users = notificationUsers,
            //        UserLink = user.Id,
            //        ProjectLink = proj.Id,
            //        DateTime = DateTime.Now
            //    };
            //}

            var commentVm = _mapper.Map<CommentViewModel>(comment);

            commentVm.LastName = user.LastName;
            commentVm.FirstName = user.FirstName;
            commentVm.CurrentUser = user.Id;
                       
            //_context.Notifications.Add(notification);

            _context.Comments.Add(comment);

            //project.Updates.Add(projectUpdate);
            //_context.Requirements.Add(requirement);

            //var requirementVm = _mapper.Map<RequirementViewModel>(requirement);


            //_context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return Ok(new CreateCommentResponse
            {
                Comment = commentVm
            });

        }

        [HttpPost]
        public async Task<IActionResult> CreateRequirement([FromBody] ProjectDetailsViewModel projectRequirement)
        {
            var user = await GetCurrentUserAsync();
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectRequirement.ProjectId);
            List<NotificationUser> notificationUsers = _context.ProjectUser.Where(x => x.ProjectId == projectRequirement.ProjectId).Select(u => new NotificationUser { UserId = u.UserId }).ToList();
                              
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

            var requirementVm = _mapper.Map<RequirementViewModel>(requirement);

            Notification notification = new Notification
            {
                Title = "A new requirement was added to " + project.Name,
                Body = user.FirstName + " " + user.LastName + " added the following requirement to the " + project.Name + " project: " + requirementVm.Name + ".",
                Type = UpdateType.Add,
                Users = notificationUsers,
                UserLink = user.Id,
                ProjectLink = requirementVm.ProjectId,
                DateTime = DateTime.Now
            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return Ok(new UpdateReqResponse
            {
                Requirement = requirementVm
            });

        }

        [HttpPost]
        public async Task<IActionResult> UpdateRequirement([FromBody] ProjectDetailsViewModel projectTask)
        {
            var user = await GetCurrentUserAsync();
            var requirement = _context.Requirements.Include(t => t.Tasks).FirstOrDefault(x => x.Id == projectTask.RequirementId);
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectTask.ProjectId);

            List<NotificationUser> notificationUsers = _context.ProjectUser.Where(x => x.ProjectId == projectTask.ProjectId).Select(u => new NotificationUser { UserId = u.UserId }).ToList();

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

            Notification notification = new Notification
            {
                Title = "A requirement was changed in " + project.Name,
                Body = user.FirstName + " " + user.LastName + " changed the following requirement in the " + project.Name + " project: " + requirementVm.Name + ".",
                Type = UpdateType.Edit,
                Users = notificationUsers,
                UserLink = user.Id,
                ProjectLink = requirementVm.ProjectId,
                DateTime = DateTime.Now
            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return Ok(new UpdateReqResponse
            {
                Requirement = requirementVm
            });

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequirement(int reqId, int projectId)
        {
            var user = await GetCurrentUserAsync();
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectId);
            var requirement = _context.Requirements.Find(reqId);

            List<NotificationUser> notificationUsers = _context.ProjectUser.Where(x => x.ProjectId == projectId).Select(u => new NotificationUser { UserId = u.UserId }).ToList();

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "Requirement Deleted",
                Description = "'" + requirement.Name + "' was Deleted.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Remove
            };

            Notification notification = new Notification
            {
                Title = "A requirement was removed from " + project.Name,
                Body = user.FirstName + " " + user.LastName + " removed the following requirement from the " + project.Name + " project: " + requirement.Name + ".",
                Type = UpdateType.Remove,
                Users = notificationUsers,
                UserLink = user.Id,
                ProjectLink = projectId,
                DateTime = DateTime.Now
            };

            _context.Notifications.Add(notification);

            project.Updates.Add(projectUpdate);
            _context.Requirements.Remove(requirement);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] ProjectDetailsViewModel projectTask)
        {
            var user = await GetCurrentUserAsync();
            List<NotificationUser> notificationUsers = _context.ProjectUser.Where(x => x.ProjectId == projectTask.ProjectId).Select(u => new NotificationUser { UserId = u.UserId }).ToList();    
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

            Notification notification = new Notification
            {
                Title = "A new task was added to " + project.Name,
                Body = user.FirstName + " " + user.LastName + " added the following task to the " + project.Name + " project: " + projectTask.TaskName + ".",
                Type = UpdateType.Add,
                Users = notificationUsers,
                UserLink = user.Id,
                ProjectLink = projectTask.ProjectId,
                DateTime = DateTime.Now
            };

            _context.Notifications.Add(notification);

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

            var user = await GetCurrentUserAsync();
            var task = _context.Tasks.Include(t => t.SubTasks).Include(u => u.Users).FirstOrDefault(ta => ta.Id == projectTask.TaskId);
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectTask.ProjectId);

            List<NotificationUser> notificationUsers = _context.ProjectUser.Where(x => x.ProjectId == projectTask.ProjectId).Select(u => new NotificationUser { UserId = u.UserId }).ToList();
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

            Notification notification = new Notification
            {
                Title = "A task was updated in " + project.Name,
                Body = user.FirstName + " " + user.LastName + " updated the following task in the " + project.Name + " project: " + projectTask.TaskName + ".",
                Type = UpdateType.Edit,
                Users = notificationUsers,
                UserLink = user.Id,
                ProjectLink = projectTask.ProjectId,
                DateTime = DateTime.Now
            };

            _context.Notifications.Add(notification);

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
            var user = await GetCurrentUserAsync();
            var task = _context.Tasks.Find(taskId);
            var requirement = _context.Requirements.Include(x => x.Project).FirstOrDefault(x => x.Id == task.ProjectRequirementId);

            List<NotificationUser> notificationUsers = _context.ProjectUser.Where(x => x.ProjectId == requirement.Project.Id).Select(u => new NotificationUser { UserId = u.UserId }).ToList();

            task.IsCompleted = !task.IsCompleted;

            if(task.IsCompleted == true)
            {
                Notification notification = new Notification
                {
                    Title = "A task was completed in " + requirement.Project.Name,
                    Body = user.FirstName + " " + user.LastName + " marked the following task as complete in the " + requirement.Project.Name + " project: " + task.Name + ".",
                    Type = UpdateType.Remove,
                    Users = notificationUsers,
                    UserLink = user.Id,
                    ProjectLink = requirement.Project.Id,
                    DateTime = DateTime.Now
                };

                _context.Notifications.Add(notification);
            }

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
            var user = await GetCurrentUserAsync();
            var project = _context.Projects.Include(p => p.Updates).FirstOrDefault(x => x.Id == projectId);
            var task = _context.Tasks.Find(taskId);

            List<NotificationUser> notificationUsers = _context.ProjectUser.Where(x => x.ProjectId == project.Id).Select(u => new NotificationUser { UserId = u.UserId }).ToList();

            ProjectUpdate projectUpdate = new ProjectUpdate
            {
                Title = "Task Deleted",
                Description = "'" + task.Name + "' was Deleted.",
                Date = DateTime.UtcNow,
                Type = UpdateType.Remove
            };

            Notification notification = new Notification
            {
                Title = "A task was removed from " + project.Name,
                Body = user.FirstName + " " + user.LastName + " removed the following task from the " + project.Name + " project: " + task.Name + ".",
                Type = UpdateType.Remove,
                Users = notificationUsers,
                UserLink = user.Id,
                ProjectLink = projectId,
                DateTime = DateTime.Now
            };

            _context.Notifications.Add(notification);
            project.Updates.Add(projectUpdate);
            _context.Tasks.Remove(task);            

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddUsers(ProjectDetailsViewModel projectUpdate)
        {
            var currentUser = await GetCurrentUserAsync();
            var project = _context.Projects.FirstOrDefault(x => x.Id == projectUpdate.ProjectId);
            List<ProjectUser> selectedUsers = projectUpdate.AllUsers.Where(u => u.IsSelected).Select(u => new ProjectUser { UserId = u.UserId, Username = u.Username, ProjectRole = ProjectRole.Developer, ProjectId = projectUpdate.ProjectId }).ToList();
            List<NotificationUser> notificationUsers = selectedUsers.Select(u => new NotificationUser { UserId = u.UserId }).ToList();
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

                Notification notification = new Notification
                {
                    Title = currentUser.FirstName + " added you to a project",
                    Body = currentUser.FirstName + " " + currentUser.LastName + " added you to the " + project.Name + " project as a team member.",
                    Type = UpdateType.ProjectAdd,
                    Users = notificationUsers,
                    UserLink = currentUser.Id,
                    ProjectLink = projectUpdate.ProjectId,
                    DateTime = DateTime.Now
                };

                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

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