using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;
using ExamWCF.Entities;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FacultyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FacultyService.svc or FacultyService.svc.cs at the Solution Explorer and start debugging.
    public class FacultyService : IFacultyService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }

        public FacultyService()
        {
            _dataContext = new AlumniManagementDataContext(ConnectionStringSettings);
        }

        public IEnumerable<FacultyDTO> GetFaculties()
        {
            var data = _dataContext.Faculties
                .ToList()
                .OrderByDescending(r => r.ModifiedDate)
                .Select(r => Mapping.Mapper.Map<FacultyDTO>(r));
            return data;
        }

        public FacultyDTO GetFacultyByID(int facultyID)
        {
            var room = _dataContext.Faculties.FirstOrDefault(r => r.FacultyID == facultyID);
            var result = Mapping.Mapper.Map<FacultyDTO>(room);
            return result;
        }

        public void InsertFaculty(FacultyDTO faculty)
        {
            var faculties = Mapping.Mapper.Map<Faculty>(faculty);
            faculties.ModifiedDate = DateTime.Now;
            _dataContext.Faculties.InsertOnSubmit(faculties);
            _dataContext.SubmitChanges();
        }

        public void UpdateFaculty(FacultyDTO faculty)
        {
            var existingFaculty = _dataContext.Faculties.FirstOrDefault(r => r.FacultyID == faculty.FacultyID);
            Mapping.Mapper.Map(faculty, existingFaculty);
            existingFaculty.ModifiedDate = DateTime.Now;
            _dataContext.SubmitChanges();
        }

        public void DeleteFaculty(int facultyID)
        {
            var result = _dataContext.Faculties.First(r => r.FacultyID == facultyID);
            _dataContext.Faculties.DeleteOnSubmit(result);
            _dataContext.SubmitChanges();
        }
    }
}
