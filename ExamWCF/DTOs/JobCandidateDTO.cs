using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamWCF.DTOs
{
    public class JobCandidateDTO
    {
        public int AlumniID { get; set; }

        public Guid JobID { get; set; }

        public DateTime ApplyDate { get; set; }

        public string FullName { get; set; }
        public List<JobAttachmentDTO> JobAttachments { get; set; }
    }
}