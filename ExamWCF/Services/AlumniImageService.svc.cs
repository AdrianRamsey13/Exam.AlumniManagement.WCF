using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ExamWCF.DTOs;
using ExamWCF.Entities;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlumniImageService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AlumniImageService.svc or AlumniImageService.svc.cs at the Solution Explorer and start debugging.
    public class AlumniImageService : IAlumniImageService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }

        public AlumniImageService()
        {
            _dataContext = new AlumniManagementDataContext(ConnectionStringSettings);
        }

        public IEnumerable<AlumniImageDTO> GetAllImages(int alumniID)
        {
            var data = _dataContext.AlumniImages.Where(a => a.AlumniID == alumniID)
                .ToList()
                .OrderByDescending(a => a.UploadDate)
                .Select(a => Mapping.Mapper.Map<AlumniImageDTO>(a));
            return data;
        }

        public AlumniImageDTO GetImageByID(int imageID, int alumniID)
        {
            var data = _dataContext.AlumniImages
                .Where(img => img.ImageID == imageID)
                .Select(img => Mapping.Mapper.Map<AlumniImageDTO>(img))
                .FirstOrDefault();
            return data;
        }

        public async Task DeleteImageByIDAsync(int imageID, int alumniID)
        {
            var image = _dataContext.AlumniImages
                .FirstOrDefault(img => img.ImageID == imageID && img.AlumniID == alumniID);
            if (image != null)
            {
                _dataContext.AlumniImages.DeleteOnSubmit(image);
                await Task.Run(() => _dataContext.SubmitChanges());
            }
        }

        public async Task AddImageAsync(IEnumerable<AlumniImageDTO> alumniImages)
        {
            var imageEntities = alumniImages.Select(img => new AlumniImage
            {
                AlumniID = img.AlumniID,
                ImagePath = img.ImagePath,
                FileName = img.FileName,
                UploadDate = img.UploadDate
            }).ToList();

            _dataContext.AlumniImages.InsertAllOnSubmit(imageEntities);
            await Task.Run(() => _dataContext.SubmitChanges());
        }
    }
}
