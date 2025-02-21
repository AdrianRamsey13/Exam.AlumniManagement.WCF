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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IJobHistoryService" in both code and config file together.
    [ServiceContract]
    public interface IJobHistoryService
    {
        [OperationContract]
        IEnumerable<JobHistoryDTO> GetJobHistories(int alumniID);
        [OperationContract]
        JobHistoryDTO GetJobHistoryByID(int id);
        [OperationContract]
        void InsertJobHistory(JobHistoryDTO jobHistoryDTO);
        [OperationContract]
        void UpdateJobHistory(JobHistoryDTO jobHistoryDTO);
        [OperationContract]
        void DeleteJobHistory(int id);
    }
}
