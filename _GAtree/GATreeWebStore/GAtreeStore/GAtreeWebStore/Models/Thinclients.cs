using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/// <summary>
///  The data stored for the thin clients.  A thin client can patricipate in more than one run
/// </summary>
namespace GAtreeWebStore.Models
{
    public class ThinClient
    {
        [Key]
        public int ThinClient_id { get; set; }
        [Required]
        public string ThinClient_guid { get; set; } = Guid.NewGuid().ToString();
        public bool ThinClient_isRunActive { get; set; } = false; // If The run which the client has connect is still active
        public bool thinClient_isActive { get; set; } = true;     // if the thinclient is active
        [ForeignKey("Test_data_run_id")]
        public int Test_data_run_id { get; set; } //Id of the data
        public virtual TestDataRun testdatarun { get; set; } // For which testdataRun has been registered
        // TODO: Does it have a job??!? Should be revised !!! maybe there'san error!!!
    }

}
