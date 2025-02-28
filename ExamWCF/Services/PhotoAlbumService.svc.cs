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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PhotoAlbumService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PhotoAlbumService.svc or PhotoAlbumService.svc.cs at the Solution Explorer and start debugging.
    public class PhotoAlbumService : IPhotoAlbumService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public PhotoAlbumService()
        {
            _dataContext = new AlumniManagementDataContext(connectionStringSettings);
        }

        public IEnumerable<PhotoAlbumDTO> GetPhotoAlbums()
        {
            var query = from p in _dataContext.PhotoAlbums
                        select new PhotoAlbumDTO
                        {
                            AlbumID = p.AlbumID,
                            AlbumName = p.AlbumName,
                            ModifiedDate = p.ModifiedDate
                        };
            return query.ToList().OrderByDescending(p => p.ModifiedDate);
        }

        public PhotoAlbumDTO GetPhotoAlbumById(int photoAlbumId)
        {
            var album = GetPhotoAlbums().FirstOrDefault(p => p.AlbumID == photoAlbumId);
            return album;
        }

        public void UpsertPhotoAlbum(PhotoAlbumDTO photoAlbum)
        {
            try
            {
                using (var connection = new SqlConnection(_dataContext.Connection.ConnectionString))
                {
                    using (var command = new SqlCommand("dbo.UpsertPhotoAlbum", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Jika AlbumID adalah 0, set NULL agar sesuai dengan Stored Procedure
                        command.Parameters.Add(new SqlParameter("@AlbumID", SqlDbType.Int)
                        {
                            Value = photoAlbum.AlbumID > 0 ? (object)photoAlbum.AlbumID : DBNull.Value
                        });

                        command.Parameters.Add(new SqlParameter("@AlbumName", SqlDbType.VarChar, 50) { Value = photoAlbum.AlbumName });
                        command.Parameters.Add(new SqlParameter("@ModifiedDate", SqlDbType.DateTime) { Value = DateTime.Now });

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpsertPhotoAlbum: " + ex.Message);
            }
        }


        public void DeletePhotoAlbum(int photoAlbumId)
        {
            var result = _dataContext.PhotoAlbums.FirstOrDefault(p => p.AlbumID == photoAlbumId);
            _dataContext.PhotoAlbums.DeleteOnSubmit(result);
            _dataContext.SubmitChanges();
        }
    }
}
