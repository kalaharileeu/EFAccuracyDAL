using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuracyDAL.Models
{

    [Table("Testpoint")]
    public partial class Testpoint
    {
        [Key]
        public int ID { get; set; }
       // [Required]
        public int testrunID { get; set; }
        [Required]
        public float Powerfactorpcu { get; set; }
        [Required]
        public float Wdcpcu { get; set; }
        [Required]
        public float Wacpcu { get; set; }
        [Required]
        public float Wacimagpcu { get; set; }
        [Required]
        public float Idcpcu { get; set; }
        [Required]
        public float Vdcpcu { get; set; }
        [Required]
        public float Iacimagpcu { get; set; }
        [Required]
        public float IacApparantpcu_calc { get; set; }
        [Required]
        public float Vacpcu { get; set; }
        [Required]
        public float PFcontrolAcc_pcu { get; set; }
        [Required]
        public float ACvarpowermeter { get; set; }
        [Required]
        public float ACvapowermeter { get; set; }
        [Required]
        public float Idcpowermeter { get; set; }
        [Required]
        public float Wacpowermeter { get; set; }
        [Required]
        public float Vdcpowermeter { get; set; }
        [Required]
        public float Iacpowermeter { get; set; }
        [Required]
        public float Vacpowermeter { get; set; }
        [Required]
        public float Phaseconfigured { get; set; }
        [Required]
        public float Temperature { get; set; }
        [Required]
        public float NOFFpcu { get; set; }
        [Required]
        public float Wdcpowermeter { get; set; }
        [Required]
        public float Powerratioconfigured { get; set; }
        [Required]
        public float Vdcconfigured { get; set; }

        ////annotation to indicate which properties serve as the 
        ////backing fields for the two navigation properties
        [ForeignKey("testrunID")]
        public virtual Testrun Testrun { get; set; }
    }
}
