using GAtreeWebStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
/// <summary>
/// The home page
/// </summary>

namespace GAtreeWebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _db;

        public HomeController(ILogger<HomeController> logger,ApplicationDBContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.errorMessage = "";
            try
            {
                int i = _db.GATreeData.ToList().Count;
                ViewBag.errorMessage = "DB is connected";
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = "ERROR -  database isn't connected??";
            }
            return View();
        }

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
