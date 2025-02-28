using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;
using ExamWCF.Entities;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PhotoService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PhotoService.svc or PhotoService.svc.cs at the Solution Explorer and start debugging.
    public class PhotoService : IPhotoService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public IEnumerable<PhotoDTO> GetPhotos()
        {
            var query = from p in _dataContext.Photos
                        select new PhotoDTO
                        {
                            PhotoID = p.PhotoID,
                            AlbumID = p.AlbumID,
                            PhotoPath = p.PhotoPath,
                            PhotoFileName = p.PhotoFileName,
                            IsPhotoAlbumThumbnail = p.IsPhotoAlbumThumbnail,
                            ModifiedDate = p.ModifiedDate,
                        };
            return query.ToList().OrderByDescending(p => p.ModifiedDate);
        }

        public PhotoDTO GetPhotoByID(int id)
        {
            var photo = GetPhotos().FirstOrDefault(p => p.PhotoID == id);
            return photo;
        }

        public void InsertPhoto(PhotoDTO photo)
        {
            var data = Mapping.Mapper.Map<Photo>(photo);
            data.ModifiedDate = DateTime.Now;
            _dataContext.Photos.InsertOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public void UpdatePhoto(PhotoDTO photo)
        {
            var existingPhoto = _dataContext.Photos
                .FirstOrDefault(p => p.PhotoID == photo.PhotoID);
            var data = Mapping.Mapper.Map(photo, existingPhoto);
            data.ModifiedDate = DateTime.Now;
            _dataContext.SubmitChanges();
        }

        public void DeletePhoto(int id)
        {
            var data = _dataContext.Photos.FirstOrDefault(p => p.PhotoID == id);
            _dataContext.Photos.DeleteOnSubmit(data);
            _dataContext.SubmitChanges();
        }
    }
}
