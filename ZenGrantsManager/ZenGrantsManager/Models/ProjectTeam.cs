using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
   
    public class ProjectTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string TeamMemberReference { get; set; }
        public string Fullname { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public enumManager.State State { get; set; }
        public enumManager.country Country { get; set; }

        public enumManager.KPI KPI { get; set; }
       
        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public byte[] TeamMemberPhoto { get; set; }

        public string UserId { get; set; }

       

        public ProjectTeam()
        {
            TimeStamp = DateTime.Now;
        }

        public ICollection<ProjectActivity> ProjectActivity { get; set; }
        public ICollection<MeetingAttendance> MeetingAttendance { get; set; }

    }
}