using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
  
    public class SelectionCategory
    {
        public SelectionCategory()
        {
            SelectionQuestion = new HashSet<SelectionQuestion>();
            Programme = new HashSet<Programme>();
            TimeStamp = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int CategoryWeight { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
       

        public virtual ICollection<SelectionQuestion> SelectionQuestion { get; set; }
        public virtual ICollection<Programme> Programme { get; set; }
    }
}