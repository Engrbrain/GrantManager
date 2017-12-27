using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
  
    public class ProjectRisk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string RiskTitle { get; set; }
        public string RiskDescription { get; set; }
        public string RiskMitigation { get; set; }
        public enumManager.RiskStatus RiskStatus { get; set; }
        public byte[] RiskDocument { get; set; }

        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }

        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        
        public string UserId { get; set; }

        
        public ProjectRisk()
        {
            TimeStamp = DateTime.Now;
        }

       
    }
}