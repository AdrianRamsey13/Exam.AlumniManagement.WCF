using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamWCF.DTOs
{
    public class EventDTO
    {
        public int EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventImagePath { get; set; }
        public string EventImageName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
        public DateTime ModifiedDate { get; set; }
    
        public string DisplayDate { get; set; }
    }
}