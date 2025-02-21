using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using ExamWCF.DTOs;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAlumniService" in both code and config file together.
    [ServiceContract]
    public interface IAlumniService
    {
        [OperationContract]
        IEnumerable<AlumniDTO> GetAlumnis();
        [OperationContract]
        IEnumerable<DistrictDTO> GetDistrictsByStateID(int stateID);
        [OperationContract]
        IEnumerable<StateDTO> GetStates();
        [OperationContract]
        IEnumerable<DistrictDTO> GetDistricts();
        [OperationContract]
        IEnumerable<HobbyDTO> GetHobbys();
        [OperationContract]
        int GetHobbyIDByName(string hobbyName);
        [OperationContract]
        AlumniDTO GetAlumniByID(int alumniId);
        [OperationContract]
        void InsertAlumni(AlumniDTO alumni);
        [OperationContract]
        void UpdateAlumni(AlumniDTO alumni);
        [OperationContract]
        void DeleteAlumni(int alumniId);
        [OperationContract]
        int GetStateIDByName(string stateName);
        [OperationContract]
        int GetDistrictIDByName(string districtName);
        [OperationContract]
        void ImportFromExcel(AlumniDTO alumni);
    }
}
