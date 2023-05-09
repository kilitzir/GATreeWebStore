using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using GAtreeWebStore.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
/// <summary>
/// The controller that shows the tree records for a given data test run
/// </summary>

namespace GAtreeWebStore.Controllers
{
    public class RecordTreeController : Controller
    {
        private readonly ApplicationDBContext _db;
        public RecordTreeController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index(int id)
        {
            IEnumerable<RecordTree> list = _db.GATRecordOfTrees.Where(x => x.Test_data_run_id == id).OrderByDescending(s => s.RecordOfTrees_id);
            ViewBag.tesdatarun = id.ToString();
            return View(list);
        }

    }
}
