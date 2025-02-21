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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MajorService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MajorService.svc or MajorService.svc.cs at the Solution Explorer and start debugging.
    public class MajorService : IMajorService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }

        public MajorService()
        {
            _dataContext = new AlumniManagementDataContext(ConnectionStringSettings);
        }


        public IEnumerable<MajorDTO> GetMajors()
        {
            var data = _dataContext.Majors
                .ToList()
                .OrderByDescending(m => m.ModifiedDate)
                .Select(m => new MajorDTO
                {
                    MajorID = m.MajorID,
                    MajorName = m.MajorName,
                    FacultyID = m.FacultyID,
                    FacultyNames = _dataContext.Faculties
                    .Where(f => f.FacultyID == m.FacultyID)
                    .Select(f => f.FacultyName)
                    .FirstOrDefault(),
                    Description = m.Description,
                    ModifiedDate = m.ModifiedDate
                });
            return data;
        }

        public MajorDTO GetMajorByID(int majorID)
        {
            var cabinet = _dataContext.Majors.FirstOrDefault(r => r.MajorID == majorID);
            var result = Mapping.Mapper.Map<MajorDTO>(cabinet);
            return result;
        }

        public IEnumerable<MajorDTO> GetMajorsByFacultyID(int facultyID)
        {
            var query = from f in _dataContext.Faculties
                        join m in _dataContext.Majors
                        on f.FacultyID equals m.FacultyID
                        where f.FacultyID == facultyID
                        select new MajorDTO
                        {
                            MajorID = m.MajorID,
                            MajorName = m.MajorName,
                            FacultyID = f.FacultyID,
                            FacultyNames = f.FacultyName
                        };
            var result = query.ToList();
            return result;
        }

        public void InsertMajor(MajorDTO major)
        {
            var data = Mapping.Mapper.Map<Major>(major);
            data.ModifiedDate = DateTime.Now;
            _dataContext.Majors.InsertOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public void UpdateMajor(MajorDTO major)
        {
            var existingMajor = _dataContext.Majors
                .FirstOrDefault(r => r.MajorID == major.MajorID);
            Mapping.Mapper.Map(major, existingMajor);
            existingMajor.ModifiedDate = DateTime.Now;
            _dataContext.SubmitChanges();
        }

        public void DeleteMajor(int majorID)
        {
            var result = _dataContext.Majors.First(r => r.MajorID == majorID);
            _dataContext.Majors.DeleteOnSubmit(result);
            _dataContext.SubmitChanges();
        }

        public int GetFacultyIDByName(string facultyName)
        {
            var checkData = _dataContext.Faculties.FirstOrDefault(st => st.FacultyName == facultyName);
            return checkData.FacultyID;
        }

        public int GetMajorIDByName(string majorName)
        {
            var checkData = _dataContext.Majors.FirstOrDefault(st => st.MajorName == majorName);
            return checkData.MajorID;
        }
    }
}
