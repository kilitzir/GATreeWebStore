using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using GAtreeWebStore.Models;
using Newtonsoft.Json;
using GAtreeWebStore.ViewModels;
using GAtreeWebStore;
/// <summary>
/// The functions for any cycle
/// Step0: The function when the user presses Run
/// Step1: Create records for the client to create trees
/// 
/// </summary>
/// https://stackoverflow.com/questions/75017397/r-language-post-with-json-error-415-unsupported-media-type

namespace GAtreeWebStore
{
    public static class Cycle
    {
        //-------------------------- STEP 0 -------
        /// <summary>
        /// Step zero - initialize the records and call Step1 - Called when a user pressed Run
        /// </summary>
        /// <param name="testdataID"> the test data Id</param>
        /// <param name="testDataRunId">The testa data ID</param>
        /// <param name="db">ApplicationDBContext</param>
        static internal void Step0Init(int testdataID, int testDataRunId, ApplicationDBContext db)
        {

            int iterations = Utils.GetMaximumMaxIterationNumberUsingDatatestRunID(testDataRunId, db);
            if (iterations == -1)
            {
                GAtreeWebStore.Utils.LogError("Something is wrong?! getMaximumMaxIterationNumberUsingDatatestID = -1?", db);
                return;
            }
            //Update the record for the test data run
            var a = db.GATreeTestDataRun.Find(testDataRunId);

            a.TestDataRun_dateStart = DateTime.Now;
            a.TestDataRun_comments = "step0";
            a.TestData_id = testdataID;
            a.TestDataRun_runStage = RunStage.Started;
            a.TestDataRun_cycleStage = CycleStage.NotStarted;
            a.TestDataRun_MaxIterationNumber = iterations;
            db.Update(a);
            db.SaveChanges();
            //create a new log record            
            db.Add(new TestDataRunLog()
            {
                TestDataRunLog_Stage = a.TestDataRun_runStage,
                TestDataRunLog_CycleStage = a.TestDataRun_cycleStage,
                TestDataRunLog_IterationNo = a.TestDataRun_MaxIterationNumber,
                TestDataRunLog_BestResult = null,
                TestDataRunLog_comments = "Step0:initializing",
                Test_data_run_id = a.Test_data_run_id
            });
            db.SaveChanges();
            Step1CreateInitTreesInit(testDataRunId, testdataID, db);
        }
        /// <summary>
        /// Initiate step 1 - Create the n trees 
        /// </summary>
        /// <param name="testDataRunId"> data run Id</param>
        /// <param name="testDataId"> test data ID</param>
        /// <param name="db">ApplicationDBContext</param>
        //-------------------------- STEP 1 -------        
        private static void Step1CreateInitTreesInit(int testDataRunId, int testDataId, ApplicationDBContext db)
        {
            //update the testdatarun
            var run1 = db.GATreeTestDataRun.Find(testDataRunId);
            run1.TestDataRun_comments = "step1 CreateTreeRecordsInit";
            run1.TestDataRun_runStage = RunStage.initializing;
            db.Update(run1);
            db.SaveChanges();
            db.Add(new TestDataRunLog() // Add a log
            {
                TestDataRunLog_Stage = run1.TestDataRun_runStage,
                TestDataRunLog_CycleStage = run1.TestDataRun_cycleStage,
                TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                TestDataRunLog_BestResult = null,
                TestDataRunLog_comments = "Step1:initializing CreateTreeRecordsInit",
                Test_data_run_id = testDataRunId                
            });
            db.SaveChanges();
            Step1CreateInitTrees(testDataRunId, testDataId,
                                    Step0GetMaximumTreeRecords(testDataRunId, testDataId, db),
                                    db);
        }
        /// <summary>
        /// Updates a record with the data from the client
        /// </summary>
        /// <param name="content"></param>
        /// <param name="datarunId"></param>
        /// <param name="clientGuid"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static JsonServerResponse Step1CreateInitTreesPost(JsonclientResponse response, ApplicationDBContext db)
        {
            // TODO: Check if the client ID matches
            var treeRec = db.GATRecordOfTrees.Find(response.TreeRecordId);
            if (treeRec is null)
            {
                Utils.AddLogging($"Step1Post: couldn't find treerecord:{response.TreeRecordId}. client send: {JsonConvert.SerializeObject(response)}",db);
                return new JsonServerResponse
                {
                    result = "ERROR",
                    action = response.action,
                    treeRecordId = response.TreeRecordId,
                    message = $" Error Step1Post: couldn't find treerecord:{response.TreeRecordId}"
                };
            }
            if (treeRec.RecordOfTrees_treeStatus  != TreesStatus.WaitForResults)
            {
                return new JsonServerResponse
                {
                    result = JobAction.error,
                    action = response.action,
                    treeRecordId = response.TreeRecordId,
                    message = $" Error Step1CreateTreeRecordsPost: Record has already data:for id {response.TreeRecordId} .Client send: {JsonConvert.SerializeObject(response)}"
                };
            }
            treeRec.RecordOfTrees_data = response.result;
            treeRec.RecordOfTrees_treeStatus = TreesStatus.Finished;
            db.SaveChanges();
            // We have to check if all the records have been filled with data to proceed to the next step
            var b = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == treeRec.Test_data_run_id && 
                                                   ( x.RecordOfTrees_treeStatus == TreesStatus.WaitForResults  ||
                                                     x.RecordOfTrees_treeStatus == TreesStatus.Init
                                                   )                                              &&
                                                   x.Test_data_run_id == treeRec.Test_data_run_id) 
                                                   .ToList().Count;
            if ( b == 0)
            {
                var bestTree = Step1CreateInitTreesConsolidateTrees(response.datarunID, db); // consolidate all the trees
                Step1CreateInitTreeLastAction(response.datarunID,bestTree, db);
            }
            return new JsonServerResponse()
            {
                action = response.action,
                result = "OK",
                message = " Trees has been recorded",
                treeRecordId = response.TreeRecordId
            };
        }
        /// <summary>
        /// Gets a record from the pool of free recordtrees and sends it to client
        /// </summary>
        /// <param name="content">The response of the server </param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns>json </returns>
        public static JsonServerResponse Step1CreateInitGet(JsonclientResponse response, ApplicationDBContext db)
        {
            int datarunId = response.datarunID;
            string clientGuid = response.clientGuid.ToString();
            // Find if the thin client is eligible
            var a = db.GATreeThinClient.Where(x => x.ThinClient_guid == clientGuid && 
                                              x.thinClient_isActive == true && 
                                              x.Test_data_run_id == datarunId).ToList();
            //if (a.Count == 0)
            if (false)
            {
                Utils.AddLogging($"Step1Get: no client found : datarunId = {datarunId} ,clientGuid = {clientGuid} ", db);
                return new JsonServerResponse()
                {
                    result = "ERROR",
                    message = "Wrong Client Guid or dataRunId ",
                    action = JobAction.none
                };
            }
            // Find which recordtree is available and update it, otherwise return error.
            // TODO Iterationumber should be taken by the testdataRun, because it may change!!!
            var rec = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_treeStatus == TreesStatus.Init && 
                                                     x.Test_data_run_id == datarunId &&
                                                     x.RecordOfTrees_iterationNo == -1  &&
                                                     x.RecordOfTrees_runStage == RunStage.initializing).ToList();
            if (rec.Count == 0) // last record - Error!
            {
                Utils.AddLogging($"Step1Post: trees are full : datarunId = {datarunId} ,clientGuid = {clientGuid} ", db);

                return new JsonServerResponse()
                {
                    action = JobAction.Busy,
                    message = "Not any available tree records ",
                };
            }
            // update the tree record
            var recordTree = rec[0];
            recordTree.RecordOfTrees_treeStatus = TreesStatus.WaitForResults;
            recordTree.RecordTrees_thinClientGuid = response.clientGuid.ToString();
            db.Update(recordTree);
            db.SaveChanges();
            return new JsonServerResponse()
            {
                result = JobAction.OK,
                action = JobAction.createTrees,
                treeRecordId = recordTree.RecordOfTrees_id,
                message = "10", //SOS Find a function
            };
            
        }
        /// <summary>
        /// When all tree records have been filled, we need to consolidate all the records to one for the next step
        /// </summary>
        /// <param name="testDatarunId">test Datarun Id</param>
        /// <param name="db">ApplicationDBContext</param>
        private static JsonTree Step1CreateInitTreesConsolidateTrees(int testDatarunId, ApplicationDBContext db)
        {
            var run1 = db.GATreeTestDataRun.Find(testDatarunId);
            if (run1 is null)
            {
                Utils.AddLogging($"ConsolidateTrees: datarun not found : testDatarunId = {testDatarunId} ", db);
                return null;
            }
            // Take all treerecords' data from the previous step and merge them to one and save it to a treerecord
            var recs = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == testDatarunId && 
                                              x.RecordOfTrees_iterationNo == -1 &&  
                                              x.Test_data_run_id  == run1.Test_data_run_id
                                              ).ToList();
            if (recs is null || recs.Count == 0) 
            {
                Utils.AddLogging($"Step1CreateTreeRecordsConsolidateTrees: no trees found:testDatarunId : {testDatarunId}",db);
                return null;
            }
            //For every record, convert it to jsontree and add all the trees -  one treerecord has multiple trees!
            List<JsonTree> jsontree = new(); 

            foreach (var b in recs)
            {
                var c = JsonConvert.DeserializeObject<List<JsonTree>>(b.RecordOfTrees_data); // Changed 15/2
                jsontree.AddRange(c);
            }
            if (jsontree.Count == 0 )
            {
                Utils.AddLogging($"Step1CreateTreeRecordsConsolidateTrees :jsontree.Count is zero. testDatarunId :  {testDatarunId}",db);
                return null;
            }
            //sort and select max trees
            SortJsonTree gg = new();
            jsontree.Sort(gg);
            int maxTrees = Utils.GetTestDataObjectFromTestDataRunId(testDatarunId, db).TestData_numberOfMaxTrees;
            jsontree = jsontree.Take(maxTrees).ToList();
            // Create a treerecord with the selected trees
            // most of these variables are redudant - TODO: delete them after debugging
            db.GATRecordOfTrees.Add( new RecordTree ()
                {
                RecordOfTrees_data = JsonConvert.SerializeObject(new JsonRoot()
                {
                    trees = jsontree
                }),
                RecordOfTrees_runStage = run1.TestDataRun_runStage,
                RecordOfTrees_cycleStage = run1.TestDataRun_cycleStage, // What is right??
                RecordOfTrees_isConsolidation = true,
                RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                Test_data_run_id = testDatarunId,
                RecordOfTrees_treeStatus = TreesStatus.AllResults,
                });
            db.SaveChanges();
            db.Add(new TestDataRunLog()
            {
                Test_data_run_id = testDatarunId,
                TestDataRunLog_Stage = RunStage.Cycle,
                TestDataRunLog_CycleStage = CycleStage.Choosing,
                TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                TestDataRunLog_BestResult = JsonConvert.SerializeObject(jsontree.Take(1).ToList()[0]),
                TestDataRunLog_comments = $"ConsolidateTrees: Merging and sorting the records in recordstree",
            });
            db.SaveChanges();
            return jsontree.Take(1).ToList()[0];
        }

        /// <summary>
        /// Create record trees for the 1st step.
        /// </summary>
        /// <param name="testDataRunId"> test data run ID</param>
        /// <param name="TestDataId"> test data ID</param>
        /// <param name="NoOfTreeRecords">How many tree records should we make</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns> If there's an error</returns>
        static bool Step1CreateInitTrees(int testDataRunId, int TestDataId, int NoOfTreeRecords, ApplicationDBContext db)
        {
            var aa = db.GATreeTestDataRun.Find(testDataRunId);
            if (aa is null)
            {
                Utils.LogError($"step1:CreateTrees: no {testDataRunId} found in db.GATreeTestsData", db);
                return false;
            }
            // TODO:check if the records already exists
            // Adding the records
            var bb = db.GATreeTestsData.Find(TestDataId);
            int TotalTrees = bb.TestData_numberOfMaxTrees * Step1CreateInitGetTreeMultiplier(testDataRunId,TestDataId,db);
            for (int i = 0; i < NoOfTreeRecords; i++)
            {
                db.Add(new RecordTree()
                {
                    RecordOfTrees_treeStatus = TreesStatus.Init,
                    Test_data_run_id = testDataRunId,
                    RecordOfTrees_iterationNo = aa.TestDataRun_CurrentIterationNumber,
                    RecordOfTrees_runStage = RunStage.initializing,
                    RecordOfTrees_data = ((TotalTrees / NoOfTreeRecords) + 1).ToString(),
                    RecordOfTrees_cycleStage = CycleStage.InitValue,
                    RecordOfTrees_isConsolidation = false
                }); ;
            }
            db.SaveChanges();
            db.Add(new TestDataRunLog()
            {
                Test_data_run_id = testDataRunId,
                TestDataRunLog_Stage = aa.TestDataRun_runStage,
                TestDataRunLog_CycleStage = CycleStage.Evaluate,
                TestDataRunLog_IterationNo = aa.TestDataRun_CurrentIterationNumber,
                TestDataRunLog_BestResult = null,
                TestDataRunLog_comments = $"Step1: Creating {NoOfTreeRecords} records  in the RecordOfTrees asking {TotalTrees} trees"
            });
            db.SaveChanges();
            return true;
        }
        /// <summary>
        /// Step1 Last Actions: Put the data test run for step 2
        /// </summary>
        /// <param name="CurrentIterationNumber"> the current iteration number</param>
        /// <param name="testDataRunId"> test run ID</param>
        /// <param name="bestTree"> the best tree</param>
        /// <param name="db">ApplicationDBContext</param>
        private static void Step1CreateInitTreeLastAction(int testDataRunId, JsonTree bestTree, ApplicationDBContext db)
        {
            var run1 = db.GATreeTestDataRun.Find(testDataRunId);
            run1.TestDataRun_results = JsonConvert.SerializeObject(bestTree);
            db.SaveChanges();
            //add a log record TODO: Add all the records?
            db.GATreeTestDataRunLog.Add(new TestDataRunLog()
            {
                TestDataRunLog_CycleStage = run1.TestDataRun_cycleStage,
                TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                Test_data_run_id = run1.Test_data_run_id,
            });
            db.SaveChanges();
            Step2BeginCycleInit(testDataRunId, db);
            return;
        }
        //------------------------------------------ Step 2 Begin of the loop
        /// <summary>
        /// The beginning of the loop. It iniializes the loop and checks if we should exit the loop
        /// The loop is while and NOT do-while!
        /// </summary>
        /// <param name="testDataRunId"></param>
        /// <param name="db"></param>
        internal static void Step2BeginCycleInit(int testDataRunId, ApplicationDBContext db)
        {
            TestDataRun run1 = db.GATreeTestDataRun.Find(testDataRunId);
            List<RecordTree> recs;
            switch (run1.TestDataRun_CurrentIterationNumber)
            {
                case -1: // first run - find the trees from the previous step
                             recs = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_isConsolidation == true &&
                                                                      x.Test_data_run_id == run1.Test_data_run_id &&
                                                                      x.RecordOfTrees_cycleStage == run1.TestDataRun_cycleStage &&
                                                                      x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber).ToList();

                            if (recs.Count !=1)
                            {
                                Utils.AddLogging($"Step2BeginCycleInit/switch/-1: Couldn't find the consolidated record: testDataRunId : {testDataRunId}", db);
                                return;
                            }
                        //update the data run
                        run1.TestDataRun_cycleStage = CycleStage.Evaluate;
                        run1.TestDataRun_runStage = RunStage.Cycle;
                        run1.TestDataRun_CurrentIterationNumber = 1;
                        db.SaveChanges();
                        db.GATRecordOfTrees.Add(new RecordTree()
                        {
                            RecordOfTrees_cycleStage = run1.TestDataRun_cycleStage,
                            RecordOfTrees_data = recs[0].RecordOfTrees_data,
                            RecordOfTrees_isConsolidation = true,
                            RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                            RecordOfTrees_runStage = run1.TestDataRun_runStage,
                            RecordOfTrees_treeStatus = TreesStatus.AllResults,
                            RecordTrees_thinClientGuid = Guid.Empty.ToString(),
                            Test_data_run_id = run1.Test_data_run_id
                        });
                         db.SaveChanges();

                        db.GATreeTestDataRunLog.Add(new TestDataRunLog()
                        {
                            TestDataRunLog_CycleStage = run1.TestDataRun_cycleStage,
                            TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                            Test_data_run_id = run1.Test_data_run_id,
                            TestDataRunLog_data = recs[0].RecordOfTrees_data,
                        });
                            Step3Point2CrossOverInit(testDataRunId, db);
                            return;
                case var value when value == run1.TestDataRun_MaxIterationNumber: //last run
                            // Find the consolidated record 
                            recs = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_isConsolidation == true &&
                                                                     x.RecordOfTrees_treeStatus == TreesStatus.AllResults &&
                                                                      x.Test_data_run_id == run1.Test_data_run_id &&
                                                                      x.RecordOfTrees_cycleStage == run1.TestDataRun_cycleStage &&
                                                                      x.RecordOfTrees_iterationNo == run1.TestDataRun_MaxIterationNumber).ToList();
                            if (recs.Count == 0)
                            {
                                Utils.AddLogging($"Step2BeginCycleInit/switch/run1.TestDataRun_MaxIterationNumber: Couldn't find the consolidated record: testDataRunId : {testDataRunId}", db);
                                return;
                            }
                            JsonRoot trees = JsonConvert.DeserializeObject<JsonRoot>(recs[0].RecordOfTrees_data);
                            List<JsonTree> trees1 = trees.trees;
                            SortJsonTree gg = new();
                            trees1.Sort(gg);
                            // from its data take the best tree
                            run1.TestDataRun_cycleStage = CycleStage.Ended;
                            run1.TestDataRun_runStage = RunStage.Ended;
                            run1.TestDataRun_CurrentIterationNumber = run1.TestDataRun_MaxIterationNumber;
                            run1.TestDataRun_results = JsonConvert.SerializeObject(trees1.Take(1).ToList()[0]);
                            run1.TestDataRun_dateEnd = DateTime.Now;
                            db.SaveChanges();
                            return;
                default: // We are in while
                            recs = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_isConsolidation == true &&
                                                                  x.RecordOfTrees_treeStatus == TreesStatus.AllResults &&
                                                                  x.Test_data_run_id == run1.Test_data_run_id &&
                                                                  x.RecordOfTrees_cycleStage == CycleStage.Mutation &&
                                                                  x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber).ToList();
                            if (recs.Count == 0)
                            {
                                Utils.AddLogging($"Step2BeginCycleInit/switch/run1.TestDataRun_MaxIterationNumber: Couldn't find the consolidated record: testDataRunId : {testDataRunId}", db);
                                return;
                            }
                            List<RecordTree> treesUsed = JsonConvert.DeserializeObject<List<RecordTree>>(recs[0].RecordOfTrees_data);
                            treesUsed.Sort();
                            treesUsed.Reverse();
                            run1.TestDataRun_cycleStage = CycleStage.Evaluate;
                            run1.TestDataRun_CurrentIterationNumber += 1;
                            run1.TestDataRun_results = JsonConvert.SerializeObject(treesUsed[0]);
                            db.SaveChanges();
                            db.GATRecordOfTrees.Add(new RecordTree()
                            {
                                RecordOfTrees_cycleStage = run1.TestDataRun_cycleStage,
                                RecordOfTrees_data = recs[0].RecordOfTrees_data,
                                RecordOfTrees_isConsolidation = true,
                                RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                                RecordOfTrees_runStage = run1.TestDataRun_runStage,
                                RecordOfTrees_treeStatus = TreesStatus.AllResults,
                                RecordTrees_thinClientGuid = Guid.Empty.ToString(),
                                Test_data_run_id = run1.Test_data_run_id
                            });
                            db.GATreeTestDataRunLog.Add(new TestDataRunLog()
                            {
                                TestDataRunLog_CycleStage = run1.TestDataRun_cycleStage,
                                TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                                Test_data_run_id = run1.Test_data_run_id,
                                TestDataRunLog_data = recs[0].RecordOfTrees_data,
                            });
                            db.SaveChanges();
                            Step2BeginCycleLastAction(testDataRunId, db);
                            return;

            }//switch

        }
        /// <summary>
        /// The last of step: Begin cycle
        /// </summary>
        /// <param name="testDataRunId"></param>
        /// <param name="db"></param>
        internal static void Step2BeginCycleLastAction(int testDataRunId, ApplicationDBContext db)
        {
            Step3Point2CrossOverInit(testDataRunId, db);
        }

        //------------------------------------------ Step 3.2 - CrossOver
        /// <summary>
        /// Inits the step 3.2 -  crossover. It creates the treerecords.
        /// </summary>
        /// <param name="testDataRunId">The data run Id</param>
        /// <param name="db">ApplicationDBContext</param>
        internal static void Step3Point2CrossOverInit(int testDataRunId, ApplicationDBContext db)
        {
            TestDataRun run1 = db.GATreeTestDataRun.Find(testDataRunId);
            List<RecordTree> recs = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_isConsolidation == true && 
                                                              x.Test_data_run_id == run1.Test_data_run_id &&
                                                              x.RecordOfTrees_cycleStage == run1.TestDataRun_cycleStage &&
                                                              x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber).ToList();
            if (recs.Count == 0)
            {
                Utils.AddLogging($"initStep2:no consolidated record found: testDataRunId:{testDataRunId} ", db);
                return;
            }
            //update the run 
            run1.TestDataRun_cycleStage = CycleStage.Crossover;
            db.SaveChanges();
            // Select n times 2 trees and create recordTrees
            int noOfCrossOver = GetNoOfCrossOverPairs(testDataRunId, db);
            if (noOfCrossOver == 0) // not crossOver in this cycle
            {
                // Add a log
                db.GATreeTestDataRunLog.Add(new TestDataRunLog()
                {
                    TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                    TestDataRunLog_Stage = RunStage.Cycle,
                    TestDataRunLog_CycleStage = run1.TestDataRun_cycleStage,
                    TestDataRunLog_comments = $"No of crossOver pairs: {noOfCrossOver} ",
                    Test_data_run_id = testDataRunId
                });
                db.SaveChanges();
                var rec2 = recs[0];
                // create the consolidated record for the next step
                db.GATRecordOfTrees.Add(new RecordTree()
                {
                    Test_data_run_id = rec2.Test_data_run_id,
                    RecordOfTrees_isConsolidation = true,
                    RecordOfTrees_cycleStage = run1.TestDataRun_cycleStage,
                    RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                    RecordOfTrees_data = rec2.RecordOfTrees_data,
                    RecordOfTrees_runStage = run1.TestDataRun_runStage,
                    RecordOfTrees_treeStatus = TreesStatus.AllResults
                });
                db.SaveChanges();
                Step3Point2CrossoverLastAction(testDataRunId, db);

            }//(noOfCrossOver == 0 )
            else
            {
                //Check if we have enough trees and adjust the number accordingly. This line shouldn't be run because it's a propablity
                noOfCrossOver = Step3Point2CrossOverReturnCorrectCrossOverNo(recs[0], noOfCrossOver, testDataRunId, db);
                List<int> treesUsed = new List<int>(); // which we have used
                List<JsonTree2> trees = Step3Point2CrossoverCreateJson2Trees(testDataRunId, noOfCrossOver, recs[0], db, out treesUsed);
                //We have the trees, we need to create the trees records for this step
                treesUsed = treesUsed.Distinct().ToList<int>(); //remove duplicate trees that were used
                var rec0 = recs[0];
                List<RecordTree> trees2DB = Step3Point2CrossoverCreateRecordTrees(rec0.RecordOfTrees_data, treesUsed, trees, db);
                foreach (var a in trees2DB)
                {
                    a.Test_data_run_id = rec0.Test_data_run_id;
                    a.RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber;
                    a.RecordOfTrees_cycleStage = run1.TestDataRun_cycleStage;
                    a.RecordOfTrees_runStage = run1.TestDataRun_runStage;
                    db.GATRecordOfTrees.Add(a);
                }
                db.SaveChanges();
            }//else
        }
        /// <summary>
        /// Creates the recordTrees that need to be added to the db. It creates the treerecords for the thincliens 
        /// and another one for the rest of the trees not been used
        /// </summary>
        /// <param name="recordOfTrees_data"> All the trees in string</param>
        /// <param name="treesUsed">List of trees used in crossover</param>
        /// <param name="treesCreated">the new trees that are created</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        private static List<RecordTree> Step3Point2CrossoverCreateRecordTrees(string recordOfTrees_data, List<int> treesUsed, List<JsonTree2> treesCreated, ApplicationDBContext db)
        {
            List<RecordTree> ret = new();
            // First create the records for each crossover
            foreach (var a in treesCreated)
            {
                ret.Add(new RecordTree()
                {
                    RecordOfTrees_data = JsonConvert.SerializeObject(a),
                    RecordOfTrees_treeStatus = TreesStatus.Init,
                    RecordOfTrees_isConsolidation = false,
                });
            }
            //Now, we have to delete the trees used in mutation  and add the remaining tree as a record
            List<JsonTree> trees1 = JsonConvert.DeserializeObject<JsonRoot>(recordOfTrees_data).trees;
            treesUsed.Sort();
            treesUsed.Reverse(); // Now that we have it in descending order, we can easily delete it because we start at the back.
            for (int i = 0; i < treesUsed.Count; i++)
            {
                trees1.RemoveAt(treesUsed[i]);
            }
            ret.Add(new RecordTree() // the others parameters will be given from the parent function!
            {
                RecordOfTrees_data = JsonConvert.SerializeObject(trees1),
                RecordOfTrees_treeStatus = TreesStatus.PartialResults,
                RecordOfTrees_isConsolidation = true
            });
            return ret;
        }
        /// <summary>
        /// This function checks if no of trees < noOfCrossOver*2 if it is, it returns the correct noOfCrossOver
        /// </summary>
        /// <param name="rec1"></param>
        /// <param name="noOfCrossOver"></param>
        /// <param name="testDataRunId">The data run Id</param>
        /// <param name="db"></param>
        /// <returns> a noOfCrossOver which is suitable for this number of trees</returns>
        static int Step3Point2CrossOverReturnCorrectCrossOverNo(RecordTree rec1, int noOfCrossOver, int testDataRunId, ApplicationDBContext db)
        {
            var trees = JsonConvert.DeserializeObject<JsonRoot>(rec1.RecordOfTrees_data).trees;

            if (trees.Count <= 1)
            {
                Utils.AddLogging($"Step3Point2CreateRecords error1:too few records :trees.Count {trees.Count} " +
                                 $"testDataRunId : {testDataRunId} noOfCrossOver:{noOfCrossOver} ", db);
                return 0;
            }
            if (trees.Count <= noOfCrossOver * 2) // We need at least 2 trees and as many as crossover*2
            {
                Utils.AddLogging($"Step3Point2CreateRecords error1:too many crossover:trees.Count {trees.Count} " +
                                 $"testDataRunId : {testDataRunId} noOfCrossOver:{noOfCrossOver} . Changed crossover to trees.Count/2 : {trees.Count / 2}", db);
 
                noOfCrossOver /= 2;
            }
            return noOfCrossOver;
        }
        /// <summary>
        /// Creates a list of JsonTree2 for Crossover
        /// </summary>
        /// <param name="testDataRunId">The data run Id</param>
        /// <param name="noOfCrossOver">No of crossovers - i.e. number of parents/s</param>
        /// <param name="rec2"> the tree record with all the trees</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <param name="treesUsed"> a list of int (trees) that are used</param>
        /// <returns> List of JsonTree2</returns>
        internal static List<JsonTree2> Step3Point2CrossoverCreateJson2Trees(int testDataRunId, int noOfCrossOver, RecordTree rec2, ApplicationDBContext db, out List<int> treesUsed)
        {
            // rec2 --> JsonTree
            List<JsonTree> trees = JsonConvert.DeserializeObject<JsonRoot>(rec2.RecordOfTrees_data).trees;
            List<JsonTree2> ret = new();
            treesUsed = new();
            double[] cumulativeWeights = new double[trees.Count];
            double cumulativeWeight = 0.0;
            for (int i = 0; i < trees.Count; i++)
            {
                cumulativeWeight += Convert.ToDouble(trees[i].evaluation);
                cumulativeWeights[i] = cumulativeWeight;
            }
            Random random = new Random();
            for (int i = 0; i < noOfCrossOver; i++) {
                // Find the two trees
                double randomValue1 = random.NextDouble() * cumulativeWeight;
                double randomValue2 = random.NextDouble() * cumulativeWeight;
                int index1 = Array.BinarySearch(cumulativeWeights, randomValue1) < 0 ? ~Array.BinarySearch(cumulativeWeights, randomValue1) : Array.BinarySearch(cumulativeWeights, randomValue1);
                int index2 = Array.BinarySearch(cumulativeWeights, randomValue2) < 0 ? ~Array.BinarySearch(cumulativeWeights, randomValue2) : Array.BinarySearch(cumulativeWeights, randomValue2);
                ret.Add(new JsonTree2() {
                    tree1 = trees[index1],
                    tree2 = trees[index2]
                });
                treesUsed.Add(index1);
                treesUsed.Add(index2);
            }
            return ret;
        }
        /// <summary>
        /// Last actions before going to mutation
        /// </summary>
        /// <param name="testDataRunId"> the test ID</param>
        /// <param name="db">ApplicationDBContext</param>
        static void Step3Point2CrossoverLastAction(int testDataRunId, ApplicationDBContext db)
        {
            Step3Point3Init(testDataRunId, db);
        }
        /// <summary>
        /// CrossOver GET - response to client get a job
        /// </summary>
        /// <param name="value"> The data of the client</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        internal static JsonServerResponse Step3Point2CrossoverGetaJob(JsonclientResponse response, ApplicationDBContext db)
        {

            int datarunId = response.datarunID;
            string clientGuid = response.clientGuid.ToString();
            // Find if the thin client is eligible
            var a = db.GATreeThinClient.Where(x => x.ThinClient_guid == clientGuid &&
                                              x.thinClient_isActive == true &&
                                              x.Test_data_run_id == datarunId).ToList();
            //if (a.Count == 0)
            if (false)
            {
                Utils.AddLogging($"Step3Point2CrossoverGet: no client found : datarunId = {datarunId} ,clientGuid = {clientGuid} ", db);
                return new JsonServerResponse()
                {
                    result = "FALSE",
                    message = $"Wrong Client Guid or dataRunId :{datarunId}: client response:{JsonConvert.SerializeObject(response)} ",
                    action = JobAction.none
                };
            }
            // Find which recordtree is available and update it, otherwise return error.
             var run1 = db.GATreeTestDataRun.Find(response.datarunID);
            if (run1 is null)
                    {
                return new JsonServerResponse()
                {
                    message = $"Wrong  dataRunId {datarunId}.  Client response:{JsonConvert.SerializeObject(response)} ",
                    action = JobAction.Busy,
                };
            }
            var treerec = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_treeStatus == TreesStatus.Init &&
                                                     x.Test_data_run_id == datarunId &&
                                                     x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                                     x.RecordOfTrees_cycleStage == CycleStage.Crossover &&
                                                     x.RecordOfTrees_runStage == RunStage.Cycle).ToList();
            if (treerec.Count == 0) // last record - Error!
            {
                Utils.AddLogging($"Step3Point2CrossoverGet: trees are full : datarunId = {datarunId} ,clientGuid = {clientGuid} ", db);
                return new JsonServerResponse()
                {
                    result = "BUSY",
                    message = $"Not any available tree records Client response:{JsonConvert.SerializeObject(response)}",
                    action = JobAction.none
                };
            }
            // update the tree record
            var recordTree = treerec[0];
            recordTree.RecordOfTrees_treeStatus = TreesStatus.WaitForResults;
            recordTree.RecordTrees_thinClientGuid = response.clientGuid.ToString();
            db.Update(recordTree);
            db.SaveChanges();
            return new JsonServerResponse()
            {
                action = JobAction.crossOver,
                treeRecordId = recordTree.RecordOfTrees_id,
                result = treerec[0].RecordOfTrees_data
            };
        }
        /// <summary>
        /// CrossOver POST - response to client sending a package
        /// </summary>
        /// <param name="value"> The data of the client</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        internal static JsonServerResponse Step3Point2CrossoverPost(JsonclientResponse response, ApplicationDBContext db)
        {
            // TODO: Check if the client ID matches
            var a = db.GATRecordOfTrees.Find(response.TreeRecordId);
            if (a is null)
            {
                Utils.AddLogging($"step3Point2CrossoverPost: couldn't find treerecord:{response.TreeRecordId}. client send: {JsonConvert.SerializeObject(response)}", db);
                return new JsonServerResponse
                {
                    result = "ERROR",
                    action = response.action,
                    treeRecordId = response.TreeRecordId,
                    message = $"Error step3Point2CrossoverPost: couldn't find treerecord:{response.TreeRecordId}"
                };
            }
            //TODO Check if the client is eligible to give this information
            if (a.RecordOfTrees_treeStatus != TreesStatus.WaitForResults)
            {
                return new JsonServerResponse
                {
                    result = "ERROR",
                    action = response.action,
                    treeRecordId = response.TreeRecordId,
                    message = $" Error step3Point2CrossoverPost: Record has already data:for id {response.TreeRecordId} .Client send: {JsonConvert.SerializeObject(response)}"
                };
            }
            a.RecordOfTrees_data = response.result;
            a.RecordOfTrees_treeStatus = TreesStatus.Finished;
            db.SaveChanges();
            // We have to check if all the records have been filled with data to proceed to the next step
            var b = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == a.Test_data_run_id &&
                                                   (x.RecordOfTrees_treeStatus == TreesStatus.WaitForResults ||
                                                     x.RecordOfTrees_treeStatus == TreesStatus.Init
                                                   ) &&
                                                   x.Test_data_run_id == a.Test_data_run_id)
                                                   .ToList().Count;
            if (b == 0)
            {
                Step3Point2ConsolidateTrees(response.datarunID, db);
                Step3Point2CrossoverLastAction(response.datarunID, db);
            }
            return new JsonServerResponse()
            {
                action = response.action,
                result = "OK",
                message = " Trees has been recorded",
                treeRecordId = response.TreeRecordId
            };
        }
        /// <summary>
        /// Cross Over - consolidate trees
        /// </summary>
        /// <param name="datarunID"> data run ID</param>
        /// <param name="db">ApplicationDBContext</param>
        private static void Step3Point2ConsolidateTrees(int testDatarunId, ApplicationDBContext db)
        {

            var run1 = db.GATreeTestDataRun.Find(testDatarunId);
            if (run1 is null)
            {
                Utils.AddLogging($"Step3Point2ConsolidateTrees: datarun not found : testDatarunId = {testDatarunId} ", db);
                return;
            }
            // Take all treerecords' data and merge them to one and save it to a treerecord. Start with the finished records
            var treeRecs = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == testDatarunId &&
                                              x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                              x.RecordOfTrees_cycleStage == CycleStage.Crossover &&
                                              x.RecordOfTrees_treeStatus == TreesStatus.Finished &&
                                              x.RecordOfTrees_isConsolidation == false
                                              ).ToList();
            if (treeRecs.Count == 0)
            {
                Utils.AddLogging($"Step3Point2ConsolidateTrees: no trees found:testDatarunId : {testDatarunId}", db);
                return;
            }
            var consolidateRec = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == testDatarunId &&
                                              x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                              x.RecordOfTrees_treeStatus == TreesStatus.PartialResults &&
                                              x.RecordOfTrees_cycleStage == CycleStage.Crossover &&
                                              x.RecordOfTrees_isConsolidation == true
                                              ).ToList();
            if (consolidateRec.Count == 0)
            {
                Utils.AddLogging($"Step3Point2ConsolidateTrees: no consolidate tree found:testDatarunId : {testDatarunId}", db);
                return;
            }
            db.GATRecordOfTrees.Add(new RecordTree()
            {
                RecordOfTrees_data = ConsolidateRecordTrees2(treeRecs, consolidateRec),
                RecordOfTrees_runStage = RunStage.Cycle,
                RecordOfTrees_cycleStage = CycleStage.Crossover,
                RecordOfTrees_isConsolidation = true,
                RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                Test_data_run_id = testDatarunId,
                RecordOfTrees_treeStatus = TreesStatus.AllResults,
            });
            db.SaveChanges();
            db.Add(new TestDataRunLog()
            {
                Test_data_run_id = testDatarunId,
                TestDataRunLog_Stage = RunStage.Cycle,
                TestDataRunLog_CycleStage = CycleStage.Choosing,
                TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                TestDataRunLog_comments = $"Step3Point2ConsolidateTrees: Merging and sorting the records in recordstree",
            });
            db.SaveChanges();
        }
        //---------------------------------------- Step 3.3 Mutation ---------------------------
        static void Step3Point3Init(int testDataRunId, ApplicationDBContext db)

        {
            //Find the consolidated recordtree
            TestDataRun run1 = db.GATreeTestDataRun.Find(testDataRunId);
            run1.TestDataRun_cycleStage = CycleStage.Mutation;
            db.SaveChanges();
            List<RecordTree> treeRecs = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_isConsolidation == true &&
                                                              x.RecordOfTrees_cycleStage == CycleStage.Crossover &&
                                                              x.RecordOfTrees_treeStatus == TreesStatus.AllResults &&
                                                              x.Test_data_run_id == run1.Test_data_run_id &&
                                                              x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber)
                                                              .ToList();
            if (treeRecs.Count == 0)
            {
                Utils.AddLogging($"initStep2:no record found ", db);
                return;
            }
            var rec0 = treeRecs[0];
            var mutations = Step3Point3ReturnCorrectMutationNo(rec0, GetNoOfMutation(testDataRunId, db), testDataRunId, db);
            if (mutations == 0) // no mutations, going to the next step
            {
                // Add a log
                db.GATreeTestDataRunLog.Add(new TestDataRunLog()
                {
                    TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                    TestDataRunLog_Stage = RunStage.Cycle,
                    TestDataRunLog_CycleStage = CycleStage.Mutation,
                    TestDataRunLog_comments = $"No of mutations : {mutations} ",
                    Test_data_run_id = testDataRunId
                });
                db.SaveChanges();
                db.GATRecordOfTrees.Add(new RecordTree()
                {
                    Test_data_run_id = run1.Test_data_run_id,
                    RecordOfTrees_isConsolidation = true,
                    RecordOfTrees_cycleStage = CycleStage.Mutation,
                    RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                    RecordOfTrees_data = rec0.RecordOfTrees_data,
                    RecordOfTrees_runStage = RunStage.Cycle,
                    RecordOfTrees_treeStatus = TreesStatus.AllResults
                });
                db.SaveChanges();
                Step3Point3LastAction(testDataRunId, db);
                return;
            }
            else // we have mutations!!
            {
                // Create the record trees
                List<int> treesUsed = new();
                var jsontrees = Step3Point3MutationCreateJsonTrees(rec0, mutations, testDataRunId,out treesUsed, db);
                List<RecordTree> trees2DB = Step3Point3MutationCreateRecordTrees(rec0.RecordOfTrees_data, treesUsed, jsontrees, db);
                foreach (var a in trees2DB)
                {
                    a.Test_data_run_id = run1.Test_data_run_id;
                    a.RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber;
                    a.RecordOfTrees_cycleStage = run1.TestDataRun_cycleStage;
                    a.RecordOfTrees_runStage = run1.TestDataRun_runStage;
                    db.GATRecordOfTrees.Add(a);
                }
                db.SaveChanges();
            }

        }
        /// <summary>
        /// Creates the json trees for the mutation
        /// </summary>
        /// <param name="Treerec"> the treerecord</param>
        /// <param name="noOfMutations">no of crossOver</param>
        /// <param name="testDataRunId"> run Id</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        internal static List<JsonTree> Step3Point3MutationCreateJsonTrees(RecordTree Treerec, int noOfMutations, int testDataRunId, out List<int> treesUsed, ApplicationDBContext db)
        {
            List<JsonTree> trees = JsonConvert.DeserializeObject< List<JsonTree>>(Treerec.RecordOfTrees_data);
            // Floyd's algorithm to select n unique numbers out of noOfMutations
            Random random = new Random();
            treesUsed = Enumerable.Range(0, trees.Count - 1).OrderBy(t => random.Next()).Take(noOfMutations).ToList();
            List<JsonTree> ret = new();
            foreach ( var i in treesUsed)
            {
                ret.Add(trees[i]);
            }
            return ret;
        }
        private static List<RecordTree> Step3Point3MutationCreateRecordTrees(string recordOfTrees_data, List<int> treesUsed, List<JsonTree> treesCreated, ApplicationDBContext db)
        {
            List<RecordTree> ret = new();
            // First create the records for each mutation
            foreach (var a in treesCreated)
            {
                ret.Add(new RecordTree()
                {
                    RecordOfTrees_data = JsonConvert.SerializeObject(a),
                    RecordOfTrees_treeStatus = TreesStatus.Init,
                    RecordOfTrees_isConsolidation =  false,
                });
            }
            //Now, we have to delete the trees used in mutation  and add the remaining tree as a record
            List<JsonTree> trees1 = JsonConvert.DeserializeObject<List<JsonTree>>(recordOfTrees_data);
            treesUsed.Sort();
            treesUsed.Reverse(); // Now that we have it in descending order, we can easily delete it because we start at the back.
            for (int i = 0; i < treesUsed.Count; i++)
            {
                trees1.RemoveAt(treesUsed[i]);
            }
            ret.Add(new RecordTree()
            {
                RecordOfTrees_data = JsonConvert.SerializeObject(trees1),
                RecordOfTrees_treeStatus = TreesStatus.PartialResults,
                RecordOfTrees_isConsolidation = true,
            });
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testDataRunId">Run Id</param>
        /// <param name="db"></param>
        internal static void Step3Point3LastAction(int testDataRunId, ApplicationDBContext db)
        {
            var run1 = db.GATreeTestDataRun.Find(testDataRunId);
            //consolidate to one record
            // read the records from clients
            List<RecordTree> rec1 = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_isConsolidation == false &&
                                                 x.RecordOfTrees_cycleStage == CycleStage.Mutation &&
                                                 x.RecordOfTrees_treeStatus == TreesStatus.Finished &&
                                                 x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                                 x.Test_data_run_id == testDataRunId).ToList();
            //read the record tree with the rest of trees.
            List<RecordTree> rec2 = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_isConsolidation == true &&
                                                 x.RecordOfTrees_cycleStage == CycleStage.Mutation &&
                                                 x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                                 x.Test_data_run_id == testDataRunId).ToList();
            db.GATRecordOfTrees.Add(new RecordTree()
            {
                RecordOfTrees_data =  ConsolidateRecordTrees2(rec1, rec2),
                RecordOfTrees_isConsolidation = true,
                RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                RecordOfTrees_runStage = RunStage.Cycle,
                RecordOfTrees_cycleStage = CycleStage.Mutation,
                RecordOfTrees_treeStatus = TreesStatus.AllResults,
                Test_data_run_id = testDataRunId,
            });           
            db.SaveChanges(); // not needed but to clarify that two objects are been updated.
            run1.TestDataRun_cycleStage = CycleStage.Mutation;
            db.SaveChanges();
        }
        internal static JsonServerResponse Step3Point3MutationGetaJob(JsonclientResponse response, ApplicationDBContext db)
        {
                int datarunId = response.datarunID;
                string clientGuid = response.clientGuid.ToString();

            if (Utils.IsClientValid(response.datarunID, response.clientGuid, db))
                ;
            if (false)
            {
                Utils.AddLogging($"Step3Point3MutationGetaJob: no client found : datarunId = {datarunId} ,clientGuid = {clientGuid} ", db);
                return new JsonServerResponse()
                {
                    result = "FALSE",
                    message = $"Wrong Client Guid or dataRunId :{datarunId}: client response:{JsonConvert.SerializeObject(response)} ",
                    action = JobAction.none
                };
            }
            // Find which recordtree is available and update it, otherwise return error.
            var run1 = db.GATreeTestDataRun.Find(response.datarunID);
            if (run1 is null)
            {
                return new JsonServerResponse()
                {
                    action = JobAction.Busy,
                    message = $"Wrong  dataRunId {datarunId}.  Client response:{JsonConvert.SerializeObject(response)} ",
                };
            }
            var rec = db.GATRecordOfTrees.Where(x => x.RecordOfTrees_treeStatus == TreesStatus.Init &&
                                                     x.Test_data_run_id == datarunId &&
                                                     x.RecordOfTrees_isConsolidation == false &&
                                                     x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                                     x.RecordOfTrees_cycleStage == run1.TestDataRun_cycleStage).ToList();
            if (rec.Count == 0) // last record - Error!
            {
                Utils.AddLogging($"Step3Point3MutationGetaJob: trees are full : datarunId = {datarunId} ,clientGuid = {clientGuid} ", db);
                return new JsonServerResponse()
                {
                    result = JobAction.Busy,
                    message = $"Not any available tree records. Client response:{JsonConvert.SerializeObject(response)}",
                    action = response.action
                };
            }
            // update the tree record
            var recordTree = rec[0];
            recordTree.RecordOfTrees_treeStatus = TreesStatus.WaitForResults;
            recordTree.RecordTrees_thinClientGuid = response.clientGuid.ToString();
            db.Update(recordTree);
            db.SaveChanges();
            return new JsonServerResponse()
            {
                action = JobAction.mutate,
                treeRecordId = recordTree.RecordOfTrees_id,
                result = rec[0].RecordOfTrees_data
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response">json client response</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns>JsonServerResponse</returns>
        internal static JsonServerResponse Step3Point3MutationPost(JsonclientResponse response, ApplicationDBContext db)
        {
            // TODO: Check if the client ID matches
            var a = db.GATRecordOfTrees.Find(response.TreeRecordId);
            if (a is null)
            {
                Utils.AddLogging($"Step3Point3MutationPost: couldn't find treerecord:{response.TreeRecordId}. client send: {JsonConvert.SerializeObject(response)}", db);
                return new JsonServerResponse
                {
                    result = "ERROR",
                    action = response.action,
                    treeRecordId = response.TreeRecordId,
                    message = $"Error Step3Point3MutationPost: couldn't find treerecord:{response.TreeRecordId}"
                };
            }
            //TODO Check if the client is eligible to give this information
            if (a.RecordOfTrees_treeStatus != TreesStatus.WaitForResults)
            {
                return new JsonServerResponse
                {
                    result = "ERROR",
                    action = response.action,
                    treeRecordId = response.TreeRecordId,
                    message = $" Error Step3Point3MutationPost: Record has already data:for id {response.TreeRecordId} .Client send: {JsonConvert.SerializeObject(response)}"
                };
            }
            a.RecordOfTrees_data = response.result;
            a.RecordOfTrees_treeStatus = TreesStatus.Finished;
            db.SaveChanges();
            // We have to check if all the records have been filled with data to proceed to the next step
            var b = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == a.Test_data_run_id &&
                                                   (x.RecordOfTrees_treeStatus == TreesStatus.WaitForResults ||
                                                     x.RecordOfTrees_treeStatus == TreesStatus.Init
                                                   ) &&
                                                   x.Test_data_run_id == a.Test_data_run_id)
                                                   .ToList().Count;
            if (b == 0)
            {
                Step3Point3MutationConsolidateTrees(response.datarunID, db);
                Step3Point3MutationLastAction(response.datarunID, db);
            }
            return new JsonServerResponse()
            {
                action = response.action,
                result = "OK",
                message = " Trees has been recorded",
                treeRecordId = response.TreeRecordId
            };
        }
        private static void Step3Point3MutationConsolidateTrees(int testDatarunId, ApplicationDBContext db)
        {
            var run1 = db.GATreeTestDataRun.Find(testDatarunId);
            if (run1 is null)
            {
                Utils.AddLogging($"Step3Point3ConsolidateTrees: datarun not found : testDatarunId = {testDatarunId} ", db);
                return;
            }
            // Take all treerecords' data and merge them to one and save it to a treerecord. Start with the finished records
            var treeRecs = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == testDatarunId &&
                                              x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                              x.RecordOfTrees_treeStatus == TreesStatus.Finished &&
                                              x.RecordOfTrees_cycleStage == CycleStage.Mutation &&
                                              x.RecordOfTrees_isConsolidation == false
                                              ).ToList();
            if (treeRecs is null || treeRecs.Count == 0)
            {
                Utils.AddLogging($"Step3Point3ConsolidateTrees: no trees found:testDatarunId : {testDatarunId}", db);
                return;
            }
            var consolidateRec = db.GATRecordOfTrees.Where(x => x.Test_data_run_id == testDatarunId &&
                                              x.RecordOfTrees_iterationNo == run1.TestDataRun_CurrentIterationNumber &&
                                              x.RecordOfTrees_treeStatus == TreesStatus.PartialResults &&
                                              x.RecordOfTrees_cycleStage == CycleStage.Mutation &&
                                              x.RecordOfTrees_isConsolidation == true
                                              ).ToList();
            if (consolidateRec.Count == 0)
            {
                Utils.AddLogging($"Step3Point3ConsolidateTrees: no consolidate tree found:testDatarunId : {testDatarunId}", db);
                return;
            }

            db.GATRecordOfTrees.Add(new RecordTree()
            {
                RecordOfTrees_data =  ConsolidateRecordTrees(treeRecs, consolidateRec),
                RecordOfTrees_runStage = RunStage.Cycle,
                RecordOfTrees_cycleStage = CycleStage.Mutation,
                RecordOfTrees_isConsolidation = true,
                RecordOfTrees_iterationNo = run1.TestDataRun_CurrentIterationNumber,
                Test_data_run_id = testDatarunId,
                RecordOfTrees_treeStatus = TreesStatus.AllResults,
            });
            db.SaveChanges();
            db.Add(new TestDataRunLog()
            {
                Test_data_run_id = testDatarunId,
                TestDataRunLog_Stage = RunStage.Cycle,
                TestDataRunLog_CycleStage = CycleStage.Evaluate,
                TestDataRunLog_IterationNo = run1.TestDataRun_CurrentIterationNumber,
                TestDataRunLog_comments = $"Step3Point3ConsolidateTrees: Merging and sorting the records in recordstree",
            });
             db.SaveChanges();

        }
        /// <summary>
        /// last action - we close the circle and go to the top again.
        /// </summary>
        /// <param name="datarunID"> data run id</param>
        /// <param name="db">ApplicationDBContext</param>
        private static void Step3Point3MutationLastAction(int datarunID, ApplicationDBContext db)
        {
            Step2BeginCycleInit(datarunID, db);
        }

        /// <summary>
        /// returns the correct mutation number
        /// </summary>
        /// <param name="rec1"> the treerecord</param>
        /// <param name="MutationNo">currrent mutation number</param>
        /// <param name="testDataRunId"> run Id</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        static int Step3Point3ReturnCorrectMutationNo(RecordTree rec1, int MutationNo, int testDataRunId, ApplicationDBContext db)
        {
            List<JsonTree> jTrees = JsonConvert.DeserializeObject<List<JsonTree>>(rec1.RecordOfTrees_data);

            if (jTrees.Count <= 1)
            {
                Utils.AddLogging($"Step3Point3ReturnCorrectMutationNo error1:too few records :trees.Count {jTrees.Count} " +
                                  $"testDataRunId : {testDataRunId} noOfCrossOver:{MutationNo} ", db);
                return 0;
            }
            if (jTrees.Count <= MutationNo * 2) // We need at least 2 trees and as many as crossover*2
            {
                Utils.AddLogging($"Step3Point2CreateRecords error1:too many crossover:trees.Count {jTrees.Count} " +
                                 $"testDataRunId : {testDataRunId} noOfCrossOver:{MutationNo} . Changed crossover to trees.Count/2 : {jTrees.Count / 2}", db);
                MutationNo /= 2;
            }
            return MutationNo;
        }

        //---------------------------------------- UTILS ---------------------------------------

        /// <summary>
        /// Consolidates two recordtrees and returns one item with all the record trees 
        /// The record tree should have a JsonTree2 .
        /// </summary>
        /// <param name="recs1">List of the records</param>
        /// <param name="rec2">the partial results</param>
        /// <remarks> rec2 could not be a list but for future purposes it's a list</remarks>
        /// <returns>a json string with all the trees</returns>
        static internal string ConsolidateRecordTrees2( List<RecordTree> recs1, List<RecordTree> recs2 )
        {
            // Make it Jsontree because the data is json format
            
            List<JsonTree> trees1 = new();
            foreach (var a in recs1)         
            {

                var trees = JsonConvert.DeserializeObject<List<JsonTree>>(a.RecordOfTrees_data);

                trees1.AddRange(trees);
            }
            foreach (var a in recs2)
            {
                var trees = JsonConvert.DeserializeObject<List<JsonTree>>(a.RecordOfTrees_data);
                trees1.AddRange(trees);
            }

            return JsonConvert.SerializeObject(trees1);
        }
        /// <summary>
        /// Consolidates two recordtrees and returns a json string with all the trees
        /// The record tree should have a JsonTree .
        /// </summary>
        /// <param name="rec1">List of the records</param>
        /// <param name="rec2">the partial results</param>
        /// <remarks> rec2 could not be a list but for future purposes it's a list</remarks>
        /// <returns>a json tree with all the trees</returns>
        static internal string ConsolidateRecordTrees(List<RecordTree> rec1, List<RecordTree> rec2)
        {
            List<JsonTree> trees1 = new();
            foreach (var a in rec1)
            {
                trees1.AddRange(JsonConvert.DeserializeObject<List<JsonTree>>(a.RecordOfTrees_data));
            }
            foreach (var b in rec2)
            {
                var bb = JsonConvert.DeserializeObject<List<JsonTree>>(b.RecordOfTrees_data);
                trees1.AddRange(bb);
            }

            return JsonConvert.SerializeObject(new JsonRoot()
            {
                trees = trees1
            });
        }
        /// <summary>
        /// How many treerecords need to be made for step 1
        /// </summary>
        /// <param name="testDataRunId"></param>
        /// <param name="testDataId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        internal static int Step0GetMaximumTreeRecords(int testDataRunId, int testDataId, ApplicationDBContext db)
        {
            return 10;
        }

        /// <summary>
        /// Returns the number of crossOvers that should be done
        /// </summary>
        /// <param name="testDataRunId"> The runIdItRuns</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns>the number of pairs</returns>
        internal static int GetNoOfCrossOverPairs(int testDataRunId, ApplicationDBContext db)
        {
            var a = Utils.GetTestDataObjectFromTestDataRunId(testDataRunId, db);
            return (int)((a.TestData_crossoverPropability * a.TestData_numberOfMaxTrees / 100) + 1);

        }
        /// <summary>
        /// returns the number of mutations that should be done
        /// </summary>
        /// <param name="testDataRunId"> testData ID</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        internal static int GetNoOfMutation(int testDataRunId, ApplicationDBContext db)
        {
            var a = Utils.GetTestDataObjectFromTestDataRunId(testDataRunId, db);
            return (int)((a.TestData_mutationPropability * a.TestData_numberOfMaxTrees/100) + 1);
        }
        /// <summary>
        ///  How many treesrecords we need to create
        /// </summary>
        /// <param name="testDataRunId"> run Id</param>
        /// <param name="testDataId"> test data ID - maybe not needed</param>
        /// <param name="db">ApplicationDBContext</param>
        /// <returns></returns>
        internal static int Step1CreateInitGetTreeMultiplier(int testDataRunId, int testDataId, ApplicationDBContext db)
        {
            return 10;
        }
    }//class

}//namespace
