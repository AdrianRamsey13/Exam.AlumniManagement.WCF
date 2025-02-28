using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPhotoService" in both code and config file together.
    [ServiceContract]
    public interface IPhotoService
    {
        [OperationContract]
        IEnumerable<PhotoDTO> GetAllPhotos();

        [OperationContract]
        IEnumerable<PhotoDTO> GetPhotos(int AlbumID);

        [OperationContract]
        PhotoDTO GetPhotoByID(int AlbumID,int id);

        [OperationContract]
        void InsertPhoto(PhotoDTO photo, int AlbumID);

        [OperationContract]
        void DeletePhoto(int id);

        [OperationContract]
        void SetThumbnail(int id, int albumID);
    }
}
