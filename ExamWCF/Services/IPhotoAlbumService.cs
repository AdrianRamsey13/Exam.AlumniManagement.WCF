using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPhotoAlbumService" in both code and config file together.
    [ServiceContract]
    public interface IPhotoAlbumService
    {
        [OperationContract]
        IEnumerable<PhotoAlbumDTO> GetPhotoAlbums();

        [OperationContract]
        PhotoAlbumDTO GetPhotoAlbumById(int photoAlbumId);

        [OperationContract]
        void UpsertPhotoAlbum(PhotoAlbumDTO photoAlbum);

        [OperationContract]
        void DeletePhotoAlbum(int photoAlbumId);
    }
}
