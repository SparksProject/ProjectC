using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Chep.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUnitOfWork _uow;
        private IOptions<SparksConfig> _options;

        public UserService(IUnitOfWork uow, IOptions<SparksConfig> options)
        {
            _uow = uow;
            _options = options;
        }

        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List()
        {
            try
            {
                var entities = _uow.Users.Set().Include(x => x.UserType).Include(x => x.RecordStatus).ToList();

                var list = new List<UserDTO>();

                foreach (var item in entities)
                {
                    var obj = new UserDTO
                    {
                        UserId = item.UserId,
                        UserName = item.UserName,
                        RecordStatusName = item.RecordStatus.RecordStatusName,
                        UserTypeName = item.UserType.UserTypeName,
                        CreatedDate = item.CreatedDate
                    };

                    list.Add(obj);
                }

                return Success(list);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(int id)
        {
            try
            {
                var entity = _uow.Users.Set()
                                       .Include(x => x.UserType)
                                       .Include(x => x.UserCustomer)
                                       .Include(x => x.UserPermission)
                                       .Include(x => x.RecordStatus)
                                       .Include(x => x.CreatedByNavigation)
                                       .Include(x => x.ModifiedByNavigation)
                                       .Include(x => x.DeletedByNavigation)
                                       .FirstOrDefault(x => x.UserId == id);

                var target = Mapper.MapSingle<User, UserDTO>(entity);
                target.UserTypeName = entity.UserType.UserTypeName;
                target.RecordStatusName = entity.RecordStatus.RecordStatusName;
                target.ModifiedByName = entity.ModifiedByNavigation?.UserName;
                target.DeletedByName = entity.DeletedByNavigation?.UserName;

                var userCustomers = entity.UserCustomer;
                if (userCustomers != null && userCustomers.Count > 0)
                {
                    target.UserCustomerList = new List<UserCustomerDTO>();
                    target.CustomerIdList = new List<Guid>();

                    foreach (var item in userCustomers.Where(x => x != null))
                    {
                        target.UserCustomerList.Add(new UserCustomerDTO { CustomerName = item.Customer.Name, });
                        target.CustomerIdList.Add(item.CustomerId);
                    }
                }

                target.UserPermissions = Mapper.MapSingle<UserPermission, UserPermissionDTO>(entity.UserPermission.FirstOrDefault());

                return Success(target);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="obj">User to be created</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(UserDTO obj)
        {
            try
            {
                if (obj == null)
                {
                    return Warning("Sorgu başarıyla ulaşmadı!");
                }

                if (obj.UserPermissions == null)
                {
                    obj.UserPermissions = new UserPermissionDTO();
                }

                obj.RecordStatusId = 1;
                obj.UserName = obj.EmailAddress;
                obj.Password = new Random().Next(12563, 98453).ToString();
                //obj.Password = Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");
                obj.CreatedDate = DateTime.Now;

                var entity = Mapper.MapSingle<UserDTO, User>(obj);

                entity.UserPermission = new List<UserPermission>();

                if (obj.UserTypeId == Convert.ToByte(Enums.UserType.Admin))
                {
                    entity.UserPermission.Add(new UserPermission
                    {
                        CompanyEdit = true,
                        CustomerEdit = true,
                        CustomerAdd = true,
                        CustomerGet = true,
                        CustomerList = true,
                        GenericReportAdd = true,
                        GenericReportEdit = true,
                        GenericReportExecute = true,
                        GenericReportGet = true,
                        GenericReportList = true,
                        MailDefinitionAdd = true,
                        MailDefinitionEdit = true,
                        MailDefinitionGert = true,
                        MailDefinitionList = true,
                        MailReportAdd = true,
                        MailReportEdit = true,
                        MailReportGet = true,
                        MailReportList = true,
                        ProductAdd = true,
                        ProductEdit = true,
                        ProductGet = true,
                        ProductList = true,
                        UserAdd = true,
                        UserEdit = true,
                        UserGet = true,
                        UserList = true,
                        CreatedDate = obj.CreatedDate,
                    });
                }
                else
                {
                    entity.UserPermission.Add(new UserPermission
                    {
                        CompanyEdit = obj.UserPermissions.CompanyEdit,
                        CustomerEdit = obj.UserPermissions.CustomerEdit,
                        CreatedDate = obj.CreatedDate,
                        CustomerAdd = obj.UserPermissions.CustomerAdd,
                        CustomerGet = obj.UserPermissions.CustomerGet,
                        CustomerList = obj.UserPermissions.CustomerList,
                        GenericReportAdd = obj.UserPermissions.GenericReportAdd,
                        GenericReportEdit = obj.UserPermissions.GenericReportEdit,
                        GenericReportExecute = obj.UserPermissions.GenericReportExecute,
                        GenericReportGet = obj.UserPermissions.GenericReportGet,
                        GenericReportList = obj.UserPermissions.GenericReportList,
                        MailDefinitionAdd = obj.UserPermissions.MailDefinitionAdd,
                        MailDefinitionEdit = obj.UserPermissions.MailDefinitionEdit,
                        MailDefinitionGert = obj.UserPermissions.MailDefinitionGert,
                        MailDefinitionList = obj.UserPermissions.MailDefinitionList,
                        MailReportAdd = obj.UserPermissions.MailReportAdd,
                        MailReportEdit = obj.UserPermissions.MailReportEdit,
                        MailReportGet = obj.UserPermissions.MailReportGet,
                        MailReportList = obj.UserPermissions.MailReportList,
                        ProductAdd = obj.UserPermissions.ProductAdd,
                        ProductEdit = obj.UserPermissions.ProductEdit,
                        ProductGet = obj.UserPermissions.ProductGet,
                        ProductList = obj.UserPermissions.ProductList,
                        UserAdd = obj.UserPermissions.UserAdd,
                        UserEdit = obj.UserPermissions.UserEdit,
                        UserGet = obj.UserPermissions.UserGet,
                        UserList = obj.UserPermissions.UserList,
                        StokGirisAdd = obj.UserPermissions.StokGirisAdd,
                        StokGirisEdit = obj.UserPermissions.StokGirisEdit,
                        StokGirisGet = obj.UserPermissions.StokGirisGet,
                        StokGirisList = obj.UserPermissions.StokGirisList,
                        StokCikisAdd = obj.UserPermissions.StokCikisAdd,
                        StokCikisEdit = obj.UserPermissions.StokCikisEdit,
                        StokCikisGet = obj.UserPermissions.StokCikisGet,
                        StokCikisList = obj.UserPermissions.StokCikisList,
                    });
                }

                var result = _uow.Users.Add(entity);

                _uow.Commit();

                return Success(result.UserId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Update the given user.
        /// </summary>
        /// <param name="obj">User to be updated</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(UserDTO obj)
        {
            try
            {
                var now = DateTime.Now;

                if (obj.RecordStatusId == 1)
                {
                    obj.ModifiedDate = now;
                }
                else
                {
                    obj.DeletedDate = now;

                }

                var entity = Mapper.MapSingle<UserDTO, User>(obj);
                var permissions = Mapper.MapSingle<UserPermissionDTO, UserPermission>(obj.UserPermissions);

                _uow.UserCustomers.Delete(x => x.UserId == obj.UserId);
                if (obj.CustomerIdList != null)
                {
                    foreach (var item in obj.CustomerIdList.Where(x => x != null))
                    {
                        _uow.UserCustomers.Add(new UserCustomer { CustomerId = item, UserId = obj.UserId });
                    }
                }

                if (_uow.UserPermissions.Any(x => x.UserId == obj.UserId))
                {
                    permissions.ModifiedDate = now;

                    _uow.UserPermissions.Update(permissions);
                }
                else
                {
                    permissions.UserId = obj.UserId;

                    _uow.UserPermissions.Add(permissions);
                }

                _uow.Users.Update(entity);

                _uow.Commit();

                return Success(obj.UserId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Retrives the user by id
        /// </summary>
        /// <param name="token">Token string</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(string token, string secret)
        {
            try
            {
                //var entity = _uow.Users.Single(x => x.UserId == id);

                //var result = Mapper.MapSingle<User, UserDTO>(entity);
                //result.CreatedByName = entity.CreatedByNavigation.FirstName + " " + entity.CreatedByNavigation.LastName;
                //result.ModifiedByName = entity.ModifiedBy != null ? entity.ModifiedByNavigation.FirstName + " " + entity.ModifiedByNavigation.LastName : null;
                //result.DeletedByName = entity.DeletedBy != null ? entity.DeletedByNavigation.FirstName + " " + entity.DeletedByNavigation.LastName : null;

                token = token.Replace("Bearer ", "");
                //string secret = "EA5257A1-66C1-44A7-A31F-43BECFFFB2C7";
                var key = Encoding.ASCII.GetBytes(secret);
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                int userId = Convert.ToInt32(claims.FindFirst(x => x.Type == "UserId").Value);
                string fullName = claims.FindFirst(x => x.Type == "FullName").Value;

                return new ResponseDTO
                {
                    IsSuccesful = true,
                    Result = new UserDTO
                    {
                        UserId = userId,
                        FirstName = claims.FindFirst(x => x.Type == "FirstName").Value,
                        LastName = claims.FindFirst(x => x.Type == "LastName").Value,
                    },
                    ResultMessage = Enums.ResponseMessage.OK
                };
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO Authenticate(string userName, string password)
        {
            try
            {
                var entity = _uow.Users.Set()
                                       .Include(x => x.UserPermission)
                                       .FirstOrDefault(x => x.UserName == userName && x.Password == password && x.RecordStatusId == 1);

                if (entity == null)
                {
                    return NotFound();
                }

                var result = Mapper.MapSingle<User, UserDTO>(entity);

                result.UserPermissions = Mapper.MapSingle<UserPermission, UserPermissionDTO>(entity.UserPermission.FirstOrDefault());
                result.CompanyId = _uow.Companies.GetAll().FirstOrDefault().CompanyId;

                var secretKey = _options.Value.TokenManagement.Secret;
                var secretKeyByteArray = Encoding.ASCII.GetBytes(secretKey);
                var expiryDuration = _options.Value.TokenManagement.ExpiryDuration;

                var claims = new List<Claim>
                {
                    new Claim("UserId", result.UserId.ToString()),
                    new Claim("FullName", result.FullName),
                    new Claim("CompanyId", result.CompanyId.ToString()),
                    new Claim("UserCustomerList", JsonConvert.SerializeObject(result.UserCustomerList)),
                };

                if (result.UserPermissions != null)
                {
                    claims.Add(new Claim("CompanyEdit", $"{result.UserPermissions.CompanyEdit}"));
                    claims.Add(new Claim("CustomerEdit", $"{result.UserPermissions.CustomerEdit}"));
                    claims.Add(new Claim("CustomerAdd", $"{result.UserPermissions.CustomerAdd}"));
                    claims.Add(new Claim("CustomerGet", $"{result.UserPermissions.CustomerGet}"));
                    claims.Add(new Claim("CustomerList", $"{result.UserPermissions.CustomerList}"));
                    claims.Add(new Claim("GenericReportAdd", $"{result.UserPermissions.GenericReportAdd}"));
                    claims.Add(new Claim("GenericReportEdit", $"{result.UserPermissions.GenericReportEdit}"));
                    claims.Add(new Claim("GenericReportExecute", $"{result.UserPermissions.GenericReportExecute}"));
                    claims.Add(new Claim("GenericReportGet", $"{result.UserPermissions.GenericReportGet}"));
                    claims.Add(new Claim("GenericReportList", $"{result.UserPermissions.GenericReportList}"));
                    claims.Add(new Claim("MailDefinitionAdd", $"{result.UserPermissions.MailDefinitionAdd}"));
                    claims.Add(new Claim("MailDefinitionEdit", $"{result.UserPermissions.MailDefinitionEdit}"));
                    claims.Add(new Claim("MailDefinitionGert", $"{result.UserPermissions.MailDefinitionGert}"));
                    claims.Add(new Claim("MailDefinitionList", $"{result.UserPermissions.MailDefinitionList}"));
                    claims.Add(new Claim("ProductAdd", $"{result.UserPermissions.ProductAdd}"));
                    claims.Add(new Claim("ProductEdit", $"{result.UserPermissions.ProductEdit}"));
                    claims.Add(new Claim("ProductGet", $"{result.UserPermissions.ProductGet}"));
                    claims.Add(new Claim("ProductList", $"{result.UserPermissions.ProductList}"));
                    claims.Add(new Claim("UserAdd", $"{result.UserPermissions.UserAdd}"));
                    claims.Add(new Claim("UserEdit", $"{result.UserPermissions.UserEdit}"));
                    claims.Add(new Claim("UserGet", $"{result.UserPermissions.UserGet}"));
                    claims.Add(new Claim("UserList", $"{result.UserPermissions.UserList}"));
                    claims.Add(new Claim("MailReportList", $"{result.UserPermissions.MailReportList}"));
                    claims.Add(new Claim("MailReportGet", $"{result.UserPermissions.MailReportGet}"));
                    claims.Add(new Claim("MailReportEdit", $"{result.UserPermissions.MailReportEdit}"));
                    claims.Add(new Claim("MailReportAdd", $"{result.UserPermissions.MailReportAdd}"));
                    claims.Add(new Claim("StokGirisAdd", $"{result.UserPermissions.StokGirisAdd}"));
                    claims.Add(new Claim("StokGirisEdit", $"{result.UserPermissions.StokGirisEdit}"));
                    claims.Add(new Claim("StokGirisGet", $"{result.UserPermissions.StokGirisGet}"));
                    claims.Add(new Claim("StokGirisList", $"{result.UserPermissions.StokGirisList}"));
                    claims.Add(new Claim("StokCikisAdd", $"{result.UserPermissions.StokCikisAdd}"));
                    claims.Add(new Claim("StokCikisEdit", $"{result.UserPermissions.StokCikisEdit}"));
                    claims.Add(new Claim("StokCikisGet", $"{result.UserPermissions.StokCikisGet}"));
                    claims.Add(new Claim("StokCikisList", $"{result.UserPermissions.StokCikisList}"));
                }

                var now = DateTime.UtcNow;

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByteArray), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "Chep",
                    Audience = null,
                    IssuedAt = now,
                    NotBefore = now,
                    Expires = now.AddMinutes(expiryDuration),
                    Subject = new ClaimsIdentity(claims),
                };

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                var token = jwtTokenHandler.WriteToken(jwtToken);
                result.Token = token;

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                token = token.Replace("Bearer ", "");
                var key = Encoding.ASCII.GetBytes(_options.Value.TokenManagement.Secret);
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken securityToken;

                var claims = handler.ValidateToken(token, validations, out securityToken);

                var result = claims.Identity.IsAuthenticated;

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}