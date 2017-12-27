using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class ProjectTransactionLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProjectTransactionHeaderID { get; set; }
        public virtual ProjectTransactionHeader ProjectTransactionHeader { get; set; }
        public string TransactionRef { get; set; }
        public int LineNumber { get; set; }
        public string Narration { get; set; }
        public double LineAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public ProjectTransactionLine()
        {
            TimeStamp = DateTime.Now;
        }

        
    }
}