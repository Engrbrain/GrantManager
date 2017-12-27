using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
   
    public class SelectionQuestion
    {
        public SelectionQuestion()
        {
            this.SelectionCategory = new HashSet<SelectionCategory>();
            TimeStamp = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Question { get; set; }
        public int QuestionWeight { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public string UserId { get; set; }

       
        public ICollection<SelectionAnswer> SelectionAnswer { get; set; }
        public virtual ICollection<SelectionCategory> SelectionCategory { get; set; }

    }
}