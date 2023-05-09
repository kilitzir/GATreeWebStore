using GAtreeWebStore.Models;
using GAtreeWebStore.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Functions that had to do with thin client

namespace GAtreeWebStore
{
    public static partial class Utils
    {
        /// <summary>
        /// registers a new thin client to a specific datatestrun
        /// </summary>
        /// <param name="response"> The client post </param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns>String to be send back through API</returns>
        static internal JsonServerResponse RegisterNewClient(JsonclientResponse response, ApplicationDBContext db)
        {
            if (response.action == JobAction.unRegisterClient)
                return UnRegisterNewClient(response, db);
            db.Add(new ThinClient()
            {
                Test_data_run_id = response.datarunID,
                ThinClient_guid = Guid.NewGuid().ToString(),
                thinClient_isActive = true,
                ThinClient_isRunActive = true, //TODO: check it
            });
            db.SaveChanges();
            string[] s = { "Guid", Guid.NewGuid().ToString() };
            return new JsonServerResponse
            {
                result = JobAction.OK,
                action = JobAction.registerClient,
                message = JsonConvert.SerializeObject(s),
            };
        }
        static internal JsonServerResponse UnRegisterNewClient(JsonclientResponse response, ApplicationDBContext _db)
        {
            var a = _db.GATreeThinClient.Where(x => x.ThinClient_guid == response.clientGuid.ToString() &&
                                                    x.thinClient_isActive == true &&
                                                    x.Test_data_run_id == response.datarunID
                                                    ).ToList();
            if (a.Count == 0)
                return new JsonServerResponse
                {
                    result = JobAction.error,
                    action = JobAction.unRegisterClient,
                    message = $"Couldn't find the client: Client data: {JsonConvert.SerializeObject(response)}"
                };
                // delete the record
                _db.GATreeThinClient.Remove(a[0]);
                _db.SaveChanges();
                return new JsonServerResponse
                {
                    result = JobAction.OK,
                    action = JobAction.unRegisterClient,
                    clientGuid = response.clientGuid,                    
                    message = $" Client data: {JsonConvert.SerializeObject(response)}"
                };
        }
        /// <summary>
        /// Finds if a client exists for this test data ID
        /// </summary>
        /// <param name="testRunId">id of the test run</param>
        /// <param name="guid">Guid of the thin client</param>
        /// <param name="_db">ApplicationDBContext</param>
        /// <returns>true/false </returns>
        static internal bool IsClientValid(int testRunId, Guid guid, ApplicationDBContext _db)
        {
            //TODO 
            var a = _db.GATreeThinClient.Where(
            //x => (x.ThinClient_guid == guid.ToString() &&  x.Test_data_run_id == id)).ToList();
            x => (x.thinClient_isActive == true && x.Test_data_run_id == testRunId)).ToList();

            return (a.Count == 1);
        }
    }//class
}//name

