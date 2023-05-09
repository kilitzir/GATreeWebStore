using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAtreeWebStore.Models;
using Newtonsoft.Json;
using GAtreeWebStore.ViewModels;
/// <summary>
/// Utils for getting datas from SQL server
/// </summary>

namespace GAtreeWebStore
{
    public static partial class Utils
    {
        static public readonly Guid whoCares = Guid.Empty;
        /// <summary>
        /// Sends the data to the web api
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        internal static string GetDataFromdataId(int id, ApplicationDBContext _db)
        {
            var a = _db.GATreeData.Find(id);
            if (a is null)
                return JsonConvert.SerializeObject(new JsonServerResponse()
                {
                    result = "FALSE",
                    message = "id for data is " + id + " : use valid id"
                });
            return a.Data_json;
        }

        /// <summary>
        ///  Sends the data attributes to the web api.
        ///  Data need to be sorted and minimize to absolute needed data - dataAttributes has many unneeded variables that 
        ///  need to be deleted before sending it to web api.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        internal static string GetDataAttributes2Json(int dataId, ApplicationDBContext _db)
        {
            var a = _db.GATreeData.Find(dataId);
            if (a is null)
                return JsonConvert.SerializeObject(new JsonServerResponse()
                {
                    result = "FALSE",
                    message = "id for data-datattributes " + dataId + " : use valid id"
                });
            var b = _db.GATreeDataAttributes.Where(x => x.data_DataAttributes_data_ID == dataId).ToList();
            if (b.Count == 0)
                return JsonConvert.SerializeObject(new JsonServerResponse()
                {
                    result = "FALSE",
                    message = "id for data-datattributes " + dataId + " : no records Found??!!"
                });
            List<DataAttributesWebApiModel> ret = new();
            foreach (var bb in b)
            {
                string smax = bb.DataAttributes_attributesData != null ? null : // may "NA" is better?
                                                bb.data_DataAttributes_floatMax != float.MaxValue ?
                                                          bb.data_DataAttributes_floatMax.ToString() :
                                                          bb.data_DataAttributes_intMax.ToString();
                string smin = bb.DataAttributes_attributesData != null ? null :
                                                bb.data_DataAttributes_floatMin != float.MaxValue ?
                                                bb.data_DataAttributes_floatMin.ToString() :
                                                bb.data_DataAttributes_intMin.ToString();

                ret.Add(new DataAttributesWebApiModel
                {
                    sn = bb.DataAttributes_sn,
                    name = bb.DataAttributes_name,
                    type = bb.DataAttributes_type,
                    attributesData = bb.DataAttributes_attributesData,
                    isClass = bb.DataAttributes_isClass,
                    hasNullValues = bb.DataAttributes_hasNullValues, ///TODO: return nothing if it has attributes!
                    Max = smax,
                    Min = smin,
                }
                    );
            }

            // Sorth the data accoarding to the DataAttributes_sn
            SortDataAttributesWebApiModel gg = new();
            ret.Sort(gg);

            return JsonConvert.SerializeObject(ret);
        }
        /// <summary>
        /// Gets the Data Parametsrs based on Run Id
        /// </summary>
        /// <param name="testDataRunId">the test data Id</param>
        /// <param name="GuidThinClient">The guid of the client</param>
        /// <param name="_db">ApplicationDBContext</param>
        /// <returns>string with the parameters</returns>
        internal static string GetTestDataParameters(int testDataRunId, Guid GuidThinClient, ApplicationDBContext _db)
        {

            //client exists or who cares?
            if (!Utils.IsClientValid(testDataRunId,GuidThinClient,_db) && !(whoCares == GuidThinClient))
            return JsonConvert.SerializeObject(new
            {
                result = "Error",
                Message = "client doesn't exists"
            });
            //testrunexists?
            var d = GetTestDataObjectFromTestDataRunId(testDataRunId,_db);
            if (d is null)
                return JsonConvert.SerializeObject(new JsonServerResponse()
                {
                    result = "Error",
                    message = "TestData doesn't exists"
                });

            return JsonConvert.SerializeObject(d);
        }
        /// <summary>
        /// Gets the id from the TestData using  TestDataRun Id
        /// </summary>
        /// <param name="testDataRunId"> the testdataRunID</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        internal static TestData GetTestDataObjectFromTestDataRunId(int testDataRunId,ApplicationDBContext db)
        {

            var a = GetTestDataIdFromTestDataRunId(testDataRunId, db);
            if (a == -1) 
            {
                Utils.AddLogging($"GetTestDataObjectFromTestDataRunId: a == -1, testDataRunId:{testDataRunId}",db);
                return null;
            }
            return db.GATreeTestsData.Find(a);
        }
        internal static int GetTestDataIdFromTestDataRunId(int testDataRunId, ApplicationDBContext db)
        {
            var a = db.GATreeTestDataRun.Find(testDataRunId);
            if (a is null)
            {
                Utils.AddLogging($"GetTestDataIdFromTestDataRunId: a = null, testDataRunId:{testDataRunId}",db);
                return -1;
            }
            return a.TestData_id;
        }
        internal static int GetMaximumMaxIterationNumberUsingDatatestRunID(int dataRunId, ApplicationDBContext db)
        {
            var a = db.GATreeTestDataRun.Find(dataRunId);
            if (a is null) return -1;

            var b = db.GATreeTestsData.Find(a.TestData_id);
            return (b is null ? -1 : b.TestData_numberOfMaxGenerations);
        }
    }//class
    
}//namespace
