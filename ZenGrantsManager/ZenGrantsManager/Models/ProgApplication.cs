using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
   
    public class ProgApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ApplicantReference { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantPhoneNumber { get; set; }
        public string ApplicantEmailAddress { get; set; }
        public string ApplicationSummary { get; set; }
        public byte[] Proposal { get; set; }
        public byte[] ApplicantPhoto { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Applicationscore { get; set; }
        public int ProgrammeID { get; set; }
        public virtual Programme Programme { get; set; }

        public int? AssessorID { get; set; }
        public virtual Assessor Assessor { get; set; }
        public enumManager.applicationStatus applicationStatus { get; set; }



        public ProgApplication()
        {
            TimeStamp = DateTime.Now;
        }

        public ICollection<CustomApplicationDetails> CustomApplicationDetails { get; set; }
        public ICollection<ApplicationDocument> ApplicationDocuments { get; set; }
        public ICollection<SelectionAnswer> SelectionAnswer { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}