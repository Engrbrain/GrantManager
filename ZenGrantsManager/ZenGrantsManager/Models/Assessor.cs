using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class Assessor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string AssessorCode { get; set; }
        public string AssessorName { get; set; }
        public string AssessorEmail { get; set; }
        public string AssessorPassword { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }

        public Assessor()
        {
            TimeStamp = DateTime.Now;
        }

        public ICollection<ProgApplication> ProgApplication { get; set; }
        public ICollection<SelectionAnswer> SelectionAnswer { get; set; }
    }
}