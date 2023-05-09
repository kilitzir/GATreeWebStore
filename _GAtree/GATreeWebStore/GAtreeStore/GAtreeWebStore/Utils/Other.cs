using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using Newtonsoft.Json;
using GAtreeWebStore.ViewModels;

namespace GAtreeWebStore
    {
        public static partial class Utils
        {
        /// <summary>
        /// Adds a Error log
        /// TODO: Put the test data run Record  in an error position
        /// </summary>
        /// <param name="s"> string</param>
        /// <param name="db">ApplicationDBContext</param>
        internal static void AddLogging(string s, ApplicationDBContext db)
        {
            db.GATreeLogging.Add(new Logging()
            {
                ErrorMessage = s
            });
            db.SaveChanges();
        }
        /// <summary>
        /// test function to find the json for classes - unused in production
        /// </summary>
        public static void jsondfd()
        {
            var a1 = new JsonTree()
            {
                tree = "tree11",
                evaluation = "11"
            };
            var a2 = new JsonTree()
            {
                tree = "tree22",
                evaluation = "22"
            };
            List<JsonTree> l = new List<JsonTree>();
            l.Add(a1);
            l.Add(a2);
            var jsonr = new JsonRoot()
            {
                trees = l
            };
            var jrootree2 = new JsonTree2()
            {
                tree1 = a1,
                tree2 = a2
            };
            string s = JsonConvert.SerializeObject(a1);
            s = JsonConvert.SerializeObject(a2);
            s = JsonConvert.SerializeObject(jsonr);
            s = JsonConvert.SerializeObject(jrootree2);
        }
        }
}

