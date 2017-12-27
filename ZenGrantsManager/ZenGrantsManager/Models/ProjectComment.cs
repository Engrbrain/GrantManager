using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
  
    public class ProjectComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CommentTitle { get; set; }
        public string CommentDescription { get; set; }
        public string TagUser { get; set; }
        public byte[] CommentAttachment { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

     
        public ProjectComment()
        {
            TimeStamp = DateTime.Now;
        }

       
    }
}