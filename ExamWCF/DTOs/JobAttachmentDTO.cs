using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamWCF.DTOs
{
    public class JobAttachmentDTO
    {
        public int AttachmentID { get; set; }

        public Guid JobID { get; set; }

        public int AlumniID { get; set; }

        public string AlumniName { get; set; }

        public byte AttachmentTypeID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public List<int> SelectedAttachmentTypes { get; set; }
        public string AttachmentTypeDisplay { get; set; }
    }
}