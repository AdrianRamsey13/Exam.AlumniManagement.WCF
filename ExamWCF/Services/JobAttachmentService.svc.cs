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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "JobAttachmentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select JobAttachmentService.svc or JobAttachmentService.svc.cs at the Solution Explorer and start debugging.
    public class JobAttachmentService : IJobAttachmentService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionStringSettings
        {
            get => connectionStringSettings; set =>
                ConnectionStringSettings = value;
        }

        public JobAttachmentService()
        {
            _dataContext = new AlumniManagementDataContext(ConnectionStringSettings);
        }

        public IEnumerable<JobAttachmentDTO> GetCandidates(Guid jobID)
        {
            var query = from ja in _dataContext.JobAttachments
                        join a in _dataContext.Alumnis
                        on ja.AlumniID equals a.AlumniID
                        where ja.JobID == jobID
                        select new JobAttachmentDTO
                        {
                            AttachmentID = ja.AttachmentID,
                            JobID = ja.JobID,
                            AlumniID = ja.AlumniID,
                            AlumniName = a.FirstName + " " + a.LastName,
                            //ApplyDate = a.ModifiedDate,
                            FileName = ja.FileName,
                            FilePath = ja.FilePath
                        };
            return query;
        }
    }
}
