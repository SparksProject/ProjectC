using Microsoft.Extensions.Options;

using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Chep.Service
{
    /// <summary>
    /// Generic Report service class
    /// </summary>
    public class GenericReportService : BaseService, IGenericReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IOptions<SparksConfig> _sparksConfig;
        private readonly IMailReportService _mailReportService;

        public GenericReportService(IUnitOfWork uow, IOptions<SparksConfig> sparksConfig, IMailReportService mailReportService)
        {
            _uow = uow;
            _sparksConfig = sparksConfig;
            _mailReportService = mailReportService;
        }

        /// <summary>
        /// Add a new Generic Report.
        /// </summary>
        /// <param name="obj">Generic Report object</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(GenericReportDTO obj)
        {
            try
            {
                obj.RecordStatusId = 1;
                obj.CreatedDate = DateTime.Now;
                var entity = Mapper.MapSingle<GenericReportDTO, GenericReport>(obj);

                entity.GenericReportUser = new List<GenericReportUser>();

                if (obj.UserList != null && !obj.IsDefaultReport)
                {
                    foreach (var item in obj.UserList)
                    {
                        entity.GenericReportUser.Add(new GenericReportUser
                        {
                            UserId = item
                        });
                    }
                }

                if (obj.GenericReportParameterList != null && obj.GenericReportParameterList.Count > 0)
                {
                    entity.GenericReportParameter = new List<GenericReportParameter>();

                    foreach (var item in obj.GenericReportParameterList)
                    {
                        entity.GenericReportParameter.Add(new GenericReportParameter
                        {
                            GenericReportParameterName = item.GenericReportParameterName,
                            ParameterLabel = item.ParameterLabel,
                            ParameterType = item.ParameterType,
                        });
                    }
                }

                var result = _uow.GenericReports.Add(entity);

                bool saveResult = _uow.Commit();

                return Success(result.GenericReportId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Edits the given Generic Report object.
        /// </summary>
        /// <param name="obj">Generic Report object</param>
        /// <returns></returns>
        public ResponseDTO Edit(GenericReportDTO obj)
        {
            try
            {
                if (obj.RecordStatusId == 1)
                {
                    obj.ModifiedDate = DateTime.Now;
                }
                else
                {
                    obj.DeletedDate = DateTime.Now;
                }

                _uow.GenericReportUsers.Delete(x => x.GenericReportId == obj.GenericReportId);

                _uow.GenericReportParameters.Delete(x => x.GenericReportId == obj.GenericReportId);

                if (obj.UserList != null && !obj.IsDefaultReport)
                {
                    foreach (var item in obj.UserList)
                    {
                        _uow.GenericReportUsers.Add(new GenericReportUser
                        {
                            GenericReportId = obj.GenericReportId,
                            UserId = item
                        });
                    }
                }

                if (obj.GenericReportParameterList != null && obj.GenericReportParameterList.Count > 0)
                {
                    foreach (var item in obj.GenericReportParameterList)
                    {
                        if (!string.IsNullOrEmpty(item.GenericReportParameterName) && !string.IsNullOrEmpty(item.ParameterLabel))
                        {
                            _uow.GenericReportParameters.Add(new GenericReportParameter
                            {
                                GenericReportId = obj.GenericReportId,
                                GenericReportParameterName = item.GenericReportParameterName,
                                ParameterLabel = item.ParameterLabel,
                                ParameterType = item.ParameterType,
                            });
                        }
                    }
                }

                var entity = Mapper.MapSingle<GenericReportDTO, GenericReport>(obj);

                var result = _uow.GenericReports.Update(entity);

                bool saveResult = _uow.Commit();

                return Success(result.GenericReportId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Gets given Generic Report object.
        /// </summary>
        /// <param name="id">GenericReportId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(int id)
        {
            try
            {
                var entity = _uow.GenericReports.Set()
                                                .Include(x => x.RecordStatus)
                                                .Include(x => x.GenericReportParameter)
                                                .Include(x => x.GenericReportUser)
                                                .ThenInclude(x => x.User)
                                                .Single(x => x.GenericReportId == id);

                var result = Mapper.MapSingle<GenericReport, GenericReportDTO>(entity);
                result.RecordStatusName = entity.RecordStatus.RecordStatusName;

                // ModifiedByName ve DeletedByName ataması yapılınca burası silinecek.
                result.ModifiedDate = null;
                result.ModifiedBy = null;
                result.DeletedBy = null;
                result.DeletedDate = null;

                result.UserList = entity.GenericReportUser.Select(x => x.UserId).ToList();
                result.GenericReportParameterList = entity.GenericReportParameter.Select(x => new GenericReportParameterDTO
                {
                    GenericReportParameterName = x.GenericReportParameterName,
                    ParameterLabel = x.ParameterLabel,
                    ParameterType = x.ParameterType,
                }).ToList();
                result.GenericReportUserList = entity.GenericReportUser.Select(x => new GenericReportUserDTO
                {
                    GenericReportId = x.GenericReportId,
                    GenericReportUserId = x.GenericReportUserId,
                    UserId = x.UserId,
                    UserName = x.User?.UserName
                }).ToList();

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Lists the user allowed Generic Reports.
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List(int id)
        {
            try
            {
                var entities = _uow.GenericReports.Set()
                                                  .Include(x => x.RecordStatus)
                                                  .Where(x => x.GenericReportUser.Any(y => y.UserId == id) || x.IsDefaultReport)
                                                  .ToList();

                if (entities.Count == 0)
                {
                    return NotFound();
                }

                var target = new List<GenericReportDTO>();

                foreach (var item in entities)
                {
                    var obj = new GenericReportDTO
                    {
                        GenericReportId = item.GenericReportId,
                        GenericReportName = item.GenericReportName,
                        RecordStatusName = item.RecordStatus.RecordStatusName,
                        CreatedDate = item.CreatedDate,
                        IsDefaultReport = item.IsDefaultReport,
                    };

                    target.Add(obj);
                }

                return Success(target);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Executes the sql script and shows the report.
        /// </summary>
        /// <param name="id">GenericReportId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO GetResultSet(int id, int userId, List<GenericReportParameterDTO> parameters)
        {
            try
            {
                if (userId < 1)
                {
                    return Unauthorized();
                }

                var entity = _uow.GenericReports.Set()
                            .Include(x => x.GenericReportParameter)
                            .Include(x => x.GenericReportUser)
                            .FirstOrDefault(x => x.GenericReportId == id);

                if (entity == null)
                {
                    return NotFound();
                }

                if (parameters != null && entity.GenericReportParameter.Count() != 0)
                {
                    foreach (var item in parameters)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        var parameter = entity.GenericReportParameter.FirstOrDefault(x => x.GenericReportParameterName == item.GenericReportParameterName);
                        if (parameter != null)
                        {
                            if (item.ParameterType == Convert.ToInt16(Enums.ParameterType.Tarih) && DateTime.TryParse(item.GenericReportParameterValue, out DateTime parsedDate))
                            {
                                var valueDate = $"CAST('{parsedDate:yyyy-MM-dd}' as date)";
                                entity.SqlScript = entity.SqlScript.Replace(parameter.GenericReportParameterName, valueDate);
                            }
                            else
                            {
                                entity.SqlScript = entity.SqlScript.Replace(parameter.GenericReportParameterName, item.GenericReportParameterValue);
                            }
                        }
                    }
                }

                try
                {
                    using (var sqlDataAdapter = new SqlDataAdapter(entity.SqlScript, _sparksConfig.Value.ConnectionString))
                    using (DataTable dataTable = new DataTable())
                    {
                        sqlDataAdapter.Fill(dataTable);

                        var result = dataTable.ToDynamicList(userId);

                        return Success(result);
                    }
                }
                catch (SqlException ex)
                {
                    return Warning(ex.Message);
                }
            }
            catch (ArgumentException ex) // ToDynamicList metodunda kullanılıyor!
            {
                return Warning(ex.Message);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO CreateExcel(int id, List<GenericReportParameterDTO> parameters)
        {
            var entity = _uow.GenericReports.Single(x => x.GenericReportId == id);

            if (parameters != null && entity.GenericReportParameter.Count() != 0)
            {
                foreach (var item in parameters)
                {
                    var parameter = entity.GenericReportParameter.FirstOrDefault(x => x.GenericReportParameterName == item.GenericReportParameterName);

                    if (parameter != null)
                    {
                        entity.SqlScript = entity.SqlScript.Replace(item.GenericReportParameterName, item.GenericReportParameterValue);
                    }
                }
            }

            var dataset = _mailReportService.GetScriptData(entity.SqlScript).Result as DataSet;

            if (dataset.Tables.Count == 0)
            {
                return NotFound();
            }

            var excelData = _mailReportService.SaveExcel(dataset).Result as byte[];

            return Success(excelData);
        }
    }
}