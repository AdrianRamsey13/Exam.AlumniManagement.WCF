using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamWCF.DTOs
{
    public class SkillsDTO
    {
        public byte SkillID { get; set; }

        public string Name { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}