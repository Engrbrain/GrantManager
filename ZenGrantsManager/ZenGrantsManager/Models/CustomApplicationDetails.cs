using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class CustomApplicationDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProposalTemplateID { get; set; }
        public virtual ProposalTemplate ProposalTemplate { get; set; }
        public string FieldUserInput { get; set; }
        public int ProgApplicationID { get; set; }
        public virtual ProgApplication ProgApplication { get; set; }

        public int ProgrammeID { get; set; }
        public virtual Programme Programme { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
      

        public CustomApplicationDetails()
        {
            TimeStamp = DateTime.Now;
        }

       
    }
}