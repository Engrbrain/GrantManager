using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
   
    public class ActivityRisk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string RiskTitle { get; set; }
        public string RiskDescription { get; set; }
        public string RiskMitigation { get; set; }
        public enumManager.RiskStatus RiskStatus { get; set; }
        public byte[] RiskDocument { get; set; }
        public int ProjectActivityID { get; set; }
        public virtual ProjectActivity ProjectActivity { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public string UserId { get; set; }

      
        public ActivityRisk()
        {
            TimeStamp = DateTime.Now;
        }

       
    }
}