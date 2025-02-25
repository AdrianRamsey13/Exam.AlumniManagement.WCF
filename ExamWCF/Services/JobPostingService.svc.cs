using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AutoMapper.Configuration.Annotations;
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

        public IEnumerable<JobPostingDTO> GetJobPostings()
        {
            var query = from jp in _dataContext.JobPostings
                        join js in _dataContext.JobSkills on jp.JobID equals js.JobID into jps
                        join jat in _dataContext.JobAttachmentTypes on jp.JobID equals jat.JobID into jats
                        join ja in _dataContext.JobAttachments on jp.JobID equals ja.JobID into jas
                        join et in _dataContext.EmploymentTypes on jp.EmploymentTypeID equals et.EmploymentTypeID
                        join jc in _dataContext.JobCandidates on jp.JobID equals jc.JobID into candidates
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
                            SkillDisplay = string.Join(", ", jps.Select(h => h.Skill.Name).ToArray()),
                            AttachmentTypeDisplay = string.Join(", ", jats.Select(h => h.AttachmentType.Name).ToArray()),
                            //JobAttachmentDisplay = string.Join(", ", jas.Select(h => h.Alumni.FirstName).ToArray()),
                            EmploymentTypeNames = et.Name,
                            SelectedAttachmentTypes = jats.Select(h => (int)h.AttachmentTypeID).ToList(),
                            SelectedSkills = jps.Select(h => (int)h.SkillID).ToList(),
                            //SelectedJobAttachments = jas.Select(h => h.AlumniID).ToList()
                            Candidates = jas.Select(h => h.AlumniID).Distinct().Count(),
                            SelectedCandidates = candidates.Select(jc => jc.AlumniID).ToList(),
                            TotalCandidates = jp.JobCandidates.Count()
                        };
            var jobs = query.ToList().OrderByDescending(a => a.ModifiedDate);
            return jobs;
        }

        public IEnumerable<SkillsDTO> GetSkills()
        {
            var data = _dataContext.Skills
                .ToList()
                .Select(r => Mapping.Mapper.Map<SkillsDTO>(r));
            return data;
        }

        public IEnumerable<EmploymentTypeDTO> GetEmploymentTypes()
        {
            var data = _dataContext.EmploymentTypes
                .ToList()
                .Select(r => Mapping.Mapper.Map<EmploymentTypeDTO>(r));
            return data;
        }

        public IEnumerable<AttachmentTypeDTO> GetAttachmentTypes()
        {
            var data = _dataContext.AttachmentTypes
                .ToList()
                .Select(r => Mapping.Mapper.Map<AttachmentTypeDTO>(r));
            return data;
        }

        public IEnumerable<JobAttachmentDTO> GetJobAttachments()
        {
            var data = _dataContext.JobAttachments
                .ToList()
                .Select(r => Mapping.Mapper.Map<JobAttachmentDTO>(r));
            return data;
        }

        public JobPostingDTO GetJobPostingByID(Guid jobPostingID)
        {
            var job = GetJobPostings()
                        .Where(jp => jp.JobID == jobPostingID)
                        .ToList()
                        .OrderByDescending(jp => jp.ModifiedDate)
                        .FirstOrDefault();

            return job;
        }

        public SkillsDTO GetSkillByID(int skillID)
        {
            var data = _dataContext.Skills
                .FirstOrDefault(h => h.SkillID == skillID);
            var mapped = Mapping.Mapper.Map<SkillsDTO>(data);
            return mapped;
        }

        public EmploymentTypeDTO GetEmploymentTypeByID(byte employmentTypeID)
        {
            var data = _dataContext.EmploymentTypes
                .FirstOrDefault(h => h.EmploymentTypeID == employmentTypeID);
            var mapped = Mapping.Mapper.Map<EmploymentTypeDTO>(data);
            return mapped;
        }

        public AttachmentTypeDTO GetAttachmentTypeByID(byte attachmentTypeID)
        {
            throw new NotImplementedException();
        }

        public JobAttachmentDTO GetJobAttachmentByID(int attachmentID)
        {
            throw new NotImplementedException();
        }

        public void InsertJobPosting(JobPostingDTO jobPosting)
        {
            var data = Mapping.Mapper.Map<JobPosting>(jobPosting);
            data.ModifiedDate = DateTime.Now;
            data.JobID = Guid.NewGuid();
            _dataContext.JobPostings.InsertOnSubmit(data);
            _dataContext.SubmitChanges();
            int totalSkills = jobPosting.SelectedSkills.Count;
            int totalAttachmentTypes = jobPosting.SelectedAttachmentTypes.Count;
            int totalAlumni = jobPosting.SelectedJobAttachments.Count;
            if (totalSkills > 0)
            {
                foreach (var skillID in jobPosting.SelectedSkills)
                {
                    var jobSkill = new JobSkill
                    {
                        JobID = data.JobID,
                        SkillID = Convert.ToByte(skillID)
                    };
                    _dataContext.JobSkills.InsertOnSubmit(jobSkill);
                    _dataContext.SubmitChanges();
                }
            }
            if (totalAttachmentTypes > 0)
            {
                foreach (var attachmentTypeID in jobPosting.SelectedAttachmentTypes)
                {
                    var jobAttachmentType = new JobAttachmentType
                    {
                        JobID = data.JobID,
                        AttachmentTypeID = Convert.ToByte(attachmentTypeID)
                    };
                    _dataContext.JobAttachmentTypes.InsertOnSubmit(jobAttachmentType);
                    _dataContext.SubmitChanges();
                }
            }
        }

        //if untuk alumni attachments(jobattachment)

        public void UpdateJobPosting(JobPostingDTO jobPosting)
        {
            var existingJob = _dataContext.JobPostings
                .FirstOrDefault(a => a.JobID == jobPosting.JobID);

            Mapping.Mapper.Map(jobPosting, existingJob);
            existingJob.ModifiedDate = DateTime.Now;

            //remove existing skills
            var existingSkills = _dataContext.JobSkills
                .Where(a => a.JobID == jobPosting.JobID)
                .Select(a => a.SkillID)
                .ToList();
            //_dataContext.JobSkills.DeleteAllOnSubmit(existingSkills);

            var existingAttachmentTypes = _dataContext.JobAttachmentTypes
                .Where(a => a.JobID == jobPosting.JobID)
                .Select(a => a.AttachmentTypeID)
                .ToList();

            //determine skills to add and remove
            var skillsToAdd = jobPosting.SelectedSkills.Except(existingSkills.Select(s => (int)s)).ToList();
            var skillsToRemove = existingSkills.Select(s => (int)s).Except(jobPosting.SelectedSkills).ToList();
            var attachmentTypesToAdd = jobPosting.SelectedAttachmentTypes.Except(existingAttachmentTypes.Select(a => (int)a)).ToList();
            var attachmentTypesToRemove = existingAttachmentTypes.Select(a => (int)a).Except(jobPosting.SelectedAttachmentTypes).ToList();

            //add new hobbies
            foreach (var skill in skillsToAdd)
            {
                var jobSkill = new JobSkill
                {
                    JobID = jobPosting.JobID,
                    SkillID = Convert.ToByte(skill)
                };
                _dataContext.JobSkills.InsertOnSubmit(jobSkill);
            }

            //remove old hobbies
            foreach (var skill in skillsToRemove)
            {
                var jobSkill = _dataContext.JobSkills
                    .FirstOrDefault(a => a.JobID == jobPosting.JobID && a.SkillID == skill);
                if (jobSkill != null)
                {
                    _dataContext.JobSkills.DeleteOnSubmit(jobSkill);
                }
            }

            foreach (var attachmentType in attachmentTypesToAdd)
            {
                var jobAttachmentType = new JobAttachmentType
                {
                    JobID = jobPosting.JobID,
                    AttachmentTypeID = Convert.ToByte(attachmentType)
                };
                _dataContext.JobAttachmentTypes.InsertOnSubmit(jobAttachmentType);
            }

            foreach (var attachmentType in attachmentTypesToRemove)
            {
                var jobAttachmentType = _dataContext.JobAttachmentTypes
                    .FirstOrDefault(a => a.JobID == jobPosting.JobID && a.AttachmentTypeID == attachmentType);
                if (jobAttachmentType != null)
                {
                    _dataContext.JobAttachmentTypes.DeleteOnSubmit(jobAttachmentType);
                }
            }
            _dataContext.SubmitChanges();
        }

        public void DeleteJobPosting(Guid jobPostingID)
        {
            var data = _dataContext.JobPostings.FirstOrDefault(a => a.JobID == jobPostingID);
            if (data == null)
                throw new Exception("Alumni not found");

            // Delete related skills first (to avoid foreign key violations)
            var existingSkills = _dataContext.JobSkills.Where(ah => ah.JobID == jobPostingID);
            _dataContext.JobSkills.DeleteAllOnSubmit(existingSkills);

            // Delete related attachment first (to avoid foreign key violations)
            var existingAttachment = _dataContext.JobAttachmentTypes.Where(ah => ah.JobID == jobPostingID);
            _dataContext.JobAttachmentTypes.DeleteAllOnSubmit(existingAttachment);

            // Now delete the alumni record
            _dataContext.JobPostings.DeleteOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public void ApplyToJob(JobAttachmentDTO applyJob)
        {
            var data = Mapping.Mapper.Map<JobAttachment>(applyJob);
            _dataContext.JobAttachments.InsertOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public void InsertJobCandidate(JobCandidateDTO jobCandidate)
        {
            var jobCandidates = new JobCandidate
            {
                AlumniID = jobCandidate.AlumniID,
                JobID = jobCandidate.JobID,
                ApplyDate = DateTime.Now
            };

            _dataContext.JobCandidates.InsertOnSubmit(jobCandidates);
            _dataContext.SubmitChanges();
        }

        public void UpsertJobPosting(JobPostingDTO jobPosting)
        {
            try
            {
                //jobPosting.JobID = Guid.NewGuid();
                // Job Skills Table
                var jobSkills = new DataTable();
                jobSkills.Columns.Add("JobID", typeof(Guid));
                jobSkills.Columns.Add("SkillID", typeof(int));

                foreach (var skill in jobPosting.SelectedSkills)
                {
                    jobSkills.Rows.Add(jobPosting.JobID, skill);
                }

                // Job Attachment Types Table
                var jobAttachmentTypes = new DataTable();
                jobAttachmentTypes.Columns.Add("JobID", typeof(Guid));
                jobAttachmentTypes.Columns.Add("AttachmentTypeID", typeof(int));

                foreach (var attachmentType in jobPosting.SelectedAttachmentTypes)
                {
                    jobAttachmentTypes.Rows.Add(jobPosting.JobID, attachmentType);
                }

                using (var connection = new SqlConnection(_dataContext.Connection.ConnectionString))
                {
                    using (var command = new SqlCommand("dbo.UpsertJobPosting", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@JobID", SqlDbType.UniqueIdentifier) { Value = (object)jobPosting.JobID ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 255) { Value = jobPosting.Title });
                        command.Parameters.Add(new SqlParameter("@Company", SqlDbType.NVarChar, 255) { Value = jobPosting.Company });
                        command.Parameters.Add(new SqlParameter("@JobDescription", SqlDbType.NVarChar, -1) { Value = jobPosting.JobDescription });
                        command.Parameters.Add(new SqlParameter("@EmploymentTypeID", SqlDbType.TinyInt) { Value = jobPosting.EmploymentTypeID });
                        command.Parameters.Add(new SqlParameter("@MinimumExperience", SqlDbType.TinyInt) { Value = jobPosting.MinimumExperience });
                        command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = jobPosting.IsActive });
                        command.Parameters.Add(new SqlParameter("@IsClosed", SqlDbType.Bit) { Value = (object)jobPosting.IsClosed ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@ModifiedDate", SqlDbType.DateTime) { Value = DateTime.Now });

                        command.Parameters.Add(new SqlParameter("@JobSkills", SqlDbType.Structured) { TypeName = "dbo.JobSkillsType", Value = jobSkills });
                        command.Parameters.Add(new SqlParameter("@JobAttachmentTypes", SqlDbType.Structured) { TypeName = "dbo.JobAttachmentTypesType", Value = jobAttachmentTypes });

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

        public IEnumerable<JobCandidateDTO> GetAllCandidateByJobID(Guid jobID)
        {
            var data = from c in _dataContext.JobCandidates
                       where c.JobID == jobID
                       select c;

            var result = data.ToList().Select(d => new JobCandidateDTO
            {
                JobID = d.JobID,
                ApplyDate = d.ApplyDate,
                FullName = d.Alumni.FirstName + " " + (d.Alumni.MiddleName ?? "") + " " + d.Alumni.LastName,
                JobAttachments = d.JobPosting.JobAttachments.Where(ja => ja.AlumniID == d.AlumniID).Select(ja => new JobAttachmentDTO
                {
                    FilePath = ja.FilePath,
                    FileName = ja.FileName,
                }
                ).ToList(),
                AlumniID = d.AlumniID
            });
            return result;
        }
    }
}
