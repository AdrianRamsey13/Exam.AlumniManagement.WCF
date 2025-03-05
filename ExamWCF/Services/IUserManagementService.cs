using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;
using System.ServiceModel;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserManagementService" in both code and config file together.
    [ServiceContract]
    public interface IUserManagementService
    {
        [OperationContract]
        bool IsSuperadminExists();
        [OperationContract]
        void RegisterUser(string fullName, string email, string password, bool superadmin);
        [OperationContract]
        void UpdatePassword(string id, string password);
        [OperationContract]
        void UpdateUser(AspNetUserDTO.UserDTO user);

        [OperationContract]
        void AssignSuperadmin(string id);

        [OperationContract]
        void DeleteUser(string id);
        [OperationContract]
        AspNetUserDTO.UserDTO GetUser(string id);
        [OperationContract]
        IEnumerable<AspNetUserDTO.UserDTO> GetAllUsers();
        [OperationContract]
        IEnumerable<AspNetUserDTO.UserDTO> GetUsersByRole(string roleName);
        [OperationContract]
        AspNetUserDTO.UserDTO GetUserByEmail(string email);
        [OperationContract]
        //void UpdateUserFullName(string id, string fullName);
        void UpdateUserFullName(AspNetUserDTO.UserDTO userModel);
        [OperationContract]
        IEnumerable<AspNetUserDTO.RoleDTO> GetRoles();
        [OperationContract]
        void InsertRole(AspNetUserDTO.RoleDTO role);

        [OperationContract]
        void DeleteRole(string roleId);
    }
}
