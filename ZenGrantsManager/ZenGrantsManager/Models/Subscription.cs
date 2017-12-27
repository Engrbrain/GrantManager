using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public enumManager.subType subType { get; set; }
        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime NextExpiryDate { get; set; }
        public DateTime LastRenewalDate { get; set; }
        public bool isActive { get; set; }
        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

       
        public Subscription()
        {
            TimeStamp = DateTime.Now;
        }
        public ICollection<Renewal> Renewal { get; set; }
    }
}