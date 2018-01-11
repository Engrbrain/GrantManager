using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class ZenGrantsManagerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ZenGrantsManagerContext() : base("name=ZenGrantsManagerContext")
        {
        }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.Organization> Organizations { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ActivityDocument> ActivityDocuments { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectActivity> ProjectActivities { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ActivityRisk> ActivityRisks { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ApplicationDocument> ApplicationDocument { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProgApplication> ProgApplications { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.Assessor> Assessors { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.BudgetTemplate> BudgetTemplates { get; set; }
        
        public System.Data.Entity.DbSet<ZenGrantsManager.Models.Programme> Programmes { get; set; }
        

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.FocusArea> FocusAreas { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.MeetingAttendance> MeetingAttendances { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectMeeting> ProjectMeetings { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectTeam> ProjectTeams { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectActivityComment> ProjectActivityComments { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectBudget> ProjectBudgets { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectComment> ProjectComments { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectDocument> ProjectDocuments { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectRisk> ProjectRisks { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectTemplate> ProjectTemplates { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectTransactionHeader> ProjectTransactionHeaders { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.ProjectTransactionLine> ProjectTransactionLines { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.Renewal> Renewals { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.Subscription> Subscriptions { get; set; }
        

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.SelectionQuestion> SelectionQuestions { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.SelectionCategory> SelectionCategories { get; set; }

        public System.Data.Entity.DbSet<ZenGrantsManager.Models.SelectionAnswer> SelectionAnswers { get; set; }
    }
}
