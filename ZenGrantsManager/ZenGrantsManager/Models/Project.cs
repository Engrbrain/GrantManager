using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string ProjectReference { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProgrammeDesc { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectDueDate { get; set; }
        public string ProjectContigencyPeriod { get; set; }
        public double AmountAllocated { get; set; }
        public double BalanceAmount { get; set; }
        public string ProjectStatus { get; set; }
        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }

        public int ProgApplicationID { get; set; }

        public virtual ProgApplication ProgApplication { get; set; }

        public int ProgrammeID { get; set; }

        public virtual Programme Programme { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] ProjectLogo { get; set; }


        public Project()
        {
            TimeStamp = DateTime.Now;
        }

        public ICollection<ProjectTeam> ProjectTeams { get; set; }
        public ICollection<ProjectDocument> ProjectDocuments { get; set; }
        public ICollection<ProjectRisk> ProjectRisk { get; set; }
        public ICollection<ProjectActivity> ProjectActivity { get; set; }
        public ICollection<ProjectMeeting> ProjectMeeting { get; set; }
        public ICollection<ProjectComment> ProjectComment { get; set; }
        public ICollection<ProjectBudget> ProjectBudget { get; set; }
        public ICollection<MeetingAttendance> MeetingAttendance { get; set; }
        public ICollection<ProjectTransactionHeader> ProjectTransactionHeader { get; set; }
    }
}