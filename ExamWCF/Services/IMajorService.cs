using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMajorService" in both code and config file together.
    [ServiceContract]
    public interface IMajorService
    {
        [OperationContract]
        IEnumerable<MajorDTO> GetMajors();
        [OperationContract]
        IEnumerable<MajorDTO> GetMajorsByFacultyID(int facultyID);
        [OperationContract]
        MajorDTO GetMajorByID(int majorID);
        [OperationContract]
        void InsertMajor(MajorDTO major);
        [OperationContract]
        void UpdateMajor(MajorDTO major);
        [OperationContract]
        void DeleteMajor(int majorID);
        [OperationContract]
        int GetFacultyIDByName(string facultyName);
        [OperationContract]
        int GetMajorIDByName(string majorName);
    }
}
