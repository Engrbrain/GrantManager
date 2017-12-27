using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class enumManager
    {
        public enum AttendanceStatus
        {
            Attended,
            NotAttended,
        }

        public enum applicationStatus
        {
            Submitted,
            InternalReview,
            UnderAssessment,
            Approved,
            Rejected

        }

        public enum ActivityPriority
        {
            Critical,
            High,
            Medium,
            Low
        }
        public enum ActivityStatus
        {
            NotStarted,
            Started,
            InProgress,
            Completed,
            Parked
        }

        public enum MeetingMedium
        {
            ConferenceCall,
            Online,
            PhysicalMeeting
        }

        public enum MeetingStatus
        {
            Scheduled,
            Ongoing,
            Done
        }

        public enum RiskStatus
        {
            Critical,
            High,
            Medium,
            Low,
            Mitigated
        }

        public enum KPI
        {
            Five,
            Four,
            Three,
            Two,
            One,
            Risk

        }
        public enum FieldType
        {
            Text,
            RadioButton,
            CheckBox,
            Dropdown,
            FileUpload,
            ImageUpload,
            DatePicker,
            DateRange

        }
        public enum PaymentMethod
        {
            PhysicalInvoice,
            OnlinePayment,
            BankTransfer
        }

        public enum subType
        {
            Basic,
            Standard,
            Enterprise
        }

        public enum country
        {
            Afghanistan,
            Albania,
            Algeria,
            American_Samoa,
            Andorra,
            Angola,
            Anguilla,
            Antigua_Barbuda,
            Argentina,
            Armenia,
            Aruba,
            Australia,
            Austria,
            Azerbaijan,
            Bahamas,
            Bahrain,
            Bangladesh,
            Barbados,
            Belarus,
            Belgium,
            Belize,
            Benin,
            Bermuda,
            Bhutan,
            Bolivia,
            Bosnia,
            Botswana,
            Brazil,
            BritishVirginIsland,
            Brunei,
            Bulgaria,
            BurkinaFaso,
            Burma,
            Burundi,
            Cambodia,
            Cameroon,
            Canada,
            CapeVerde,
            CaymanIslands,
            CentralAfricanRep,
            Chad,
            Chile,
            China,
            Colombia,
            Comoros,
            Congo,
            CookIslands,
            CostaRica,
            Cote_d_Ivoire,
            Croatia,
            Cuba,
            Cyprus,
            CzechRepublic,
            Denmark,
            Djibouti,
            Dominica,
            DominicanRepublic,
            EastTimor,
            Ecuador,
            Egypt,
            ElSalvador,
            Guinea,
            Eritrea,
            Estonia,
            Ethiopia,
            FaroeIslands,
            Fiji,
            Finland,
            France,
            FrenchGuiana,
            FrenchPolynesia,
            Gabon,
            GambiaThe,
            GazaStrip,
            Georgia,
            Germany,
            Ghana,
            Gibraltar,
            Greece,
            Greenland,
            Grenada,
            Guadeloupe,
            Guam,
            Guatemala,
            Guernsey,
            Guinea_Bissau,
            Guyana,
            Haiti,
            Honduras,
            HongKong,
            Hungary,
            Iceland,
            India,
            Indonesia,
            Iran,
            Iraq,
            Ireland,
            IsleofMan,
            Israel,
            Italy,
            Jamaica,
            Japan,
            Jersey,
            Jordan,
            Kazakhstan,
            Kenya,
            Kiribati,
            KoreaNorth,
            KoreaSouth,
            Kuwait,
            Kyrgyzstan,
            Laos,
            Latvia,
            Lebanon,
            Lesotho,
            Liberia,
            Libya,
            Liechtenstein,
            Lithuania,
            Luxembourg,
            Macau,
            Macedonia,
            Madagascar,
            Malawi,
            Malaysia,
            Maldives,
            Mali,
            Malta,
            MarshallIslands,
            Martinique,
            Mauritania,
            Mauritius,
            Mayotte,
            Mexico,
            MicronesiaFed,
            Moldova,
            Monaco,
            Mongolia,
            Montserrat,
            Morocco,
            Mozambique,
            Namibia,
            Nauru,
            Nepal,
            Netherlands,
            NetherlandsAntilles,
            NewCaledonia,
            NewZealand,
            Nicaragua,
            Niger,
            Nigeria,
            MarianaIslands,
            Norway,
            Oman,
            Pakistan,
            Palau,
            Panama,
            PapuaNewGuinea,
            Paraguay,
            Peru,
            Philippines,
            Poland,
            Portugal,
            PuertoRico,
            Qatar,
            Reunion,
            Romania,
            Russia,
            Rwanda,
            Samoa,
            SanMarino,
            SaoTome,
            SaudiArabia,
            Senegal,
            Serbia,
            Seychelles,
            SierraLeone,
            Singapore,
            Slovakia,
            Slovenia,
            SolomonIslands,
            Somalia,
            SouthAfrica,
            Spain,
            SriLanka,
            Sudan,
            Suriname,
            Swaziland,
            Sweden,
            Switzerland,
            Syria,
            Taiwan,
            Tajikistan,
            Tanzania,
            Thailand,
            Togo,
            Tonga,
            TrinidadNTobago,
            Tunisia,
            Turkey,
            Tuvalu,
            Uganda,
            Ukraine,
            UnitedArabEmirates,
            UnitedKingdom,
            UnitedStates,
            Uruguay,
            Uzbekistan,
            Vanuatu,
            Venezuela,
            Vietnam,
            VirginIslands,
            WallisandFutuna,
            WestBank,
            WesternSahara,
            Yemen,
            Zambia,
            Zimbabwe,
            Others

        }

        public enum State
        {
            Abia,
            Adamawa,
            AkwaIbom,
            Anambra,
            Bauchi,
            Bayelsa,
            Benue,
            Borno,
            CrossRiver,
            Delta,
            Ebonyi,
            Edo,
            Ekiti,
            Enugu,
            Gombe,
            Imo,
            Jigawa,
            Kaduna,
            Kano,
            Katsina,
            Kebbi,
            Kogi,
            Kwara,
            Lagos,
            Nasarawa,
            Niger,
            Ogun,
            Ondo,
            Osun,
            Oyo,
            Plateau,
            Rivers,
            Sokoto,
            Taraba,
            Yobe,
            Zamfara,
            FCT,
            Others

        }

        public enum Gender {
            Male,
            Female
        }

        public enum Scope
        {
            GrantGiver,
            GrantBeneficiary,
            Stakeholder,
            ProjectTeam,
            Assessor,
            Accountant,
            Auditor,
            ReportView,
            SystemAdmin,
            SuperAdmin,
            User
        }

    }
}