using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore;
using GAtreeWebStore.Models;
using Microsoft.AspNetCore.Http.Extensions;
/// <summary>
/// The controller for the attributes
/// </summary>

namespace GAtreeWebStore.Controllers
{
    public class DataAttributesController : Controller
    {
        private readonly ApplicationDBContext _db;
        public DataAttributesController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? id)
        {
            var a = _db.GATreeData.Find(id); 
            if (a == null)
                return View(null);
            var list = _db.GATreeDataAttributes.Where(x => x.data_DataAttributes_data_ID == id).ToList();
            SortDataAttributes gg = new();
            list.Sort(gg);
            return View(list);
        }
    }
}
