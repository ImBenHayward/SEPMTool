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

namespace SEPMTool.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            this.userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public async Task<UserNotificationsViewModel> GetNotificationsForUser()
        {
            var user = await GetCurrentUserAsync();
            var notifications = _context.NotificationUser.Include(n => n.Notification).Where(x => x.UserId == user.Id).ToList();

            List<NotificationUserViewModel> notificationList = _mapper.Map<List<NotificationUser>, List<NotificationUserViewModel>>(notifications);

            UserNotificationsViewModel model = new UserNotificationsViewModel()
            {
                Notifications = notificationList
            };

            return model;
        }
    }
}