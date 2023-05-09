using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using GAtreeWebStore;
using Microsoft.AspNetCore.Http.Extensions;
/// <summary>
/// Show the test running. - TODO UNUSED?
/// </summary>

namespace GAtreeWebStore.Controllers
{
    public class TestRunning : Controller
    {
        private readonly ApplicationDBContext _db;
        public TestRunning(ApplicationDBContext db)
        {
            _db = db;
        }
        // GET: TestRunning
        public ActionResult Index()
        {
            return View();
        }

        // GET: TestRunning/Details/5
        /// <summary>
        /// Shows the log for a specificRun
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var a = _db.GATreeTestDataRunLog.Where(x => x.TestDataRunLog_id == id).ToList();
            /// TODO: add a view for test data run log
            return View();
        }

        // GET: TestRunning/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestRunning/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestRunning/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestRunning/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestRunning/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestRunning/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
