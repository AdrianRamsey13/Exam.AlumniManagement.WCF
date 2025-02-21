using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWCF.Entities;

namespace ExamWCF.DTOs
{
    public class MajorDTO
    {
        public int MajorID { get; set; }

        public string MajorName { get; set; }

        public System.Nullable<int> FacultyID { get; set; }

        public string Description { get; set; }

        public System.DateTime ModifiedDate { get; set; }

        public IEnumerable<SelectListItem> Faculties { get; set; }

        public string FacultyNames { get; set; }
    }
}