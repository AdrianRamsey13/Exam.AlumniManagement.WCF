using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWCF.Entities;

namespace ExamWCF.DTOs
{
    public class JobHistoryDTO
    {
        public int JobHistoryID { get; set; }

        public System.Nullable<int> AlumniID { get; set; }

        public string JobTitle { get; set; }

        public string Company { get; set; }

        public System.Nullable<System.DateTime> StartDate { get; set; }

        public System.Nullable<System.DateTime> EndDate { get; set; }

        public string Description { get; set; }

        public System.DateTime ModifiedDate { get; set; }

        public IEnumerable<SelectListItem> Alumnis { get; set; }
    }
}