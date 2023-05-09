using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
/// Deletes a row cascading. I Hope there's a better way
/// Maybe we should delete one by one...
/// TODO: Delete the thin clients
/// </summary>

namespace GAtreeWebStore
{
    public static partial class Utils
    {
        /// <summary>
        /// Deletes a row from table data and cascading 
        /// </summary>
        /// <param name="id">the id of the data</param>
        /// <param name="db">the dbcontext</param>
        internal static void DeleteData(int? id, ApplicationDBContext db)
        {
            if (id is null || id == -1)
                return;
            var i = id ?? -1;
            var data = db.GATreeData.Find(id);
            try
            {
                if (data != null)
                {
                    db.Remove(data);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError("DeleteRow.deleteData : dataId:" + i + "error:" + ex.Message, db);
            }
            DeleteDataAttributesUsingDataId(i, db);
            DeleteAllTestDataUsingDataId(i, db);
        }
        /// <summary>
        /// Deletes all attributes associated with a specific data
        /// </summary>
        /// <param name="dataId"> the id of the data</param>
        /// <param name="db">ApplicationDBContext</param>
        internal static void DeleteDataAttributesUsingDataId(int? id, ApplicationDBContext db)
        {
            var dataId = id ?? default;
            try
            {
                var data = db.GATreeDataAttributes.Where(s => s.data_DataAttributes_data_ID == dataId).ToList();
                foreach(var d in data)
                  db.GATreeDataAttributes.Remove(d);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Utils.LogError($"DeleteRow.deleteDataAttributesUsingDataId : dataId: {dataId} error: {ex.Message}",db);
            }
        }


        /// <summary>
        /// Deletes all testdata associated with a specific data id
        /// </summary>
        /// <param name="dataId"> the id of the data</param>
        /// <param name="db">ApplicationDBContext</param>
        internal static void DeleteAllTestDataUsingDataId(int? Id, ApplicationDBContext db)
        {
            var dataId = Id ?? default;
            try
            {
                var data = db.GATreeTestsData.Where(s => s.TestData_data_id == dataId).ToList();
                foreach(var d in data)
                {
                    DeleteTestData(d.TestData_id, db);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError($"DeleteRow.deleteTestDataUsingDataId : dataId:{dataId} error:{ex.Message}",db);
            }
        }
        /// <summary>
        /// Deletes test data based on ID
        /// </summary>
        /// <param name="Id"> the test data's ID</param>
        /// <param name="db">ApplicationDBContext</param>
        internal static void DeleteTestData(int? Id, ApplicationDBContext db)
        {
            var dataId = Id ?? default;
            var data = db.GATreeTestsData.Find(dataId);
            if (data != null)
            {
                DeleteTestDataRunUsingTestDataID(data.TestData_data_id, db);
                DeleteClientsBasedOnTestRunID(data.TestData_data_id, db);
                try
                {
                    db.Remove(data);
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    Utils.LogError($"DeleteTestData: id : {Id}, error: {ex.Message}",db);
                }
            }
        }
        /// <summary>
        /// Deletest a test data run based on its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        internal static void DeleteTestDataRun(int? id, ApplicationDBContext db)
        {
            var rec = db.GATreeTestDataRun.Find(id);
            if (rec != null)
            {
                db.Remove(rec);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// removes test data run based on a test data's ID
        /// </summary>
        /// <param name="id">test data run id</param>
        /// <param name="db"></param>
        internal static void DeleteTestDataRunUsingTestDataID(int? id, ApplicationDBContext db)
        {
            var dataId = id ?? default;
            try
            {
                var data = db.GATreeTestDataRun.Where(s => s.TestData_id == dataId).ToList();
                foreach ( var a in data)
                {
                    DeleteClientsBasedOnTestRunID(a.Test_data_run_id, db);
                    db.Remove(a);
                    // SOS
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError("DeleteRow.deleteTestDataUsingDataId : dataId:" + dataId + "error:" + ex.Message,db);
            }
        }
        /// <summary>
        /// Delete clients based on test data run's ID
        /// </summary>
        /// <param name="TestRunID"> test data run ID</param>
        /// <param name="db">ApplicationDBContext</param>
        internal static void DeleteClientsBasedOnTestRunID(int TestRunID, ApplicationDBContext db)
        {
            try
            {
                var data = db.GATreeThinClient.Where(s => s.Test_data_run_id == TestRunID);
                foreach (var a in data)
                {
                    db.Remove(data);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError($"DeleteRow.DeleteClientsBasedOnTestRunID : dataId:{TestRunID} error: {ex.Message}", db);
            }
        }

    }
}
