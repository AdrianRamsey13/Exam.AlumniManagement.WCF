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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CandidateService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CandidateService.svc or CandidateService.svc.cs at the Solution Explorer and start debugging.
    public class CandidateService : ICandidateService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }
        public IEnumerable<JobAttachmentDTO> GetCandidates(Guid jobID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobCandidateDTO> GetCandidates2(Guid jobID)
        {
            var data = _dataContext.JobCandidates
                .Where(r => r.JobID == jobID)
                .ToList()
                .OrderByDescending(r => r.ApplyDate)
                .Select(r => Mapping.Mapper.Map<JobCandidateDTO>(r));
            return data;

        }
    }
}
