﻿using Microsoft.AspNetCore.Mvc;

namespace FiorelloSlider_OnetoMany.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
