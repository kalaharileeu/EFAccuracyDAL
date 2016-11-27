using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuracyDAL.Models
{
    [Table("Testrun")]
    public partial class Testrun
    {
        //EF interprets a property named that included "ID" as the primary key
        [Key]
        public int testrunID { get; set; }
        //From csv data
        [StringLength(12)]
        public string HardwareType { get; set; }
        //from csv data
        [StringLength(12)]
        public string SerialNumber { get; set; }

        //from csv data
        [StringLength(50)]
        public string TestName { get; set; }

        [StringLength(8)]
        public string FirmwareRef { get; set; }

        [StringLength(8)]
        public string ParameterRef { get; set; }

       // [Timestamp]
      //  public byte[] Timestamp { get; set; }
        //Adding  the navigation property to the Inventory, help with Join without
        //writing complx sql
        public virtual ICollection<Testpoint> Testpoints { get; set; } = new HashSet<Testpoint>();
    }
}
