using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEPMTool.Models;
using SmartBreadcrumbs.Attributes;

namespace SEPMTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [DefaultBreadcrumb("Home")]
        public IActionResult Index()
        {
            return View();
        }

        [Breadcrumb("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
