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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "JobPostingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select JobPostingService.svc or JobPostingService.svc.cs at the Solution Explorer and start debugging.
    public class JobPostingService : IJobPostingService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }

        public JobPostingService()
        {
            _dataContext = new AlumniManagementDataContext(ConnectionStringSettings);
        }

        public JobPostingDTO GetJobPostingByID(Guid id)
        {
            var jobPosting = GetJobPostings().Where(jp => jp.JobID == id).FirstOrDefault();
            return jobPosting;
        }

        public IEnumerable<JobPostingDTO> GetJobPostings()
        {
            var query = from jp in _dataContext.JobPostings
                        join ja in _dataContext.JobAttachments
                        on jp.JobID equals ja.JobID into jaGroup
                        select new JobPostingDTO
                        {
                            JobID = jp.JobID,
                            Title = jp.Title,
                            Company = jp.Company,
                            JobDescription = jp.JobDescription,
                            EmploymentTypeID = jp.EmploymentTypeID,
                            MinimumExperience = jp.MinimumExperience,
                            ModifiedDate = jp.ModifiedDate,
                            IsActive = jp.IsActive,
                            IsClosed = jp.IsClosed,
                            //count the candidates
                            TotalCandidates = jaGroup.Count()
                        };
            var result = query.OrderByDescending(a => a.ModifiedDate);
            return result;
        }

        public void InsertJobPosting(JobPostingDTO jobPostingDTO)
        {
            var newJobPosting = Mapping.Mapper.Map<JobPosting>(jobPostingDTO);
            newJobPosting.ModifiedDate = DateTime.Now;
            _dataContext.JobPostings.InsertOnSubmit(newJobPosting);
            _dataContext.SubmitChanges();
        }

        public void UpdateJobPosting(JobPostingDTO jobPostingDTO)
        {
            var existingJob = _dataContext.JobPostings
                .FirstOrDefault(jp => jp.JobID == jobPostingDTO.JobID);
            var data = Mapping.Mapper.Map(jobPostingDTO, existingJob);
            data.ModifiedDate = DateTime.Now;
            _dataContext.SubmitChanges();
        }
        public void DeleteJobPosting(Guid id)
        {
            var selectedJob = _dataContext.JobPostings.FirstOrDefault(jp => jp.JobID == id);
            _dataContext.JobPostings.DeleteOnSubmit(selectedJob);
            _dataContext.SubmitChanges();
        }
    }
}
