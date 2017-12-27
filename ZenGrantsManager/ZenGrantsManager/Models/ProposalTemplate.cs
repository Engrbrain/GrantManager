using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenGrantsManager.Models
{
   
    public class ProposalTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FieldLabel { get; set; }
        public enumManager.FieldType FieldType { get; set; }
        public string FieldValue { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }


        public ProposalTemplate()
        {
            TimeStamp = DateTime.Now;
        }

        public ICollection<CustomApplicationDetails> CustomApplicationDetails { get; set; }
        public ICollection<Programme> Programme { get; set; }

    }
}