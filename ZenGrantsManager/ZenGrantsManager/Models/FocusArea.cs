using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
   

    public class FocusArea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public string FocusAreaName { get; set; }
        public string FocusAreaDesc { get; set; }

        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
        public DateTime TimeStamp { get; set; }

        
        public string UserId { get; set; }

      

        public FocusArea()
        {
            TimeStamp = DateTime.Now;
        }
    }
}