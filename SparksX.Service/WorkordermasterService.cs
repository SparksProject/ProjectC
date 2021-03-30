using AutoMapper;
using SparksX.Data.Models;
using SparksX.Data.Repository;
using SparksX.DTO;
using SparksX.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SparksX.Service
{
    public class WorkordermasterService : BaseService, IWorkordermasterService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public WorkordermasterService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new workorder.
        /// </summary>
        /// <param name="obj">Workorder to be created</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(WorkOrderMasterDTO obj)
        {
            try
            {
                obj.RecordStatusId = 1;

                var entity = _mapper.Map<WorkOrderMaster>(obj);

                var result = _uow.WorkOrderMasters.Add(entity);

                return Success(result.WorkOrderMasterId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Updates the given workorder.
        /// </summary>
        /// <param name="obj">.Workorder to be updated</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(WorkOrderMasterDTO obj)
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

                var entity = _mapper.Map<WorkOrderMaster>(obj);

                var result = _uow.WorkOrderMasters.Update(entity);

                return Success(result.WorkOrderMasterId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Retrives the workorder by id
        /// </summary>
        /// <param name="id">WorkorderId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(Guid id)
        {
            try
            {
                var entity = _uow.WorkOrderMasters.Single(x => x.WorkOrderMasterId == id);

                var result = _mapper.Map<WorkOrderMasterDTO>(entity);

                result.CustomerName = entity.Customer.Name;
                result.RecordStatusName = entity.RecordStatus.RecordStatusName;
                result.CreatedByName = entity.CreatedByNavigation.FirstName + " " + entity.CreatedByNavigation.LastName;
                result.ModifiedByName = entity.ModifiedBy != null ? entity.ModifiedByNavigation.FirstName + " " + entity.ModifiedByNavigation.LastName : null;
                result.DeletedByName = entity.DeletedBy != null ? entity.DeletedByNavigation.FirstName + " " + entity.DeletedByNavigation.LastName : null;

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Lists all workorders.
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List()
        {
            try
            {
                var entities = _uow.WorkOrderMasters.GetAll();

                if (entities.Count == 0)
                {
                    return NotFound();
                }

                List<WorkOrderMasterDTO> list = new List<WorkOrderMasterDTO>();

                foreach (var item in entities)
                {
                    WorkOrderMasterDTO obj = new WorkOrderMasterDTO
                    {
                        WorkOrderMasterId = item.WorkOrderMasterId,
                        WorkOrderNo = item.WorkOrderNo,
                        DeclarationType = item.DeclarationType,
                        Status = item.Status,
                        MasterId = item.MasterId,
                        CustomerName = item.Customer.Name,
                        Message = item.Message,
                        CreatedDate = item.CreatedDate,
                        RecordStatusName = item.RecordStatus.RecordStatusName
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
    }
}
