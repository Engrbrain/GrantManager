using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class Programme
    {
        public Programme()
        {
            SelectionCategory = new HashSet<SelectionCategory>();
            TimeStamp = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeCode { get; set; }
        public string ProgrammeDesc { get; set; }
        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationDueDate { get; set; }

        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }

        public int MaximumApplication { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] ProgrammeLogo { get; set; }
        
        public string UserId { get; set; }

      

        public ICollection<ProgApplication> ProgApplication { get; set; }
        public ICollection<Project> Project { get; set; }
        public ICollection<SelectionCategory> SelectionCategory { get; set; }
    }
}