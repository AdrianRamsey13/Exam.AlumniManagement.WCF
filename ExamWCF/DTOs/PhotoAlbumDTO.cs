using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamWCF.DTOs
{
    public class PhotoAlbumDTO
    {
        public int AlbumID { get; set; }
        public string AlbumName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}