using Microsoft.AspNetCore.Http;

namespace GAtreeWebStore.ViewModels
{
    public class DataAddFromFileViewModel
    {        
        public string Data_friendlyname { get; set; } // Friendly name for easy remembering
        public string Data_notes { get; set; }  // Notes
        public IFormFile file1 { get; set; }
    }
    // 
    /// <summary>
    ///class to pass to the web api 
    /// </summary>
    public class DataAttributesWebApiModel 
    {
        public int sn { get; set; }                // The serial number of the attribute
        public string name { get; set; }           // the name
        public string type { get; set; }            // Type: Numeric,date,string, class
        public string attributesData { get; set; } // If it's string, it can have multiple values
        public bool isClass { get; set; }          // If the name is class - needed?
        public bool hasNullValues { get; set;}     // If it has NullValues -  needed?
        public string Max { get; set; } = "";         // If it's numeric, min and max values
        public string Min { get; set; } = "";
    }
    /// <summary>
    /// class to be passed from the web api to the client when the "GETPARAMETERS" is the request
    /// </summary>
    public class DataParametersApiModel
    {        
        public int numberOfMaxTrees { get; set; }
        public int numberOfMaxGenerations { get; set; }
        public float mutationPropability { get; set; } // %
        public float crossoverPropability { get; set; }
        public int generationGap { get; set; }
        public float correctionfactor { get; set; }
        public string otherParams { get; set; }
    }
    
}
