using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
   
    public class ProjectMeeting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingDetails { get; set; }
        public string MeetingPurpose { get; set; }
        public enumManager.MeetingMedium MeetingMedium { get; set; }
        public string DiscussionSummary { get; set; }
        public byte[] MeetingMinutes { get; set; }
        public enumManager.MeetingStatus MeetingStatus { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

      
        public ProjectMeeting()
        {
            TimeStamp = DateTime.Now;
        }

        public ICollection<MeetingAttendance> MeetingAttendance { get; set; }
        public ICollection<ProjectActivity> ProjectActivity { get; set; }
    }
}