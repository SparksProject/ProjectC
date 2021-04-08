using AutoMapper;

using Microsoft.EntityFrameworkCore;

using ProjectC.Core;
using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service.Interface;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ProjectC.Service
{
    public class SparksArchiveService : BaseService, ISparksArchiveService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SparksArchiveService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public ResponseDTO Add(SparksArchiveDTO obj)
        {
            try
            {
                obj.ArchiveId = Guid.NewGuid();
                obj.InsDate = DateTime.Now;

                var entity = _mapper.Map<SparksArchive>(obj);

                var result = _uow.SparksArchives.Add(entity);

                _uow.Commit();

                return Success(result.ArchiveId);

            }
            catch (Exception ex)
            {
                return Error(ex);
            }

        }

        public ResponseDTO AddRange(List<SparksArchive> obj)
        {
            try
            {
                _uow.SparksArchives.AddRange(obj);

                _uow.Commit();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO AddRange(Guid customerId, string dosyaTipi, string dosyaNo, string belgeAdi, string dosyaYolu)
        {
            throw new NotImplementedException();
        }

        public ResponseDTO Delete(Guid id)
        {
            var entity = _uow.SparksArchives.Single(x => x.ArchiveId == id);

            if (entity == null)
            {
                return NotFound();
            }
            _uow.SparksArchives.Delete(entity);
            _uow.Commit();

            return Success(entity);

        }

        public ResponseDTO Edit(SparksArchiveDTO obj)
        {
            try
            {
                var entity = _mapper.Map<SparksArchive>(obj);

                var result = _uow.SparksArchives.Update(entity);

                _uow.Commit();

                return Success(result.ArchiveId);
            }
            catch (Exception ex)
            {

                return Error(ex);
            }
        }

        public ResponseDTO Get(Guid id)
        {
            try
            {
                var entity = _uow.SparksArchives.Single(x => x.ArchiveId == id);

                var result = _mapper.Map<SparksArchiveDTO>(entity);

                result.CustomerName = entity.Customer.Name;

                return Success(result);

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        //public ResponseDTO List(ArchiveFiltersDTO obj)
        //{
        //    try
        //    {
        //        using (var context = new Chep_NewContext())
        //        {
        //            var sqlUserId = new SqlParameter("@Id", SqlDbType.Int) { Value = obj.UserId };
        //            var sqlTescilNo = new SqlParameter("@TescilNo", SqlDbType.NVarChar) { Value = obj.TescilNo };
        //            var sqlDosyaNo = new SqlParameter("@DosyaNo", SqlDbType.NVarChar) { Value = obj.DosyaNo };
        //            var sqlFaturaNo = new SqlParameter("@FaturaNo", SqlDbType.NVarChar) { Value = obj.FaturaNo };
        //            var sqlTescilTarihiBaslangic = new SqlParameter("@TescilTarihi1", SqlDbType.DateTime) { Value = obj.TescilTarihiBaslangic };
        //            var sqlTescilTarihiBitis = new SqlParameter("@TescilTarihi2", SqlDbType.DateTime) { Value = obj.TescilTarihiBitis };

        //            var parameters = new List<SqlParameter> { sqlUserId };

        //            var query = $"SELECT * FROM vw_SparksArchive WHERE UserId = @Id";

        //            if (!string.IsNullOrEmpty(obj.TescilNo))
        //            {
        //                query += " AND TescilNo = @TescilNo";

        //                parameters.Add(sqlTescilNo);
        //            }
        //            if (!string.IsNullOrEmpty(obj.DosyaNo))
        //            {
        //                query += " AND DosyaNo = @DosyaNo";

        //                parameters.Add(sqlDosyaNo);
        //            }
        //            if (!string.IsNullOrEmpty(obj.FaturaNo))
        //            {
        //                query += " AND FaturaNo like @FaturaNo";

        //                parameters.Add(sqlFaturaNo);
        //            }
        //            if (obj.TescilTarihiBaslangic.HasValue && obj.TescilTarihiBitis.HasValue)
        //            {
        //                query += " AND TescilTarihi BETWEEN @TescilTarihi1 and @TescilTarihi2";

        //                parameters.Add(sqlTescilTarihiBaslangic);
        //                parameters.Add(sqlTescilTarihiBitis);
        //            }

        //            var target = new List<ViewSparksArchiveDTO>();

        //            foreach (var item in context.ViewSparksArchive.FromSql(query, parameters.ToArray<object>()))
        //            {
        //                target.Add(new ViewSparksArchiveDTO
        //                {
        //                    DosyaNo = item.DosyaNo,
        //                    UserId = item.UserId,
        //                    Alici = item.Alici,
        //                    ArsivPath = item.ArsivPath,
        //                    FaturaNo = item.FaturaNo,
        //                    Firma = item.Firma,
        //                    FirmaId = item.FirmaId,
        //                    Id = item.Id,
        //                    MusRefNo = item.MusRefNo,
        //                    TescilNo = item.TescilNo,
        //                    TescilTarihi = item.TescilTarihi
        //                });
        //            }

        //            return Success(target);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ex);
        //    }
        //}
    }
}