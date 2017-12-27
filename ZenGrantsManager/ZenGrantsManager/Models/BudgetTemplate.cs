using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class BudgetTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string BudgetItem { get; set; }
        public string BudgetItemDesc { get; set; }
        public double PercentageAllocated { get; set; }

        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        

        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
       

        public BudgetTemplate()
        {
            TimeStamp = DateTime.Now;
        }

      
    }
}