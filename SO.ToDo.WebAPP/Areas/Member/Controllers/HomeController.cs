﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SO.ToDo.BusinessLayer.Interfaces;
using SO.ToDo.Entities.Concrete;

namespace SO.ToDo.WebAPP.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    [Area("Member")]
    public class HomeController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMyTaskService _taskService;
        private readonly IRapportService _rapportService;

        public HomeController(INotificationService notificationService, UserManager<AppUser> userManager, IMyTaskService taskService, IRapportService rapportService)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _taskService = taskService;
            _rapportService = rapportService;
        }
        public async Task<IActionResult> Index()
        {
            TempData["Active"] = "Home";
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.ReportCount = _rapportService.GetReportsByUserId(user.Id);
            ViewBag.CompletedTaskCount = _taskService.GetTaskCountCompletedWithUserId(user.Id);
            ViewBag.NotReadNotificationCount = _notificationService.GetNotReadCountByUserId(user.Id);
            ViewBag.MustCompleteTaskCount = _taskService.GetTaskCountMustCompleteByUserId(user.Id);
            return View();
        }
    }
}
