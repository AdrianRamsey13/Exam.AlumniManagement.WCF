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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "JobHistoryService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select JobHistoryService.svc or JobHistoryService.svc.cs at the Solution Explorer and start debugging.
    public class JobHistoryService : IJobHistoryService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }

        public JobHistoryService()
        {
            _dataContext = new AlumniManagementDataContext(ConnectionStringSettings);
        }

        public IEnumerable<JobHistoryDTO> GetJobHistories(int alumniID)
        {
            var query = from jh in _dataContext.JobHistories
                        join a in _dataContext.Alumnis
                        on jh.AlumniID equals a.AlumniID
                        where a.AlumniID == alumniID
                        select new JobHistoryDTO
                        {
                            JobHistoryID = jh.JobHistoryID,
                            AlumniID = a.AlumniID,
                            JobTitle = jh.JobTitle,
                            Company = jh.Company,
                            StartDate = jh.StartDate,
                            EndDate = jh.EndDate,
                            Description = jh.Description,
                            ModifiedDate = jh.ModifiedDate
                        };
            var result = query.OrderByDescending(a => a.ModifiedDate);
            return result;
        }

        public JobHistoryDTO GetJobHistoryByID(int id)
        {
            var jh = _dataContext.JobHistories.FirstOrDefault(r => r.JobHistoryID == id);
            var result = Mapping.Mapper.Map<JobHistoryDTO>(jh);
            return result;
        }

        public void InsertJobHistory(JobHistoryDTO jobHistoryDTO)
        {
            var newJob = Mapping.Mapper.Map<JobHistory>(jobHistoryDTO);
            newJob.ModifiedDate = DateTime.Now;
            _dataContext.JobHistories.InsertOnSubmit(newJob);
            _dataContext.SubmitChanges();
        }

        public void UpdateJobHistory(JobHistoryDTO jobHistoryDTO)
        {
            var existingJob = _dataContext.JobHistories
                .FirstOrDefault(jh => jh.JobHistoryID == jobHistoryDTO.JobHistoryID);
            var data = Mapping.Mapper.Map(jobHistoryDTO, existingJob);
            data.ModifiedDate = DateTime.Now;
            _dataContext.SubmitChanges();
        }

        public void DeleteJobHistory(int id)
        {
            var selectedJob = _dataContext.JobHistories.FirstOrDefault(a => a.JobHistoryID == id);
            _dataContext.JobHistories.DeleteOnSubmit(selectedJob);
            _dataContext.SubmitChanges();
        }
    }
}
