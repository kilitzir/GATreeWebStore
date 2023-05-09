using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using GAtreeWebStore.ViewModels;
using GAtreeWebStore.Models;

/// <summary>
/// The main json WEB Api - it responds to all web api calls
/// </summary>

namespace GAtreeWebStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        public CycleController(ApplicationDBContext db)
        {
            _db = db;
            //Utils.jsondfd();
        }
        
        [HttpPost]
        public JsonServerResponse Post( [FromBody] JsonclientResponse value)
        {
            value.action = value.action.ToUpper();
            if (!JobAction.actions.Contains(value.action))
                return new JsonServerResponse()
                {
                    action = JobAction.error,
                    message = $"Not valid request for getCycle: client answer:{JsonConvert.SerializeObject(value)}"
                };
           if (JobAction.nonCycleActions.Contains(value.action))
                return postNonCycleAction(value, _db);

            var a = _db.GATreeTestDataRun.Find(value.datarunID);
            if (a is null || a.TestDataRun_runStage is RunStage.NotAplicable or RunStage.NotStarted or RunStage.Ended)
                return new JsonServerResponse()
                {
                    result = JobAction.error,
                    action = value.action,
                    clientGuid =  value.clientGuid,
                    message = $"api/<CycleController>/Post: Error in datarunID : {value.datarunID}."
                              + (a is null? $"no datarunId: {value.datarunID}": $"Runstage is {a.TestDataRun_runStage}")
                };
            //determine the stage of the datarun
            switch (a.TestDataRun_runStage)
            {
                case RunStage.initializing: if (value.action == JobAction.clientGetJob)
                                                     return Cycle.Step1CreateInitGet(value, _db);
                                            else if (value.action == JobAction.createTrees)
                                                     return Cycle.Step1CreateInitTreesPost(value, _db);
                                                else return new JsonServerResponse()
                                                        {
                                                            action = value.action,
                                                            result = JobAction.error,
                                                            message = $"JsonServerResponse/Post/RunStage.initializing :" +
                                                                      $"JobAction is not right: We are in {RunStage.initializing} Json send: " + JsonConvert.SerializeObject(value)
                                                        };
                case RunStage.Cycle:
                          switch (a.TestDataRun_cycleStage)
                          {
                        case CycleStage.Crossover:
                                                    if (value.action == JobAction.clientGetJob)
                                                        return Cycle.Step3Point2CrossoverGetaJob(value, _db);
                                                    else if (value.action == JobAction.crossOver)
                                                        return Cycle.Step3Point2CrossoverPost(value, _db);
                                                    else return new JsonServerResponse()
                                                    {
                                                        action = value.action,
                                                        result = JobAction.error,
                                                        message = $"JsonServerResponse/Post/CycleStage.Crossover :" +
                                                                           $"JobAction is not right: We are in {RunStage.initializing} Json send: {JsonConvert.SerializeObject(value)}"
                                                    };
                        case CycleStage.Mutation:
                                                    if (value.action == JobAction.clientGetJob)
                                                        return Cycle.Step3Point3MutationGetaJob(value, _db);
                                                    else if (value.action == JobAction.mutate)
                                                        return Cycle.Step3Point3MutationPost(value, _db);
                                                    else return new JsonServerResponse()
                                                    {
                                                        action = value.action,
                                                        result = JobAction.error,
                                                        message = $"JsonServerResponse/Post/CycleStage.Mutation :" +
                                                                           $"JobAction is not right: We are in {RunStage.initializing} Json send: {JsonConvert.SerializeObject(value)}"
                                                    };
                        default:    Utils.AddLogging($"api/<CycleController>/Post: default in switch. Data from client:  {JsonConvert.SerializeObject(value)}", _db);
                                    return new JsonServerResponse()
                                    {
                                        result = JobAction.error,
                                        action = value.action,
                                        treeRecordId = value.TreeRecordId,
                                        message = $"api/<CycleController>/Post: default in switch/cycle : Runstage:{a.TestDataRun_runStage} Cyclestage = {a.TestDataRun_cycleStage}"
                                    };


                    }// case RunStage.Cycle
                case RunStage.Ended:
                                    Utils.AddLogging($"api/<CycleController>/Post: Runstage has ended : Runstage:{a.TestDataRun_runStage}" + 
                                                    $" Cyclestage = {a.TestDataRun_cycleStage}, " +
                                                    $"Data from client:  { JsonConvert.SerializeObject(value)}", _db);

                                    return new JsonServerResponse()
                                    {
                                        result = JobAction.error,
                                        treeRecordId = value.TreeRecordId,
                                        action = value.action,
                                        clientGuid = value.clientGuid,
                                        message = $"api/<CycleController>/Post: Runstage has ended : Runstage:{a.TestDataRun_runStage} Cyclestage = {a.TestDataRun_cycleStage}"
                                    };
                default:
                    Utils.AddLogging($"api/<CycleController>/Post: default in switch/runtage. Runstage:{a.TestDataRun_runStage} Cyclestage = {a.TestDataRun_cycleStage}. Data from client:  {JsonConvert.SerializeObject(value)}", _db);
                    return new JsonServerResponse()
                    {
                        result = JobAction.error,
                        treeRecordId = value.TreeRecordId,
                        action = value.action,
                        clientGuid = value.clientGuid,
                        message = $"api/<CycleController>/Post: default in switch/runstage. Runstage:{a.TestDataRun_runStage} Cyclestage = {a.TestDataRun_cycleStage}. Data from client:  {JsonConvert.SerializeObject(value)}"
                    };
            }//switch
        }
        /// <summary>
        /// The return values for non cycle commands, ie get data/parameters/etc
        /// </summary>
        /// <param name="value"> the JsonclientResponse </param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        private JsonServerResponse postNonCycleAction(JsonclientResponse value, ApplicationDBContext db)
        {
            var run1 = db.GATreeTestDataRun.Find(value.datarunID);
            // First Error checking
            if (run1 is null)
                return new JsonServerResponse()
                {
                    result = JobAction.error,
                    message = $"api/<CycleController>/postNonCycleAction: Error in datarunID : {value.datarunID}."
                              + $"no datarunId: {value.datarunID}",
                };
            var testData = db.GATreeTestsData.Find(run1.TestData_id);
            if (testData is null)
                return new JsonServerResponse()
                {
                    result = JobAction.error,
                    message = $"api/<CycleController>/postNonCycleAction: Error in testdata_id :datarunID {value.datarunID}, " 
                              + $"testdata id: {run1.Test_data_run_id}"
                              + $"no datarunId: {value.datarunID}",
                };
            var data = db.GATreeData.Find(testData.TestData_data_id);
            if (data is null)
                return new JsonServerResponse()
                {
                    result = JobAction.error,
                    message = $"api/<CycleController>/postNonCycleAction: Error in testdata_id :datarunID {value.datarunID}, "
                              + $"testdata id: {run1.Test_data_run_id} data id: {testData.TestData_data_id}"
                              + $"no datarunId: {value.datarunID}",
                };
            // after error checking, create the response
            switch (value.action)
            {
                case JobAction.getData:
                        return new JsonServerResponse()
                        {
                            message = data.Data_json,
                            action = value.action,                            
                            result = JobAction.OK,
                        };
                case JobAction.getAttributes: //TODO Fix it
                        return new JsonServerResponse
                        {
                            action = value.action,
                            message = Utils.GetDataAttributes2Json(data.Data_id, _db),
                            result = JobAction.OK
                        };
                case JobAction.getAttributesText:
                    return new JsonServerResponse()
                    {
                            message = data.attributes_text,
                            action = value.action,
                            result = JobAction.OK,
                        };
                case JobAction.getParameters:

                    var ret = new DataParametersApiModel()
                    {
                        correctionfactor = testData.TestData_correctionfactor,
                        crossoverPropability = testData.TestData_crossoverPropability,
                        generationGap = testData.TestData_generationGap,
                        mutationPropability = testData.TestData_mutationPropability,
                        numberOfMaxGenerations = testData.TestData_numberOfMaxGenerations,
                        numberOfMaxTrees = testData.TestData_numberOfMaxTrees,
                        otherParams = testData.TestData_params
                    };
                    return new JsonServerResponse ()
                    {
                        action = JobAction.getParameters,
                        message = JsonConvert.SerializeObject(ret),
                        result = JobAction.OK
                     };
                case JobAction.registerClient: 
                case JobAction.unRegisterClient: return Utils.RegisterNewClient(value, _db);

            default: // It shouldn't come here. Ever
                    Utils.AddLogging($"Cycle/get:Not a valid get:{JsonConvert.SerializeObject(value)}", _db);
                    return new JsonServerResponse()
                    {
                        action = value.action,
                        clientGuid = value.clientGuid,
                        result = JobAction.error,
                        message = $"api/<CycleController>/postNonCycleAction switch default: client answer:{JsonConvert.SerializeObject(value)}"
                    };
            }
        }
        // GET: api/<CycleController>
        [HttpGet]
        public JsonServerResponse Get()
        {
            return new JsonServerResponse()
            {
                result = JobAction.error,
                action = JobAction.error,
                message = "CycleController/get : get is not supported here "
            };
        }

        // GET api/<CycleController>/5
        //[HttpGet("{id}")]
        public JsonServerResponse Get(int id)
        {
            return new JsonServerResponse()
            {
                result = JobAction.error,
                action = JobAction.error,
                message = "CycleController/get : get is not supported here "
            };
        }
        
        [HttpPut("{id}")]
        public JsonServerResponse Put(int id, [FromBody] string value)
        {
            return new JsonServerResponse
            {
                result = JobAction.error,
                action = JobAction.error,
                message = $"Put is not  implemented"
            };
        }

        // DELETE api/<CycleController>/5
        [HttpDelete("{id}")]
        public JsonServerResponse Delete(int id)
        {
            return new JsonServerResponse
            {
                result = JobAction.error,
                action = JobAction.error,
                message = $"Delete is not  implemented"
            };
        }
    }
}
