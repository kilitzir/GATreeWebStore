using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using System.Threading.Tasks;
/// <summary>
/// All the data saved to SQL server
/// </summary>
namespace GAtreeWebStore.Models
{
    /// <summary>
    ///  The stages of the run
    /// </summary>
    public enum RunStage
    {
        NotAplicable = -81, //For future use
        NotStarted = 81,    // It hasn't been started yet
        Started = 82,       // Started
        initializing = 800, // Initializing the primary trees
        Cycle = 820,        // We are in a cycle
        last = 830,         // Stage after the cycle to wrap up
        Ended = 890,        // ended
        InitValue = 810     // InitValue
    }
    /// <summary>
    ///  The stages of the RunStage.Cycle (above)
    /// </summary>
    public enum CycleStage
    {
        InitValue  = -71, //For future use
        NotStarted = 71,  // if we aren't in a cycle - 
        Evaluate   = 710, // For future use
        Choosing   = 720, // get the <n> trees
        Crossover  = 730, // CrossOver
        Mutation   = 740, // Mutation
        Ended      = 790  //  Cycle has ended.
    }
    ///
    /// <summary>
    /// The Data of the .arff file
    /// </summary>
    public class Data
    {
        [Key]
        public int Data_id { get; set; } //Id of the data
        [Required]
        public string Data_guid { get; set; } = Guid.NewGuid().ToString(); // Guid of the data, diffferent ID
        [Required]
        public string Data_friendlyname { get; set; } // Friendly name for easy remembering
        public  string Data_notes { get; set; }  // Notes
        public string Data_json { get; set; }  // Data in Json Format of the .arff file
        public string attributes_text { get; set; }  // attributes in string format
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data_date { get; set; } = DateTime.Now; // Date it was saved
        public string Data_header { get; set; } // The header of the .arff file
        public string Data_URL { get; set; } = null;// The data's URL 0- if y have the URl instead of a file.
    }
    /// <summary>
    ///Saves the attributes for a specific data structure 
    /// </summary>
    public class DataAttributes
    {
        [Key]
        public int DataAttributes_Id { get; set; }
        [Required]
        public string DataAttributes_guid { get; set; } = Guid.NewGuid().ToString();
        public int DataAttributes_sn { get; set; } // The serial number of the attribute
        public string DataAttributes_name { get; set; } // the name
        public string DataAttributes_type { get; set; } // Type: Numeric,date,string, class
        public string DataAttributes_attributesData { get; set; } // If it's string, it can have multiple values
        public bool DataAttributes_isClass { get; set; } = false; // If the name is class - needed?
        public bool DataAttributes_hasNullValues { get; set; } = false; // If it has NullValues -  needed?
        public int data_DataAttributes_data_ID { get; set; } //???
        public int data_DataAttributes_intMax { get; set; } = int.MinValue;
        public int data_DataAttributes_intMin { get; set; } = int.MaxValue;
        public float data_DataAttributes_floatMax { get; set; } = float.MinValue;
        public float data_DataAttributes_floatMin { get; set; } = float.MaxValue;

        [ForeignKey("data_DataAttributes_data_ID")]
        public virtual Data Data { get; set; }
    }
    /// <summary>
    ///This the the test that we define with its own parameters.  It connects with the associated data and 
    ///with the relevant runs - test the model. 
    /// </summary>
    public class TestData
    {
        [Key]
        public int TestData_id { get; set; }
        [Required]
        public string TestData_friendlyName { get; set; }
        [Required]
        public string TestData_guid { get; set; } = Guid.NewGuid().ToString();
        public string TestData_notes { get; set; }
        [Required]
        [Range(0, 10000)]
        public int TestData_numberOfMaxTrees { get; set; }
        [Required]
        [Range(0, 10000)]
        public int TestData_numberOfMaxGenerations { get; set; }
        [Required]
        [Range(0.0, 1.0)]
        public float TestData_mutationPropability { get; set; } // %
        [Required]
        [Range(0.0, 1.0)]
        public float TestData_crossoverPropability { get; set; }
        [Required]
        [Range(0.0, 1.0)]
        public int TestData_generationGap { get; set; }
        [Required]
        /// TODO: Add range
        public float TestData_correctionfactor { get; set; }
        public string TestData_params { get; set; }
        [ForeignKey("TestData_data_id")]
        public int TestData_data_id { get; set; }
        public virtual Data Data { get; set; }
    }
    //-----------------------------------------------
    /// <summary>
    /// The elements for a Run. It It connects with the associated testdata (data parameter) and connects with 
    /// the log for every step of the algorithm
    /// </summary>

