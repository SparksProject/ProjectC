using AutoMapper;

using SparksX.Data.Models;
using SparksX.Data.Repository;
using SparksX.DTO;
using SparksX.Service.Interface;

using System;
using System.Collections.Generic;
using System.Data;

namespace SparksX.Service
{
    public class StokGirisService : BaseService, IStokGirisService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StokGirisService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="obj">Product to be created</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(ChepStokGirisDTO obj)
        {
            try
            {
                //obj.RecordStatusId = 1;
                //obj.CreatedDate = DateTime.Now;

                var entity = _mapper.Map<ChepStokGiris>(obj);

                var result = _uow.ChepStokGiris.Add(entity);

                _uow.Commit();

                return Success(result.StokGirisId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Updates the given product.
        /// </summary>
        /// <param name="obj">Product to be updated</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(ChepStokGirisDTO obj)
        {
            try
            {
                //if (obj.RecordStatusId == 1)
                //{
                //    obj.ModifiedDate = DateTime.Now;
                //}
                //else
                //{
                //    obj.DeletedDate = DateTime.Now;
                //}

                var entity = _mapper.Map<ChepStokGiris>(obj);

                var result = _uow.ChepStokGiris.Update(entity);

                _uow.Commit();

                return Success(result.StokGirisId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Retrives the product by id
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(int id)
        {
            try
            {
                var entity = _uow.ChepStokGiris.Single(x => x.StokGirisId == id);

                var result = _mapper.Map<ChepStokGirisDTO>(entity);

                //result.CustomerName = entity.Customer.Name;
                //result.RecordStatusName = entity.RecordStatus.RecordStatusName;
                //result.CreatedByName = entity.CreatedByNavigation.FirstName + " " + entity.CreatedByNavigation.LastName;
                //result.ModifiedByName = entity.ModifiedBy != null ? entity.ModifiedByNavigation.FirstName + " " + entity.ModifiedByNavigation.LastName : null;
                //result.DeletedByName = entity.DeletedBy != null ? entity.DeletedByNavigation.FirstName + " " + entity.DeletedByNavigation.LastName : null;

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Lists all products.
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List()
        {
            try
            {
                var entities = _uow.ChepStokGiris.GetAll();

                if (entities.Count == 0)
                {
                    return NotFound();
                }

                var list = new List<ChepStokGirisDTO>();

                foreach (var item in entities)
                {
                    var obj = new ChepStokGirisDTO
                    {
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