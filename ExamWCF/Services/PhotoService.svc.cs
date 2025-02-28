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

        public PhotoService()
        {
            _dataContext = new AlumniManagementDataContext(connectionStringSettings);
        }

        public IEnumerable<PhotoDTO> GetAllPhotos()
        {
            return (from photo in _dataContext.Photos
                    join album in _dataContext.PhotoAlbums
                    on photo.AlbumID equals album.AlbumID
                    select new PhotoDTO
                    {
                        PhotoID = photo.PhotoID,
                        AlbumID = album.AlbumID,
                        AlbumName = album.AlbumName,
                        PhotoPath = photo.PhotoPath,
                        PhotoFileName = photo.PhotoFileName,
                        IsPhotoAlbumThumbnail = photo.IsPhotoAlbumThumbnail,
                        ModifiedDate = photo.ModifiedDate
                    })
                    .OrderByDescending(p => p.ModifiedDate)
                    .ToList();
        }

        public IEnumerable<PhotoDTO> GetPhotos(int AlbumID)
        {
            return (from photo in _dataContext.Photos
                    join album in _dataContext.PhotoAlbums
                    on photo.AlbumID equals album.AlbumID
                    where album.AlbumID == AlbumID
                    select new PhotoDTO
                    {
                        PhotoID = photo.PhotoID,
                        AlbumID = album.AlbumID,
                        AlbumName = album.AlbumName,
                        PhotoPath = photo.PhotoPath,
                        PhotoFileName = photo.PhotoFileName,
                        IsPhotoAlbumThumbnail = photo.IsPhotoAlbumThumbnail,
                        ModifiedDate = photo.ModifiedDate
                    })
        .OrderByDescending(p => p.ModifiedDate)
        .ToList();

        }

        public PhotoDTO GetPhotoByID(int AlbumID, int id)
        {
            var photo = GetPhotos(AlbumID).FirstOrDefault(p => p.PhotoID == id);
            return photo ?? new PhotoDTO(); // Menghindari null reference
        }

        public void InsertPhoto(PhotoDTO photo, int AlbumID)
        {
            var data = Mapping.Mapper.Map<Photo>(photo);

            // Cek apakah sudah ada foto dalam album
            bool isFirstPhoto = !_dataContext.Photos.Any(p => p.AlbumID == AlbumID);

            data.IsPhotoAlbumThumbnail = isFirstPhoto; // True jika ini adalah foto pertama, false jika tidak
            data.ModifiedDate = DateTime.Now;

            _dataContext.Photos.InsertOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public void DeletePhoto(int id)
        {
            var data = _dataContext.Photos.FirstOrDefault(p => p.PhotoID == id);
            _dataContext.Photos.DeleteOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public void SetThumbnail(int id, int albumID)
        {
            var photos = _dataContext.Photos.Where(p => p.AlbumID == albumID);
            var previousThumbnail = photos.FirstOrDefault(p => p.IsPhotoAlbumThumbnail == true);
            previousThumbnail.IsPhotoAlbumThumbnail = false;
            foreach (var photo in photos)
            {
                photo.IsPhotoAlbumThumbnail = photo.PhotoID == id;
            }
            _dataContext.SubmitChanges();
        }
    }
}
