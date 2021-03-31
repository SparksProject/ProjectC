using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using ProjectC.Data.Models;
using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service.Interface;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace ProjectC.Service
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
                var entities = _uow.Users.GetAll();

                List<UserDTO> list = new List<UserDTO>();

                foreach (var item in entities)
                {
                    UserDTO obj = new UserDTO
                    {
                        UserId = item.UserId,
                        UserName = item.UserName,
                        RecordStatusName = item.RecordStatusId == 1 ? "Aktif" : "Pasif",
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
        public ResponseDTO GetUser(int id)
        {
            try
            {
                var entity = _uow.Users.Single(x => x.UserId == id);

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
                        WorkOrderMasterAdd = true,
                        WorkOrderMasterEdit = true,
                        WorkOrderMasterGet = true,
                        WorkOrderMasterList = true,
                        ProductAdd = true,
                        ProductEdit = true,
                        ProductGet = true,
                        ProductList = true,
                        UserAdd = true,
                        UserEdit = true,
                        UserGet = true,
                        UserList = true,
                        EvrimArchiveList = true,
                        BeyannameList = true,
                        SparksArchiveList = true,
                        SparksArchiveImport = true,
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
                        WorkOrderMasterAdd = obj.UserPermissions.WorkOrderMasterAdd,
                        WorkOrderMasterEdit = obj.UserPermissions.WorkOrderMasterEdit,
                        WorkOrderMasterGet = obj.UserPermissions.WorkOrderMasterGet,
                        WorkOrderMasterList = obj.UserPermissions.WorkOrderMasterList,
                        ProductAdd = obj.UserPermissions.ProductAdd,
                        ProductEdit = obj.UserPermissions.ProductEdit,
                        ProductGet = obj.UserPermissions.ProductGet,
                        ProductList = obj.UserPermissions.ProductList,
                        UserAdd = obj.UserPermissions.UserAdd,
                        UserEdit = obj.UserPermissions.UserEdit,
                        UserGet = obj.UserPermissions.UserGet,
                        UserList = obj.UserPermissions.UserList,
                        SparksArchiveImport = obj.UserPermissions.SparksArchiveImport,
                        SparksArchiveList = obj.UserPermissions.SparksArchiveList,
                        StokGirisAdd = obj.UserPermissions.StokGirisAdd,
                        StokGirisEdit = obj.UserPermissions.StokGirisEdit,
                        StokGirisGet = obj.UserPermissions.StokGirisGet,
                        StokGirisList = obj.UserPermissions.StokGirisList,
                        StokCikisAdd = obj.UserPermissions.StokCikisAdd,
                        StokCikisEdit = obj.UserPermissions.StokCikisEdit,
                        StokCikisGet = obj.UserPermissions.StokCikisGet,
                        StokCikisList = obj.UserPermissions.StokCikisList,
                        BeyannameList = obj.UserPermissions.BeyannameList,
                        EvrimArchiveList = obj.UserPermissions.EvrimArchiveList
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
                var entity = _uow.Users.Single(x => x.UserName == userName && x.Password == password && x.RecordStatusId == 1);

                if (entity == null)
                {
                    return NotFound();
                }

                var userCustomer = _uow.UserCustomers.Search(x => x.UserId == entity.UserId);

                var result = Mapper.MapSingle<User, UserDTO>(entity);

                if (userCustomer != null && userCustomer.Count > 0)
                {
                    result.UserCustomerList = new List<UserCustomerDTO>();

                    foreach (var item in userCustomer)
                    {
                        result.UserCustomerList.Add(new UserCustomerDTO
                        {
                            CustomerId = item.CustomerId,
                            CustomerName = item.Customer.Name,
                        });
                    }
                }

                // Şubat sonu (2021) çalışmasın program BAŞLANGIÇ
                //var nistDate = GetNistDate();

                //if (nistDate == null || nistDate.Value > new DateTime(2021, 03, 01))
                //{
                //    return Unauthorized();
                //}
                // Şubat sonu (2021) çalışmasın program SONU

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
                    new Claim("UserCustomerList", JsonConvert.SerializeObject(result.UserCustomerList)), // 
                };

                if (result.UserPermissions != null)
                {
                    claims.Add(new Claim("CompanyEdit", result.UserPermissions.CompanyEdit.ToString()));
                    claims.Add(new Claim("CustomerEdit", result.UserPermissions.CustomerEdit.ToString()));
                    claims.Add(new Claim("CustomerAdd", result.UserPermissions.CustomerAdd.ToString()));
                    claims.Add(new Claim("CustomerGet", result.UserPermissions.CustomerGet.ToString()));
                    claims.Add(new Claim("CustomerList", result.UserPermissions.CustomerList.ToString()));
                    claims.Add(new Claim("GenericReportAdd", result.UserPermissions.GenericReportAdd.ToString()));
                    claims.Add(new Claim("GenericReportEdit", result.UserPermissions.GenericReportEdit.ToString()));
                    claims.Add(new Claim("GenericReportExecute", result.UserPermissions.GenericReportExecute.ToString()));
                    claims.Add(new Claim("GenericReportGet", result.UserPermissions.GenericReportGet.ToString()));
                    claims.Add(new Claim("GenericReportList", result.UserPermissions.GenericReportList.ToString()));
                    claims.Add(new Claim("MailDefinitionAdd", result.UserPermissions.MailDefinitionAdd.ToString()));
                    claims.Add(new Claim("MailDefinitionEdit", result.UserPermissions.MailDefinitionEdit.ToString()));
                    claims.Add(new Claim("MailDefinitionGert", result.UserPermissions.MailDefinitionGert.ToString()));
                    claims.Add(new Claim("MailDefinitionList", result.UserPermissions.MailDefinitionList.ToString()));
                    claims.Add(new Claim("ProductAdd", result.UserPermissions.ProductAdd.ToString()));
                    claims.Add(new Claim("ProductEdit", result.UserPermissions.ProductEdit.ToString()));
                    claims.Add(new Claim("ProductGet", result.UserPermissions.ProductGet.ToString()));
                    claims.Add(new Claim("ProductList", result.UserPermissions.ProductList.ToString()));
                    claims.Add(new Claim("UserAdd", result.UserPermissions.UserAdd.ToString()));
                    claims.Add(new Claim("UserEdit", result.UserPermissions.UserEdit.ToString()));
                    claims.Add(new Claim("UserGet", result.UserPermissions.UserGet.ToString()));
                    claims.Add(new Claim("UserList", result.UserPermissions.UserList.ToString()));
                    claims.Add(new Claim("WorkOrderMasterList", result.UserPermissions.WorkOrderMasterList.ToString()));
                    claims.Add(new Claim("WorkOrderMasterGet", result.UserPermissions.WorkOrderMasterGet.ToString()));
                    claims.Add(new Claim("WorkOrderMasterEdit", result.UserPermissions.WorkOrderMasterEdit.ToString()));
                    claims.Add(new Claim("WorkOrderMasterAdd", result.UserPermissions.WorkOrderMasterAdd.ToString()));
                    claims.Add(new Claim("MailReportList", result.UserPermissions.MailReportList.ToString()));
                    claims.Add(new Claim("MailReportGet", result.UserPermissions.MailReportGet.ToString()));
                    claims.Add(new Claim("MailReportEdit", result.UserPermissions.MailReportEdit.ToString()));
                    claims.Add(new Claim("MailReportAdd", result.UserPermissions.MailReportAdd.ToString()));
                    claims.Add(new Claim("SparksArchiveList", result.UserPermissions.SparksArchiveList.ToString()));
                    claims.Add(new Claim("SparksArchiveImport", result.UserPermissions.SparksArchiveImport.ToString()));
                    claims.Add(new Claim("EvrimArchiveList", result.UserPermissions.EvrimArchiveList.ToString()));
                    claims.Add(new Claim("BeyannameList", result.UserPermissions.BeyannameList.ToString()));
                    claims.Add(new Claim("StokGirisAdd", result.UserPermissions.StokGirisAdd.ToString()));
                    claims.Add(new Claim("StokGirisEdit", result.UserPermissions.StokGirisEdit.ToString()));
                    claims.Add(new Claim("StokGirisGet", result.UserPermissions.StokGirisGet.ToString()));
                    claims.Add(new Claim("StokGirisList", result.UserPermissions.StokGirisList.ToString()));
                    claims.Add(new Claim("StokCikisAdd", result.UserPermissions.StokCikisAdd.ToString()));
                    claims.Add(new Claim("StokCikisEdit", result.UserPermissions.StokCikisEdit.ToString()));
                    claims.Add(new Claim("StokCikisGet", result.UserPermissions.StokCikisGet.ToString()));
                    claims.Add(new Claim("StokCikisList", result.UserPermissions.StokCikisList.ToString()));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByteArray), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "SparksX",
                    Audience = null,
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
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

        public DateTime? GetNistDate()
        {
            try
            {
                var ipList = new List<string> { "time-a-g.nist.gov",
                                        "time-b-g.nist.gov",
                                        "time-c-g.nist.gov",
                                        "time-d-g.nist.gov",
                                        "time-d-g.nist.gov",
                                        "time-e-g.nist.gov",
                                        "time-e-g.nist.gov",
                                        "time-a-wwv.nist.gov",
                                        "time-b-wwv.nist.gov",
                                        "time-c-wwv.nist.gov",
                                        "time-d-wwv.nist.gov",
                                        "time-d-wwv.nist.gov",
                                        "time-e-wwv.nist.gov",
                                        "time-e-wwv.nist.gov",
                                        "time-a-b.nist.gov",
                                        "time-b-b.nist.gov",
                                        "time-c-b.nist.gov",
                                        "time-d-b.nist.gov",
                                        "time-d-b.nist.gov",
                                        "time-e-b.nist.gov",
                                        "time-e-b.nist.gov",
                                        "time.nist.gov",
                                        "utcnist.colorado.edu",
                                        "utcnist2.colorado.edu"
            };

                foreach (var item in ipList)
                {
                    try
                    {
                        using (var client = new TcpClient(item, 13)) // time.nist.gov
                        using (var streamReader = new StreamReader(client.GetStream()))
                        {
                            var response = streamReader.ReadToEnd();
                            var utcDateTimeString = response.Substring(7, 17);
                            var localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

                            return localDateTime;
                        }
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }

                return default;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}