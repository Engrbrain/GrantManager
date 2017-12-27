using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class ProjectTransactionHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProjectBudgetID { get; set; }
        public virtual ProjectBudget ProjectBudget { get; set; }
        public string TransactionRef { get; set; }
        public string ShortText { get; set; }
        public int FiscalYear { get; set; }
        public int Period { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime EntryDate { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public string UserId { get; set; }

       
        public ProjectTransactionHeader()
        {
            TimeStamp = DateTime.Now;
        }
        public ICollection<ProjectTransactionLine> ProjectTransactionLine { get; set; }

    }
}