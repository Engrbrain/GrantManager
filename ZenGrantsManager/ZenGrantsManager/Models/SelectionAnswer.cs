using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class SelectionAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int SelectionQuestionID { get; set; }
        public virtual SelectionQuestion SelectionQuestion { get; set; }
        public int AssessorID { get; set; }
        public virtual Assessor Assessor { get; set; }
        public int ProgApplicationID { get; set; }
        public virtual ProgApplication ProgApplication { get; set; }
        public double AssignedScore { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public SelectionAnswer()
        {
            TimeStamp = DateTime.Now;
        }

       
    }
}