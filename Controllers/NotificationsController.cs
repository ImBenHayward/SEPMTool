using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEPMTool.Data;
using SEPMTool.Models;
using SEPMTool.Models.ViewModels;
using SmartBreadcrumbs.Attributes;
using static SEPMTool.Models.ViewModels.ProjectDetailsViewModel;

namespace SEPMTool.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotificationsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            this.userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        [Breadcrumb("Notifications")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<UserNotificationsViewModel> GetNotificationsForUser()
        {
            var user = await GetCurrentUserAsync();
            var notifications = _context.NotificationUser.Include(n => n.Notification).Where(x => x.UserId == user.Id).ToList();           

            List<NotificationUserViewModel> notificationList = _mapper.Map<List<NotificationUser>, List<NotificationUserViewModel>>(notifications);

           var orderedNotificationList = notificationList.OrderByDescending(x => x.Notification.DateTime).ToList();

            foreach (var notification in notificationList)
            {
                var userLink = _context.Users.FirstOrDefault(x => x.Id == notification.UserId);

                notification.Notification.FullNameLink = userLink.FirstName + " " + userLink.LastName;
            }

            UserNotificationsViewModel model = new UserNotificationsViewModel()
            {
                Notifications = orderedNotificationList
            };

            return model;
        }

        public async Task<IActionResult> ToggleIsRead(int notificationId)
        {
            _ = ModelState;

            var user = await GetCurrentUserAsync();

            var notification = _context.NotificationUser.FirstOrDefault(x => x.NotificationId == notificationId && x.UserId == user.Id);

            notification.IsRead = !notification.IsRead;

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> MarkAllAsRead()
        {
            _ = ModelState;

            var user = await GetCurrentUserAsync();

            var notifications = _context.NotificationUser.Where(x => x.UserId == user.Id).ToList();

            foreach(var notification in notifications)
            {
                if(notification.IsRead == false)
                {
                    notification.IsRead = true;
                }
            }            

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}