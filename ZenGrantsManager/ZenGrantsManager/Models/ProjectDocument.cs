using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class ProjectDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public string LocalFilePath { get; set; }
        public string FileName { get; set; }

        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

      
        public ProjectDocument()
        {
            TimeStamp = DateTime.Now;
        }

       
    }
}