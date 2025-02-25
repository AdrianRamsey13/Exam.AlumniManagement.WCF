using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
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
        IEnumerable<SkillsDTO> GetSkills();
        [OperationContract]
        IEnumerable<EmploymentTypeDTO> GetEmploymentTypes();
        [OperationContract]
        IEnumerable<AttachmentTypeDTO> GetAttachmentTypes();
        [OperationContract]
        IEnumerable<JobAttachmentDTO> GetJobAttachments();
        [OperationContract]
        JobPostingDTO GetJobPostingByID(Guid jobPostingID);
        [OperationContract]
        SkillsDTO GetSkillByID(int skillID);
        [OperationContract]
        EmploymentTypeDTO GetEmploymentTypeByID(byte employmentTypeID);
        [OperationContract]
        AttachmentTypeDTO GetAttachmentTypeByID(byte attachmentTypeID);
        [OperationContract]
        JobAttachmentDTO GetJobAttachmentByID(int attachmentID);
        [OperationContract]
        void InsertJobPosting(JobPostingDTO jobPosting);
        [OperationContract]
        void UpdateJobPosting(JobPostingDTO jobPosting);
        [OperationContract]
        void DeleteJobPosting(Guid jobPostingID);
        [OperationContract]
        void ApplyToJob(JobAttachmentDTO applyJob);
        [OperationContract]
        void InsertJobCandidate(JobCandidateDTO jobCandidate);
        [OperationContract]
        void UpsertJobPosting(JobPostingDTO jobPosting);
        [OperationContract]
        IEnumerable<JobCandidateDTO> GetAllCandidateByJobID(Guid jobID);
    }
}
