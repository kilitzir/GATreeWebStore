using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using Microsoft.AspNetCore.Http.Extensions;
/// <summary>
/// The controller for the TestdataRun 
/// </summary>

namespace GAtreeWebStore.Controllers
{
    public class TestDataRunController : Controller
    {
        private readonly ApplicationDBContext _db;
        public TestDataRunController(ApplicationDBContext db)
        {
            _db = db;
        }
        /// <summary>
        /// Create a new test run based on a specific testdata
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult Create(int? id)
        {
            TestData a = _db.GATreeTestsData.Find(id);
            if (a is null)
            {
                ViewBag.errorMessage = "Error, it wasn't created !! Id for testdata was: " + id.ToString();
                return RedirectToAction("Index");
                ;
            }
            TestDataRun d = new()
            {
                //TestDataRun_dateStart = DateTime.Now,
                TestData_id = a.TestData_id,
                testdata = a
            };
            _db.GATreeTestDataRun.Add(d);
            _db.SaveChanges();
            ViewBag.errorMessage = " Test run was created";
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Shows all the runs
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int? id)

        {
            IEnumerable<TestDataRun> list;
            if (id is null || id == -1)
                list = _db.GATreeTestDataRun;
            else list = _db.GATreeTestDataRun.Where(s => s.testdata.TestData_id == id);
            list = list.OrderByDescending(testDataRun => testDataRun.Test_data_run_id);
            return View(list);
        }
        /// <summary>
        /// Deletes one item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int? id)
        {
            Utils.DeleteTestDataRun(id, _db);
            return RedirectToAction("Index");
        }
        public IActionResult Run(int? id)
        {
            if (id is null)
            {
                Utils.AddLogging("testDataRunController/Run error: Id is null", _db);
            }
            else
            {
                var a = _db.GATreeTestDataRun.Find(id);
                if (a is null)
                {
                    Utils.AddLogging($"testDataRunController/Run error: test not found id: {id} ", _db);
                }
                else
                {
                    var i = id ?? -1;
                    if (a.TestDataRun_runStage == RunStage.NotStarted)
                    {
                        Cycle.Step0Init(a.TestData_id, i, _db);
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Reset(int? id)
        {
            var a = _db.GATreeTestDataRun.Find(id);
            a.TestDataRun_runStage = RunStage.NotStarted;
            a.TestDataRun_cycleStage = CycleStage.NotStarted;
            a.TestDataRun_CurrentIterationNumber = -1;
            _db.SaveChanges();
            var b  =  _db.GATRecordOfTrees.Where(s => s.Test_data_run_id == id);
            foreach (var c in b)
            {
                _db.GATRecordOfTrees.Remove(c);
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