    public class TestDataRun
    {
        [Key]
        public int Test_data_run_id { get; set; } //Id of the data
        [Required]
        public string TestDataRun_guid { get; set; } = Guid.NewGuid().ToString(); // Guid of the data, diffferent ID
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TestDataRun_dateStart { get; set; }  // Date it was starts to run
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TestDataRun_dateEnd { get; set; } // Date it was starts to run
        public string TestDataRun_comments { get; set; } = null; //comments
        public RunStage TestDataRun_runStage { get; set; } = RunStage.NotStarted; // Condition that a run is at
        public CycleStage TestDataRun_cycleStage { get; set; } = CycleStage.NotStarted; // 
        public int TestDataRun_CurrentIterationNumber { get; set; } = -1; // The iteration that we are.
        public int TestDataRun_MaxIterationNumber { get; set; } = -1; // The max iteration - we save it here for easy finding.

        public string TestDataRun_results { get; set; } = null;// the intermidiate/final results of the run
        [ForeignKey("TestData_id")]
        public int TestData_id { get; set; }
        public virtual TestData testdata { get; set; }
    }
    //---------------------------------------------
    /// <summary>
    ///  It logs the data for every step of a run. It's associated with the testdatarun - the run that it's running
    /// </summary>
    public class TestDataRunLog
    {
        [Key]
        public int TestDataRunLog_id { get; set; } //Id of the log
        [Required]
        public string TestDataRunLog_guid { get; set; } = Guid.NewGuid().ToString();
        public int TestDataRunLog_IterationNo { get; set; } = 0; // the iteration it is: it's 1--->xxxx TODO: Enumerate!
        public RunStage TestDataRunLog_Stage = RunStage.NotStarted; // for which stage is the data
        public CycleStage TestDataRunLog_CycleStage = CycleStage.NotStarted; // for which stage in cycle is the data
        public string TestDataRunLog_data { get; set; } = null;//The data for the particular step in Json
        public string TestDataRunLog_comments { get; set; } = null; // Any comments
        public string TestDataRunLog_BestResult { get; set; } = null; // What is the best result in this step, if it's suitable
        public int Test_data_run_id { get; set; }
        [ForeignKey("Test_data_run_id")]
        public virtual TestDataRun testdataRun { get; set; }
    }
    /// <summary>
    /// // The state of the record tree
    /// </summary>
    public enum TreesStatus 
    {
        Init = 0, // it's initilialized, maybe unused
        WaitForResults = 10,   // it's send, we wait fot the results
        Finished = 20,         // we have the results
        AllResults = 30,       // The results of the previous step
        PartialResults = 40,    // For mutation and crossover, some trees aren't involved - we save them here.
        coulBeDiscarded = 99,   // We have evaluated its results and now it's not needed anymore
        InitValue = -1
    }
    /// <summary>
    /// The packet of information that will be sent (and received) from the clients.
    /// </summary>
    public class RecordTree
    {
        [Key]
        public int RecordOfTrees_id { get; set; } //Id of the record
        [Required]
        public string RecordOfTrees_guid { get; set; } = Guid.NewGuid().ToString();
        public string RecordOfTrees_data { get; set; } = null; // The trees and/or any other data.
        public TreesStatus  RecordOfTrees_treeStatus { get; set; } = TreesStatus.Init; // the status of the record
        public RunStage RecordOfTrees_runStage { get; set; } = RunStage.InitValue;
        public CycleStage RecordOfTrees_cycleStage { get; set; } = CycleStage.InitValue;
        public bool RecordOfTrees_isConsolidation { get; set; } = false;
        public int RecordOfTrees_iterationNo { get; set; } = -1; // The iteration that belongs to.
        public int Test_data_run_id { get; set; }
        public string RecordTrees_thinClientGuid { get; set; } = Guid.Empty.ToString();// The thinclien who did the job. It can't be a foreighn key because when we initialize it, it's empty
        [ForeignKey("Test_data_run_id")]
        public virtual TestDataRun testDataRun { get; set; } // Which run belongs to
        /*
        [ForeignKey("ThinClient_id")]
        public virtual ThinClient thinClient { get; set; } // The client assigned the job
        */
    }
}//namespace
