using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string OrgName { get; set; }
        public string OrgAddress { get; set; }
        public enumManager.State OrgState { get; set; }
        public enumManager.country OrgCountry { get; set; }
        public string OrgPhone { get; set; }
        public string OrgEmail { get; set; }
        public string OrgWebsite { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] OrgLogo { get; set; }
        public string ImageType { get; set; }
        public string UserId { get; set; }

        public Organization()
        {
            TimeStamp = DateTime.Now;
        }

        public ICollection<Subscription> Subscription { get; set; }
        public ICollection<Renewal> Renewal { get; set; }
        public ICollection<FocusArea> FocusArea { get; set; }
        public ICollection<ProposalTemplate> ProposalTemplate { get; set; }
        public ICollection<Programme> Programme { get; set; }
        public ICollection<ProgApplication> ProgApplication { get; set; }
        public ICollection<CustomApplicationDetails> CustomApplicationDetails { get; set; }
        public ICollection<ApplicationDocument> ApplicationDocument { get; set; }
        public ICollection<Assessor> Assessor { get; set; }
        public ICollection<SelectionCategory> SelectionCategory { get; set; }
        public ICollection<Project> Project { get; set; }
        public ICollection<ProjectDocument> ProjectDocument { get; set; }
        public ICollection<ProjectTemplate> ProjectTemplate { get; set; }
        public ICollection<BudgetTemplate> BudgetTemplate { get; set; }
        public ICollection<ActivityRisk> ActivityRisk { get; set; }
        public ICollection<ActivityDocument> ActivityDocument { get; set; }
        public ICollection<MeetingAttendance> MeetingAttendance { get; set; }
        public ICollection<ProjectActivity> ProjectActivity { get; set; }
        public ICollection<ProjectActivityComment> ProjectActivityComment { get; set; }
        public ICollection<ProjectComment> ProjectComment { get; set; }
        public ICollection<ProjectBudget> ProjectBudget { get; set; }
        public ICollection<ProjectMeeting> ProjectMeeting { get; set; }
        public ICollection<ProjectRisk> ProjectRisk { get; set; }
        public ICollection<ProjectTeam> ProjectTeam { get; set; }
        public ICollection<ProjectTransactionHeader> ProjectTransactionHeader { get; set; }
        public ICollection<ProjectTransactionLine> ProjectTransactionLine { get; set; }
        public ICollection<SelectionAnswer> SelectionAnswer { get; set; }
        public ICollection<SelectionQuestion> SelectionQuestion { get; set; }


    }
}