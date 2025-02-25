using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IJobAttachmentService" in both code and config file together.
    [ServiceContract]
    public interface IJobAttachmentService
    {
        [OperationContract]
        IEnumerable<JobAttachmentDTO> GetCandidates(Guid jobID);
    }
}
