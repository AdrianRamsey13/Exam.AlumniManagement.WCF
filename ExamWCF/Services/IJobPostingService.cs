using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AutoMapper.Configuration.Conventions;
using ExamWCF.DTOs;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IJobPostingService" in both code and config file together.
    [ServiceContract]
    public interface IJobPostingService
    {
        [OperationContract]
        IEnumerable<JobPostingDTO> GetJobPostings();
        [OperationContract]
        JobPostingDTO GetJobPostingByID(Guid id);
        [OperationContract]
        void InsertJobPosting(JobPostingDTO jobPostingDTO);
        [OperationContract]
        void UpdateJobPosting(JobPostingDTO jobPostingDTO);
        [OperationContract]
        void DeleteJobPosting(Guid id);
    }
}
