using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/// <summary>
///  A simple Error logging - saved to db
/// </summary>

namespace GAtreeWebStore.Models
{
    public class Logging
    {
        [Key]
        public int Error_id { get; set; } //Id of the data
        public string ErrorMessage { get; set; } 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data_date { get; set; } = DateTime.Now; // Date it was saved
    }
}
