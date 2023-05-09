using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAtreeWebStore.ViewModels
{
    //-------------------------- trees records
    /// <summary>
    /// used to (de)serialize json trees
    /// </summary>
    public class JsonRoot
    {
        public List<JsonTree> trees { get; set; }
    }
    /// <summary>
    /// For crossOver (2 trees are need to send), one to receive.
    /// </summary>
    public class JsonTree2
    {
        public JsonTree tree1 { get; set; }
        public JsonTree tree2 { get; set; }
    }
    /// <summary>
    /// A tree with its evaluation
    /// </summary>
    public class JsonTree
    {
        public string tree { get; set; }
        public string evaluation { get; set; }
    }
    // -------------------------------------- Client - server responses in Json
    /// <summary>
    /// enum actions send/receive
    /// </summary>
    public class JobAction
    {
        public const string none = "NONE";
        public const string OK = "OK";
        public const string error = "ERROR";
        public const string Busy = "BUSY";

        public const string createTrees = "CREATETREES";
        public const string mutate = "MUTATION";
        public const string crossOver = "CROSSOVER";
        public const string clientGetJob = "CLIENTGETJOB";

        public static readonly string[] cycleActions = {createTrees, mutate, crossOver, clientGetJob };

        public const string registerClient = "REGISTERCLIENT";
        public const string unRegisterClient = "UNREGISTERCLIENT";
        public const string getData = "GETDATA";
        public const string getAttributes = "GETATTRIBUTES";
        public const string getAttributesText = "GETATTRIBUTESTEXT";
        public const string getParameters = "GETPARAMETERS";

        public static readonly string[] nonCycleActions = { registerClient, unRegisterClient, getData, getAttributes, 
                                                            getAttributesText, getParameters };
        public static readonly string[] actions = nonCycleActions.Union(cycleActions).ToArray();
    }
    /// <summary>
    ///  The response from the server. The result could be a Jsontree or a JsonTree2
    /// </summary>
    public class JsonServerResponse
    {
        public string result { get; set; } = JobAction.error;
        public string action { get; set; } = JobAction.none;
        public int treeRecordId { get; set; } = -1;
        public Guid clientGuid { get; set; } = Guid.Empty;
        public string message { get; set; } = null;
    }
    /// <summary>
    /// The client response. The result could be a Jsontree or a JsonTree2
    /// </summary>
    public class JsonclientResponse
    {
        public int datarunID { get; set; } = -1;
        public string action { get; set; } = JobAction.none;
        public Guid clientGuid { get; set; } = Guid.Empty;
        public int TreeRecordId { get; set; } = -1;
        public string result { get; set; } = null;
    }
}
