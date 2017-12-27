using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class ProjectActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityDocumentID { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Milestone { get; set; }
        public enumManager.ActivityPriority Priority { get; set; }
        public int? DependencyID { get; set; }
        public ProjectActivity Dependency { get; set; }
        public enumManager.ActivityStatus ActivityStatus { get; set; }
        public int? ProjectMeetingID { get; set; }
        public virtual ProjectMeeting ProjectMeeting { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public int? ProjectTeamID { get; set; }
        public virtual ProjectTeam ProjectTeam { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public ProjectActivity()
        {
            TimeStamp = DateTime.Now;
        }

       
        public ICollection<ProjectActivityComment> ProjectActivityComment { get; set; }
        public ICollection<ActivityDocument> ActivityDocument { get; set; }
        public ICollection<ActivityRisk> ActivityRisk { get; set; }

    }
}