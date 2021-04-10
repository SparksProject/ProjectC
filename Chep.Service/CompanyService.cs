using AutoMapper;

using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace Chep.Service
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// Updates the given company.
        /// </summary>
        /// <param name="obj">.Company to be updated</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(CompanyDTO obj)
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

                var entity = _mapper.Map<Company>(obj);

                var result = _uow.Companies.Update(entity);

                _uow.Commit();

                return Success(result.CompanyId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Retrives the company by id
        /// </summary>
        /// <param name="id">CompanyId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(Guid id)
        {
            try
            {
                var entity = _uow.Companies.Set()
                                           .Include(x => x.RecordStatus)
                                           .Include(x => x.CreatedByNavigation)
                                           .Include(x => x.ModifiedByNavigation)
                                           .Include(x => x.DeletedByNavigation)
                                           .FirstOrDefault(x => x.CompanyId == id);

                var result = _mapper.Map<CompanyDTO>(entity);
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

        public ResponseDTO GetArchivePath()
        {
            try
            {
                // company tablosunda her zaman tek bir kayıt olduğu düşünülülerek böyle yazılmıştır.
                var entity = _uow.Companies.Single(x => x.CompanyId != null);

                if (entity == null)
                {
                    return NotFound();
                }

                return Success(entity.ArchivePath);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}