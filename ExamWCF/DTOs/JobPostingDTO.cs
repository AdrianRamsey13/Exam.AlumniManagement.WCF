using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamWCF.DTOs
{
    public class JobPostingDTO
    {
        public Guid JobID { get; set; }

        public string Title { get; set; }

        public string Company { get; set; }

        public string JobDescription { get; set; }

        public byte EmploymentTypeID { get; set; }

        public byte MinimumExperience { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsActive { get; set; }

        public bool? IsClosed { get; set; }

        public int TotalCandidates { get; set; }

        public IEnumerable<SelectListItem> EmploymentTypes { get; set; }

        public IEnumerable<SelectListItem> Skills { get; set; }

        public IEnumerable<SelectListItem> AttachmentTypes { get; set; }
    }
}