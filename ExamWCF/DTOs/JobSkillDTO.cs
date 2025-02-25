using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamWCF.DTOs
{
    public class JobSkillDTO
    {
        public Guid JobID { get; set; }

        public byte SkillID { get; set; }
    }
}