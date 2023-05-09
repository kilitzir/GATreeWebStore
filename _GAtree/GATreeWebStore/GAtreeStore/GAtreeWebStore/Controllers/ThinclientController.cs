using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using GAtreeWebStore.ViewModels;
/// <summary>
/// The controller for the client's data
/// </summary>

namespace GAtreeWebStore.Controllers
{
    public class ThinclientController : Controller
    {
        private readonly ApplicationDBContext _db;
        public ThinclientController(ApplicationDBContext db)
        {
            _db = db;
        }
        // GET: /thinclient ή /thnclient/index/5
        public ActionResult Index(int? id)
        {
            IEnumerable<ThinClient> list = null;
            if (id == null || id <= 0)
                list = _db.GATreeThinClient;
            else list = _db.GATreeThinClient.Where(x => x.Test_data_run_id == id);
            return View(list);
        }

        // GET: Thinclient/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null || id <= 0)
                ;
            else
            {
                ThinClient a = _db.GATreeThinClient.Find(id);
                if ( a != null)
                {
                    TempData["message"] = $"row with id {id} has been deleted";
                    _db.GATreeThinClient.Remove(a);
                    _db.SaveChanges();
                }  
            }
                return RedirectToAction("Index");
        }        
    }//class
}//namespace
