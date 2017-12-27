using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
   

    public class Renewal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }

        public int SubscriptionID { get; set; }

        public virtual Subscription Subscription { get; set; }

        public string InvoiceNumber { get; set; }

        public string PaymentReference { get; set; }
        public int Status { get; set; }
        public enumManager.PaymentMethod PaymentMethod { get; set; }
        public string Narration { get; set; }

        public DateTime PostingDate { get; set; }
        
        public DateTime ExpiryDate { get; set; }
        public double RenewalAmount { get; set; }
        public bool isActive { get; set; }
        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

       

        public Renewal()
        {
            TimeStamp = DateTime.Now;
        }
    }
}