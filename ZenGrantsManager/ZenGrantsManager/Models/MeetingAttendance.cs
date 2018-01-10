using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
  

   
    public class MeetingAttendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public enumManager.AttendanceStatus AttendanceStatus { get; set; }
        public int ProjectMeetingID { get; set; }
        public virtual ProjectMeeting ProjectMeeting { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public string UserId { get; set; }

      
        public MeetingAttendance()
        {
            TimeStamp = DateTime.Now;
        }

        
    }
}