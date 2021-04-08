using AutoMapper;
using ProjectC.Core;
using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service.Interface;
using System;
using System.Collections.Generic;

namespace ProjectC.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="obj">CustomerDTO</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(CustomerDTO obj)
        {
            try
            {
                obj.RecordStatusId = 1;
                obj.CreatedDate = DateTime.Now;
                obj.CustomerId = Guid.NewGuid();

                var entity = _mapper.Map<Customer>(obj);

                var result = _uow.Customers.Add(entity);

                _uow.Commit();

                return Success(result.CustomerId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Updates the given customer.
        /// </summary>
        /// <param name="obj">.Customer to be updated</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(CustomerDTO obj)
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

                var entity = _mapper.Map<Customer>(obj);

                var result = _uow.Customers.Update(entity);

                _uow.Commit();

                return Success(result.CustomerId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Retrives the customer by id
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(Guid id)
        {
            try
            {
                var entity = _uow.Customers.Single(x => x.CustomerId == id);

                var result = _mapper.Map<CustomerDTO>(entity);

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
        /// Lists all customers.
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List()
        {
            try
            {
                var entities = _uow.Customers.GetAll();

                List<CustomerDTO> list = new List<CustomerDTO>();

                foreach (var item in entities)
                {
                    CustomerDTO obj = new CustomerDTO
                    {
                        CustomerId = item.CustomerId,
                        Name = item.Name,
                        CreatedDate = item.CreatedDate,
                        RecordStatusName = item.RecordStatus.RecordStatusName,
                        UserNameWs = item.UserNameWs,
                        
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
