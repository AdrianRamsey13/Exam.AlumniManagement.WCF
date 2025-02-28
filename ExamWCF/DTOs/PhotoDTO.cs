using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamWCF.DTOs
{
    public class PhotoDTO
    {
        public int PhotoID { get; set; }
        public int AlbumID { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoFileName { get; set; }
        public bool? IsPhotoAlbumThumbnail { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}