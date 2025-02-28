using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWCF.Entities;

namespace ExamWCF.DTOs
{
    public class AlumniDTO
    {
        public int AlumniID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string Address { get; set; }

        public System.Nullable<int> DistrictID { get; set; }

        public System.Nullable<System.DateTime> DateOfBirth { get; set; }

        public System.Nullable<int> GraduationYear { get; set; }

        public string Degree { get; set; }

        public System.Nullable<int> MajorID { get; set; }

        public string LinkedInProfile { get; set; }

        public System.DateTime ModifiedDate { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        public IEnumerable<SelectListItem> Districts { get; set; }

        public IEnumerable<SelectListItem> Faculties { get; set; }

        public IEnumerable<SelectListItem> Majors { get; set; }

        public string StateNames { get; set; }

        public string DistrictNames { get; set; }

        public string FacultyNames { get; set; }

        public string MajorNames { get; set; }

        public string FullNames { get; set; }

        public string FullAddresses { get; set; }
        public string FacultyMajorNames { get; set; }
        public int FacultyID { get; set; }
        public int StateID { get; set; }

        public List<int> SelectedHobbies { get; set; }

        public string HobbyDisplay { get; set; }

        public string PhotoName { get; set; }

        public string PhotoPath { get; set; }

        public System.Nullable<System.DateTime> ApplyDate { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }

    }

    public class AttachmentDTO
    {
        public string FileName { get; set; }
        public string FilePath
        {
            get; set;
        }
    }
}   