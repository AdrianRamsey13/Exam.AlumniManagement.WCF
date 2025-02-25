using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamWCF.DTOs
{
    public class JobAttachmentTypeDTO
    {
        public Guid JobID { get; set; }

        public byte AttachmentTypeID { get; set; }
    }
}