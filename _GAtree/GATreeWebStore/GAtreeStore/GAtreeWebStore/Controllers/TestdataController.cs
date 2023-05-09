using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Extensions;
/// <summary>
/// The controllers for the tests's data
/// </summary>
namespace GAtreeWebStore.Controllers
{
    public class TestdataController : Controller
    {
        private readonly ApplicationDBContext _db;
        public TestdataController (ApplicationDBContext db)
        {
            _db = db;
        }
        // GET: TestdataController
        public ActionResult Index(int? id)
        {
            IEnumerable<TestData> list;
            if (id is null || id == -1)
                list = _db.GATreeTestsData;
            else
            {
                list = _db.GATreeTestsData.Where(s => s.TestData_data_id == id);
            }
            list = list.OrderByDescending(testData => testData.TestData_id);
            return View(list);
        }

        // GET: TestdataController/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return Redirect("Index");
            TestData t = _db.GATreeTestsData.Find(id);
            if (t == null)
                return Redirect("Index");
            else return View(t);
        }
        [HttpGet]
        // GET: TestdataController/Create
        /// Doesn't work for create from nothing!!!!
        public ActionResult Create( int? id)
        {
            //if (!data_id.HasValue)
              //  return Redirect("/data/Index");
            Data d = _db.GATreeData.Find(id);
            if (d is null)
            {
                Utils.LogError("testdatacontroler.Create error: " + Request.GetDisplayUrl(),_db);
            }
            //TODO: either from ID or from 

            IEnumerable<SelectListItem> dtDropdown = _db.GATreeDataAttributes.Select(i => new SelectListItem
            {
                Text = i.DataAttributes_name,
                Value = i.DataAttributes_name
            });
            //not needed
            IEnumerable<SelectListItem> TypeDropDown = _db.GATreeData.Select(i => new SelectListItem
            {
                Text = i.Data_friendlyname,
                Value = i.Data_id.ToString()
            });
            ViewBag.dtDropdown = dtDropdown;
            ViewBag.TypeDropDown = TypeDropDown;//uneeded
            ViewBag.data_id = d.Data_id;
            ViewBag.data_name = d.Data_friendlyname;
            return View();
        }

        // POST: TestdataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, TestData newtestData)
        {
            newtestData.TestData_guid = Guid.NewGuid().ToString();
            //TODO Put the id of the data in a hidden cell in html
            var s = Request.Path.Value.Split("/");
            newtestData.TestData_data_id = Int32.Parse(s[s.Length - 1]);

            _db.Add(newtestData);
            _db.SaveChanges();
            TempData["message"] = " testdata was created";
            return RedirectToAction(nameof(Index));
        }

        // GET: TestdataController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestdataController/Edit/5
        [HttpPost]
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

        // GET: TestdataController/Delete/5
        public ActionResult Delete(int id)
        {
            Utils.DeleteTestData(id, _db);
            IEnumerable<TestData> list = _db.GATreeTestsData;
            //return View(list);
            return RedirectToAction("Index",list);
        }

        // POST: TestdataController/Delete/5
        [HttpPost]
        // Needed????!!!??? callled???
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
        public ActionResult Viewtests(int? id)
        {
            IEnumerable<TestData> testdata = _db.GATreeTestsData.Where(s => s.TestData_data_id== id);
            return View(testdata);
        }
        ///Copies data from on test to another.
        public ActionResult Copydata( int id)
        {
            TestData oldTestdata = _db.GATreeTestsData.Find(id);
            IEnumerable<TestData> list = _db.GATreeTestsData;
                        
            if (oldTestdata == null)
                return View("Index", list);

            TestData newTestdata = new()
            {
                TestData_friendlyName = oldTestdata.TestData_friendlyName,
                TestData_notes = oldTestdata.TestData_notes,
                TestData_numberOfMaxTrees = oldTestdata.TestData_numberOfMaxTrees,
                TestData_numberOfMaxGenerations = oldTestdata.TestData_numberOfMaxGenerations,
                TestData_mutationPropability = oldTestdata.TestData_mutationPropability,
                TestData_crossoverPropability = oldTestdata.TestData_crossoverPropability,
                TestData_generationGap  = oldTestdata.TestData_generationGap,
                TestData_correctionfactor = oldTestdata.TestData_correctionfactor,
                TestData_params = oldTestdata.TestData_params,
            };
            newTestdata.TestData_data_id = oldTestdata.TestData_data_id;
            //newTestdata.Data = new Data();
            newTestdata.Data = oldTestdata.Data;
                
            _db.Add(newTestdata);
            _db.SaveChanges();
            
            //return View(list);
            return View("Index",list);
        }
    }
}
