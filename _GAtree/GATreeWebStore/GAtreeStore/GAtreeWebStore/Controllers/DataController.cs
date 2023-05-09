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
/// The controller for the data
/// </summary>

namespace GAtreeWebStore.Controllers
{
    public class DataController : Controller
    {
        private readonly ApplicationDBContext _db;
        public DataController(ApplicationDBContext db)
        {
            _db = db;
        }
        public ActionResult Index()
        {
            IEnumerable<Data> list = _db.GATreeData.OrderByDescending(data => data.Data_id);
            return View(list);
        }

        [HttpGet]
        public IActionResult viewheader(int? id)
        {
            var data = _db.GATreeData.Find(id);
            ViewBag.dataheader = (data != null) ? data.Data_header.Replace("%", @"</br>%") : "no header or wrong id";
            return View();
        }
        [HttpGet]
        public IActionResult show(int? id)
        {
            if (id is null)
                return RedirectToAction("Index");
            Data data = _db.GATreeData.Find(id);
            if (data is null)
                return RedirectToAction("Index");
            return View(data);
        }

        [HttpGet]
        public IActionResult delete(int? id)
        {
            Utils.DeleteData(id,_db);
            TempData["message"] = $"row with id {id} has been deleted";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult CreateFromFile()
        {
            ViewBag.text = DateTime.Now.ToUniversalTime().ToString();
            return View();
        }
        [HttpPost]
        public IActionResult CreateFromFile(DataAddFromFileViewModel d)
        {
            var bool1 = Utils.AddNewDataRecord(null,d,_db);
            TempData["message"] = bool1 ? $"New data has been added" : " Error found - look at the logs";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create() 
        {
      
            return View();
        }
        [HttpPost]
        public IActionResult Create(Data newData)
        {
            if (!ModelState.IsValid) 
                return View(newData);
            if (false) //TODO: Fix it
            {
                ModelState.AddModelError("CustomError", "SomeError"); // for returnign error messagwes
                return View(newData);
            }
            var bool1 = Utils.AddNewDataRecord(newData,null, _db);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Viewjson(int? id)
        {
            var d = _db.GATreeData.Find(id);
            if (d != null)
            {

                ViewBag.json = d.Data_json.Replace("{\"table1\":", "").Replace("}]}", "} ]");
                return View();
            }
            else

                return RedirectToAction("Index");
        }
    }//class
}//namespace
