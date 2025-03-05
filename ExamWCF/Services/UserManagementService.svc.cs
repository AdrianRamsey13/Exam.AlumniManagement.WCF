using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;
using ExamWCF.Entities;
using System.Transactions;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserManagementService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserManagementService.svc or UserManagementService.svc.cs at the Solution Explorer and start debugging.
    public class UserManagementService : IUserManagementService
    {
        private AlumniManagementDataContext _dataContext;
        public string connection = System.Configuration.ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public UserManagementService()
        {
            _dataContext = new AlumniManagementDataContext(connection);
        }

        public void AssignSuperadmin(string id)
        {
            var existingUser = _dataContext.AspNetUsers.Where(u => u.Id == id).First();
            string roleId = _dataContext.AspNetRoles.Where(r => r.Name == "Superadmin").FirstOrDefault().ToString();

            if (existingUser != null && roleId != null)
            {
                var userRole = new AspNetUserRole
                {
                    UserId = id,
                    RoleId = roleId
                };

                _dataContext.AspNetUserRoles.InsertOnSubmit(userRole);
                _dataContext.SubmitChanges();
            }
        }

        public void DeleteUser(string id)
        {
            //Check existing user
            var existingUser = _dataContext.AspNetUsers.Where(u => u.Id == id).First();
            if (existingUser != null)
            {
                _dataContext.AspNetUsers.DeleteOnSubmit(existingUser);
                _dataContext.SubmitChanges();
            }else
            {
                throw new Exception("User not found");
            }
        }

        public IEnumerable<AspNetUserDTO.UserDTO> GetAllUsers()
        {
            var users = _dataContext.AspNetUsers.Select(u => new AspNetUserDTO.UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                PasswordHash = u.PasswordHash,
                SecurityStamp = u.SecurityStamp,
                PhoneNumber = u.PhoneNumber,
                TwoFactorEnabled = u.TwoFactorEnabled,
                LockoutEndDateUtc = u.LockoutEndDateUtc,
                LockoutEnabled = u.LockoutEnabled,
                AccessFailedCount = u.AccessFailedCount,
                UserName = u.UserName,
                UserRoles = u.AspNetUserRoles.Select(ur => new AspNetUserDTO.UserRoleDTO
                {
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    Role = new AspNetUserDTO.RoleDTO
                    {
                        Id = ur.AspNetRole.Id,
                        Name = ur.AspNetRole.Name,
                        RolePermissions = ur.AspNetRole.AspNetRolePermissions.Select(rp => new AspNetUserDTO.RolePermissionDTO
                        {
                            RoleId = rp.RoleId,
                            PermissionId = rp.PermissionId,
                            Role = new AspNetUserDTO.RoleDTO
                            {
                                Id = rp.AspNetRole.Id,
                                Name = rp.AspNetRole.Name
                            },
                            Permission = new AspNetUserDTO.PermissionDTO
                            {
                                Id = rp.AspNetPermission.Id,
                                Name = rp.AspNetPermission.Name
                            }
                        }).ToList()
                    }
                }).ToList(),
                UserClaims = u.AspNetUserClaims.Select(uc => new AspNetUserDTO.UserClaimDTO
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClaimType = uc.ClaimType,
                    ClaimValue = uc.ClaimValue
                }).ToList(),
                UserLogins = u.AspNetUserLogins.Select(ul => new AspNetUserDTO.UserLoginDTO
                {
                    LoginProvider = ul.LoginProvider,
                    ProviderKey = ul.ProviderKey,
                    UserId = ul.UserId
                }).ToList()
            }).ToList();
            return users;
        }

        public AspNetUserDTO.UserDTO GetUser(string id)
        {
            var user = _dataContext.AspNetUsers.Where(u => u.Id == id).Select(u => new AspNetUserDTO.UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                PasswordHash = u.PasswordHash,
                SecurityStamp = u.SecurityStamp,
                PhoneNumber = u.PhoneNumber,
                TwoFactorEnabled = u.TwoFactorEnabled,
                LockoutEndDateUtc = u.LockoutEndDateUtc,
                LockoutEnabled = u.LockoutEnabled,
                AccessFailedCount = u.AccessFailedCount,
                UserName = u.UserName,
                UserRoles = u.AspNetUserRoles.Select(ur => new AspNetUserDTO.UserRoleDTO
                {
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    Role = new AspNetUserDTO.RoleDTO
                    {
                        Id = ur.AspNetRole.Id,
                        Name = ur.AspNetRole.Name,
                        RolePermissions = ur.AspNetRole.AspNetRolePermissions.Select(rp => new AspNetUserDTO.RolePermissionDTO
                        {
                            RoleId = rp.RoleId,
                            PermissionId = rp.PermissionId,
                            Role = new AspNetUserDTO.RoleDTO
                            {
                                Id = rp.AspNetRole.Id,
                                Name = rp.AspNetRole.Name
                            },
                            Permission = new AspNetUserDTO.PermissionDTO
                            {
                                Id = rp.AspNetPermission.Id,
                                Name = rp.AspNetPermission.Name
                            }
                        }).ToList()
                    }
                }).ToList(),
                UserClaims = u.AspNetUserClaims.Select(uc => new AspNetUserDTO.UserClaimDTO
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClaimType = uc.ClaimType,
                    ClaimValue = uc.ClaimValue
                }).ToList(),
                UserLogins = u.AspNetUserLogins.Select(ul => new AspNetUserDTO.UserLoginDTO
                {
                    LoginProvider = ul.LoginProvider,
                    ProviderKey = ul.ProviderKey,
                    UserId = ul.UserId
                }).ToList()
            }).FirstOrDefault();
            return user;
        }

        public AspNetUserDTO.UserDTO GetUserByEmail(string email)
        {
            var user = _dataContext.AspNetUsers.Where(u => u.Email == email).Select(u => new AspNetUserDTO.UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                PasswordHash = u.PasswordHash,
                SecurityStamp = u.SecurityStamp,
                PhoneNumber = u.PhoneNumber,
                TwoFactorEnabled = u.TwoFactorEnabled,
                LockoutEndDateUtc = u.LockoutEndDateUtc,
                LockoutEnabled = u.LockoutEnabled,
                AccessFailedCount = u.AccessFailedCount,
                UserName = u.UserName,
                UserRoles = u.AspNetUserRoles.Select(ur => new AspNetUserDTO.UserRoleDTO
                {
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    Role = new AspNetUserDTO.RoleDTO
                    {
                        Id = ur.AspNetRole.Id,
                        Name = ur.AspNetRole.Name,
                        RolePermissions = ur.AspNetRole.AspNetRolePermissions.Select(rp => new AspNetUserDTO.RolePermissionDTO
                        {
                            RoleId = rp.RoleId,
                            PermissionId = rp.PermissionId,
                            Role = new AspNetUserDTO.RoleDTO
                            {
                                Id = rp.AspNetRole.Id,
                                Name = rp.AspNetRole.Name
                            },
                            Permission = new AspNetUserDTO.PermissionDTO
                            {
                                Id = rp.AspNetPermission.Id,
                                Name = rp.AspNetPermission.Name
                            }
                        }).ToList()
                    }
                }).ToList(),
                UserClaims = u.AspNetUserClaims.Select(uc => new AspNetUserDTO.UserClaimDTO
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClaimType = uc.ClaimType,
                    ClaimValue = uc.ClaimValue
                }).ToList(),
                UserLogins = u.AspNetUserLogins.Select(ul => new AspNetUserDTO.UserLoginDTO
                {
                    LoginProvider = ul.LoginProvider,
                    ProviderKey = ul.ProviderKey,
                    UserId = ul.UserId
                }).ToList()
            }).FirstOrDefault();
            return user;
        }

        public IEnumerable<AspNetUserDTO.UserDTO> GetUsersByRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool IsSuperadminExists()
        {
            var user = _dataContext.AspNetUsers.Where(u => u.AspNetUserRoles.Any(ur => ur.AspNetRole.Name == "Superadmin")).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RegisterUser(string fullName, string email, string password, bool superadmin)
        {
            var user = new AspNetUser
            {
                FullName = fullName,
                Email = email,
                EmailConfirmed = false,
                PasswordHash = password,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = "",
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                UserName = email
            };
        }

        public void UpdatePassword(string id, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(AspNetUserDTO.UserDTO user)
        {
            var existingUser = _dataContext.AspNetUsers.SingleOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                if (existingUser != null)
                {
                    try
                    {
                        // Update the properties
                        existingUser.Email = user.Email;
                        existingUser.EmailConfirmed = user.EmailConfirmed;
                        existingUser.PasswordHash = user.PasswordHash;
                        existingUser.SecurityStamp = user.SecurityStamp;
                        existingUser.PhoneNumber = user.PhoneNumber;
                        existingUser.TwoFactorEnabled = user.TwoFactorEnabled;
                        existingUser.LockoutEndDateUtc = user.LockoutEndDateUtc;
                        existingUser.LockoutEnabled = user.LockoutEnabled;
                        existingUser.AccessFailedCount = user.AccessFailedCount;
                        existingUser.UserName = user.UserName;
                        existingUser.FullName = user.FullName;

                        // Submit changes
                        _dataContext.SubmitChanges();
                    }
                    catch (ChangeConflictException)
                    {
                        // Resolve the conflict by refreshing the entity state
                        foreach (ObjectChangeConflict conflict in _dataContext.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepCurrentValues);
                        }

                        // Retry submitting changes
                        _dataContext.SubmitChanges();
                    }
                }
            }
        }

        //public void UpdateUserFullName(string id, string fullName)
        //{
        //    using (var transaction = new TransactionScope())
        //    {
        //        var existingUser = _dataContext.AspNetUsers.SingleOrDefault(u => u.Id == id);
        //        if (existingUser != null)
        //        {
        //            // Refresh before making changes
        //            _dataContext.Refresh(RefreshMode.OverwriteCurrentValues, existingUser);

        //            existingUser.FullName = fullName;
        //            _dataContext.SubmitChanges();
        //        }
        //        transaction.Complete();
        //    }
        //}

        public void UpdateUserFullName(AspNetUserDTO.UserDTO userModel)
        {
            //using (var transaction = new TransactionScope())
            //{
            //    var existingUser = _dataContext.AspNetUsers.SingleOrDefault(u => u.Id == userModel.Id);
            //    if (existingUser != null)
            //    {
            //        // Refresh before making changes
            //        _dataContext.Refresh(RefreshMode.OverwriteCurrentValues, existingUser);

            //        existingUser.FullName = userModel.FullName;
            //        existingUser.AspNetUserRoles.FirstOrDefault().RoleId = userModel.UserRoles.FirstOrDefault().RoleId;
            //        _dataContext.SubmitChanges();
            //    }
            //    transaction.Complete();
            //}
            var existingUser = _dataContext.AspNetUsers
                .FirstOrDefault(u => u.Id == userModel.Id);

            if (existingUser != null)
            {
                // ✅ Update only FullName
                existingUser.FullName = userModel.FullName;

                // ✅ Get existing roles for the user
                var existingRoles = _dataContext.AspNetUserRoles
                    .Where(r => r.UserId == userModel.Id)
                    .Select(r => r.RoleId)
                    .ToList();

                // ✅ Get new role IDs from userModel.UserRoles
                var newRoleIds = userModel.UserRoles.Select(ur => ur.RoleId).ToList();

                // ✅ Identify roles to add and remove
                var rolesToAdd = newRoleIds.Except(existingRoles).ToList();
                var rolesToRemove = existingRoles.Except(newRoleIds).ToList();

                // ✅ Add new roles
                foreach (var roleId in rolesToAdd)
                {
                    var userRole = new AspNetUserRole
                    {
                        UserId = userModel.Id,
                        RoleId = roleId
                    };
                    _dataContext.AspNetUserRoles.InsertOnSubmit(userRole);
                }

                // ✅ Remove roles that are no longer selected
                foreach (var roleId in rolesToRemove)
                {
                    var userRole = _dataContext.AspNetUserRoles
                        .FirstOrDefault(ur => ur.UserId == userModel.Id && ur.RoleId == roleId);
                    if (userRole != null)
                    {
                        _dataContext.AspNetUserRoles.DeleteOnSubmit(userRole);
                    }
                }

                // ✅ Save changes
                _dataContext.SubmitChanges();
            }

        }

        public IEnumerable<AspNetUserDTO.RoleDTO> GetRoles()
        {
            var data = _dataContext.AspNetRoles
                .ToList();
            var mapped = Mapping.Mapper.Map<List<AspNetUserDTO.RoleDTO>>(data);
            return mapped;
        }

        public void InsertRole(AspNetUserDTO.RoleDTO role)
        {
            role.Id = Guid.NewGuid().ToString().ToUpper();
            var data = Mapping.Mapper.Map<AspNetRole>(role);
            _dataContext.AspNetRoles.InsertOnSubmit(data);
            _dataContext.SubmitChanges();
        }

        public void DeleteRole(string roleId) { 
            var existingRole = _dataContext.AspNetRoles.Where(r => r.Id == roleId).FirstOrDefault();
            if (existingRole != null) { 
                _dataContext.AspNetRoles.DeleteOnSubmit(existingRole);
                _dataContext.SubmitChanges();
            }
        }

    }
}
