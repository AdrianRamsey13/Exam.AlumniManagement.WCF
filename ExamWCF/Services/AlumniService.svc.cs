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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlumniService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AlumniService.svc or AlumniService.svc.cs at the Solution Explorer and start debugging.
    public class AlumniService : IAlumniService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }

        public AlumniService()
        {
            _dataContext = new AlumniManagementDataContext(ConnectionStringSettings);
        }

        public IEnumerable<AlumniDTO> GetAlumnis()
        {            
            var query = from a in _dataContext.Alumnis
                        join d in _dataContext.Districts on a.DistrictID equals d.DistrictID
                        join s in _dataContext.States on d.StateID equals s.StateID
                        join m in _dataContext.Majors on a.MajorID equals m.MajorID
                        join f in _dataContext.Faculties on m.FacultyID equals f.FacultyID
                        join h in _dataContext.AlumniHobbies on a.AlumniID equals h.AlumniID into hobbies
                        select new AlumniDTO
                        {
                            AlumniID = a.AlumniID,
                            FirstName = a.FirstName,
                            MiddleName = a.MiddleName,
                            LastName = a.LastName,
                            Gender = a.Gender,
                            Email = a.Email,
                            MobileNumber = a.MobileNumber,
                            Address = a.Address,
                            StateID = s.StateID,
                            StateNames = s.StateName,
                            DistrictID = a.DistrictID,
                            DistrictNames = d.DistrictName,
                            DateOfBirth = (DateTime)a.DateOfBirth,
                            GraduationYear = a.GraduationYear,
                            Degree = a.Degree,
                            FacultyID = f.FacultyID,
                            FacultyNames = f.FacultyName,
                            MajorID = a.MajorID,
                            MajorNames = m.MajorName,
                            LinkedInProfile = a.LinkedInProfile,
                            ModifiedDate = (DateTime)a.ModifiedDate,
                            PhotoName = a.PhotoName,
                            PhotoPath = a.PhotoPath,
                            FullNames = a.FirstName + " " + (a.MiddleName ?? "") + " " + a.LastName,
                            FullAddresses = a.Address + ", " + d.DistrictName + ", " + s.StateName,
                            HobbyDisplay = string.Join(", ", hobbies.Select(h => h.Hobby.Name).ToArray()),
                            SelectedHobbies = hobbies.Select(sh => sh.HobbyID).ToList()
                        };
            var alumnis = query.ToList().OrderByDescending(a => a.ModifiedDate);
            return alumnis;
        }

        public IEnumerable<StateDTO> GetStates()
        {
            var data = _dataContext.States
                .ToList()
                .Select(r => Mapping.Mapper.Map<StateDTO>(r));
            return data;
        }
        public IEnumerable<DistrictDTO> GetDistricts()
        {
            var data = _dataContext.Districts
                .ToList()
                .Select(r => Mapping.Mapper.Map<DistrictDTO>(r));
            return data;
        }

        public IEnumerable<HobbyDTO> GetHobbys()
        {
            var data = _dataContext.Hobbies
                .ToList()
                .Select(h => Mapping.Mapper.Map<HobbyDTO>(h));
            return data;
        }

        public int GetHobbyIDByName(string hobbyName)
        {
            var checkData = _dataContext.Hobbies.FirstOrDefault(h => h.Name == hobbyName);
            return checkData.HobbyID;
        }

        public IEnumerable<DistrictDTO> GetDistrictsByStateID(int stateID)
        {
            var query = from d in _dataContext.Districts
                        join s in _dataContext.States
                        on d.StateID equals s.StateID
                        where s.StateID == stateID
                        select new DistrictDTO
                        {
                            DistrictID = d.DistrictID,
                            DistrictName = d.DistrictName,
                            StateID = s.StateID,
                            StateNames = s.StateName
                        };
            var result = query.ToList();
            return result;
        }

        public AlumniDTO GetAlumniByID(int alumniId)
        {
            
            var alumni = GetAlumnis().Where(a => a.AlumniID == alumniId).FirstOrDefault();
            return alumni;
        }

        public void InsertAlumni(AlumniDTO alumni)
        {
            var data = Mapping.Mapper.Map<Alumni>(alumni);
            data.ModifiedDate = DateTime.Now;
            _dataContext.Alumnis.InsertOnSubmit(data);
            _dataContext.SubmitChanges();

            if (alumni.SelectedHobbies != null)
            {
                int totalHobbies = alumni.SelectedHobbies.Count;
                if (totalHobbies > 0)
                {
                    foreach (var item in alumni.SelectedHobbies)
                    {
                        AlumniHobby alumniHobby = new AlumniHobby
                        {
                            AlumniID = data.AlumniID,
                            HobbyID = item
                        };
                        _dataContext.AlumniHobbies.InsertOnSubmit(alumniHobby);
                    }
                    _dataContext.SubmitChanges();
                }
            }
        }

        public void UpsertAlumni(AlumniDTO alumni)
        {
            try
            {
                var alumniHobbies = new DataTable();
                alumniHobbies.Columns.Add("AlumniID", typeof(int));
                alumniHobbies.Columns.Add("HobbyID", typeof(int));

                foreach (var item in alumni.SelectedHobbies)
                {
                    alumniHobbies.Rows.Add(alumni.AlumniID, item);
                }

                using (var connection = new SqlConnection(_dataContext.Connection.ConnectionString))
                {
                    using (var command = new SqlCommand("dbo.UpsertAlumni", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@AlumniID", SqlDbType.Int) { Value = (object)alumni.AlumniID ?? 0 });
                        command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Value = alumni.FirstName });
                        command.Parameters.Add(new SqlParameter("@MiddleName", SqlDbType.NVarChar, 50) { Value = (object)alumni.MiddleName ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Value = alumni.LastName });
                        command.Parameters.Add(new SqlParameter("@Gender", SqlDbType.VarChar, 10) { Value = alumni.Gender });
                        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) { Value = alumni.Email });
                        command.Parameters.Add(new SqlParameter("@MobileNumber", SqlDbType.NVarChar, 15) { Value = alumni.MobileNumber });
                        command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = alumni.Address });
                        command.Parameters.Add(new SqlParameter("@DistrictID", SqlDbType.Int) { Value = alumni.DistrictID });
                        command.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.Date) { Value = (object)alumni.DateOfBirth ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@GraduationYear", SqlDbType.Int) { Value = alumni.GraduationYear });
                        command.Parameters.Add(new SqlParameter("@Degree", SqlDbType.NVarChar, 100) { Value = (object)alumni.Degree ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@MajorID", SqlDbType.Int) { Value = alumni.MajorID });
                        command.Parameters.Add(new SqlParameter("@LinkedInProfile", SqlDbType.NVarChar, 255) { Value = (object)alumni.LinkedInProfile ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@PhotoPath", SqlDbType.NVarChar, 255) { Value = (object)alumni.PhotoPath ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@PhotoName", SqlDbType.NVarChar, 100) { Value = (object)alumni.PhotoName ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@ModifiedDate", SqlDbType.DateTime) { Value = DateTime.Now });
                        command.Parameters.Add(new SqlParameter("@AlumniHobbies", SqlDbType.Structured) { TypeName = "dbo.AlumniHobbiesType", Value = alumniHobbies });

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void UpdateAlumni(AlumniDTO alumni)
        {
            var existingAlumni = _dataContext.Alumnis
                .FirstOrDefault(a => a.AlumniID == alumni.AlumniID);
            var data = Mapping.Mapper.Map(alumni, existingAlumni);
            data.ModifiedDate = DateTime.Now;
            _dataContext.SubmitChanges();

            //jika alumni selected hobbies ga kosong, diproses
            if (alumni.SelectedHobbies != null)
            {
                //Retrieve hobbies to add and remove
                var existingHobbies = _dataContext.AlumniHobbies
                    .Where(ah => ah.AlumniID == alumni.AlumniID)
                    .Select(ah => ah.HobbyID)
                    .ToList();

                var hobbiesToAdd = alumni.SelectedHobbies.Except(existingHobbies).ToList();
                var hobbiesToRemove = existingHobbies.Except(alumni.SelectedHobbies).ToList();

                //Add new hobbies
                foreach (var hobbyID in hobbiesToAdd)
                {
                    var alumniHobby = new AlumniHobby
                    {
                        AlumniID = alumni.AlumniID,
                        HobbyID = hobbyID
                    };
                    _dataContext.AlumniHobbies.InsertOnSubmit(alumniHobby);
                    _dataContext.SubmitChanges();
                }

                //remove old hobbies
                foreach (var hobbyID in hobbiesToRemove)
                {
                    var alumniHobby = _dataContext.AlumniHobbies
                        .FirstOrDefault(ah => ah.AlumniID == alumni.AlumniID && ah.HobbyID == hobbyID);
                    if (alumniHobby != null)
                    {
                        _dataContext.AlumniHobbies.DeleteOnSubmit(alumniHobby);
                        _dataContext.SubmitChanges();
                    }
                }
            }
        }

        public void DeleteAlumni(int alumniId)
        {
            var data = _dataContext.Alumnis.FirstOrDefault(a => a.AlumniID == alumniId);
            

            var alumniHobbies = _dataContext.AlumniHobbies
                .Where(ah => ah.AlumniID == alumniId)
                .ToList();
            var alumniImages = _dataContext.AlumniImages
                .Where(ai => ai.AlumniID == alumniId)
                .ToList();
            foreach (var item in alumniHobbies)
            {
                _dataContext.AlumniHobbies.DeleteOnSubmit(item);
                _dataContext.SubmitChanges();
            }
            foreach (var item in alumniImages)
            {
                _dataContext.AlumniImages.DeleteOnSubmit(item);
                _dataContext.SubmitChanges();
            }
            _dataContext.Alumnis.DeleteOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public int GetStateIDByName(string stateName)
        {
            var checkData = _dataContext.States.FirstOrDefault(st => st.StateName == stateName);
            return checkData.StateID;
        }

        public int GetDistrictIDByName(string districtName)
        {
            var checkData = _dataContext.Districts.FirstOrDefault(st => st.DistrictName == districtName);
            return checkData.DistrictID;
        }

        public void ImportFromExcel(AlumniDTO alumni)
        {
            UpsertMultipleAlumni(new List<AlumniDTO> { alumni });
        }

        private bool CheckAlumniID(int id)
        {
            var checkData = _dataContext.Alumnis.ToList();
            foreach (var item in checkData)
            {
                if (item.AlumniID == id) return true;
            }
            return false;
        }

        public void UpsertMultipleAlumni(List<AlumniDTO> alumniDTO)
        {
            try
            {
                var alumniTable = new DataTable();
                alumniTable.Columns.Add("AlumniID", typeof(int));
                alumniTable.Columns.Add("FirstName", typeof(string));
                alumniTable.Columns.Add("MiddleName", typeof(string));
                alumniTable.Columns.Add("LastName", typeof(string));
                alumniTable.Columns.Add("Gender", typeof(string));
                alumniTable.Columns.Add("Email", typeof(string));
                alumniTable.Columns.Add("MobileNumber", typeof(string));
                alumniTable.Columns.Add("Address", typeof(string));
                alumniTable.Columns.Add("DistrictID", typeof(int));
                alumniTable.Columns.Add("DateOfBirth", typeof(DateTime));
                alumniTable.Columns.Add("GraduationYear", typeof(int));
                alumniTable.Columns.Add("Degree", typeof(string));
                alumniTable.Columns.Add("MajorID", typeof(int));
                alumniTable.Columns.Add("LinkedInProfile", typeof(string));

                var hobbiesTable = new DataTable();
                hobbiesTable.Columns.Add("AlumniID", typeof(int));
                hobbiesTable.Columns.Add("HobbyID", typeof(int));

                bool hasHobbies = false; // Flag to check if there are hobbies

                foreach (var alumni in alumniDTO)
                {
                    alumniTable.Rows.Add(
                        alumni.AlumniID, alumni.FirstName, alumni.MiddleName, alumni.LastName, alumni.Gender, alumni.Email,
                        alumni.MobileNumber, alumni.Address, alumni.DistrictID, alumni.DateOfBirth ?? (object)DBNull.Value,
                        alumni.GraduationYear, alumni.Degree, alumni.MajorID, alumni.LinkedInProfile
                    );

                    if (alumni.SelectedHobbies != null && alumni.SelectedHobbies.Any()) // Check if hobbies exist
                    {
                        hasHobbies = true;
                        foreach (var hobby in alumni.SelectedHobbies)
                        {
                            hobbiesTable.Rows.Add(alumni.AlumniID, hobby);
                        }
                    }
                }

                using (var connection = new SqlConnection(_dataContext.Connection.ConnectionString))
                {
                    using (var command = new SqlCommand("dbo.UpsertMultipleAlumni", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var alumniParam = new SqlParameter("@AlumniList", SqlDbType.Structured)
                        {
                            TypeName = "dbo.AlumniType",
                            Value = alumniTable
                        };
                        command.Parameters.Add(alumniParam);

                        if (hasHobbies) // Only add this parameter if hobbies exist
                        {
                            var hobbiesParam = new SqlParameter("@AlumniHobbies", SqlDbType.Structured)
                            {
                                TypeName = "dbo.AlumniHobbiesType",
                                Value = hobbiesTable
                            };
                            command.Parameters.Add(hobbiesParam);
                        }

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpsertMultipleAlumni: " + ex.Message);
            }
        }
    }
}
