using System;
using System.Collections.Generic;
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
        public List<int> SelectedSkills { get; set; }
        public List<int> SelectedAttachmentTypes { get; set; }
        public List<int> SelectedJobAttachments { get; set; }
        public List<int> SelectedCandidates { get; set; }
        public IEnumerable<SelectListItem> EmploymentTypes { get; set; }
        public string SkillDisplay { get; set; }
        public string AttachmentTypeDisplay { get; set; }
        public string JobAttachmentDisplay { get; set; }
        public string EmploymentTypeNames { get; set; }
        public string IsActiveDisplay => IsActive ? "Active" : "Inactive";
        public string IsClosedDisplay => IsActive ? "Closed" : "Not Yet";
        public int Candidates { get; set; }
        public int TotalCandidates { get; set; }
    }
}