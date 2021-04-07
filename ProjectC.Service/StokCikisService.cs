using AutoMapper;

using ProjectC.Data.Models;
using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service.Interface;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectC.Service
{
    public class StokCikisService : BaseService, IStokCikisService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StokCikisService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public ResponseDTO Add(ChepStokCikisDTO obj)
        {
            try
            {
                var entity = Map(obj);

                var result = _uow.ChepStokCikis.Add(entity);

                _uow.Commit();

                return Success(result.StokCikisId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO Edit(ChepStokCikisDTO obj)
        {
            try
            {
                var entity = Map(obj);

                var result = _uow.ChepStokCikis.Update(entity);

                _uow.Commit();

                return Success(result.StokCikisId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO Get(int id)
        {
            try
            {
                var entity = _uow.ChepStokCikis.Single(x => x.StokCikisId == id);

                var result = Map(entity);

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO List()
        {
            try
            {
                var entities = _uow.ChepStokCikis.GetAll();

                var list = new List<ChepStokCikisDTO>();

                foreach (var item in entities)
                {
                    var obj = Map(item);

                    list.Add(obj);
                }

                return Success(list);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO AddDetail(int stokCikisId, List<ViewStokDusumListeDto> details)
        {
            try
            {
                if (stokCikisId < 1 || details == null)
                {
                    return NotFound();
                }

                var detailEntities = new List<ChepStokCikisDetay>();

                foreach (var item in details.Where(x => x.DusulenMiktar > 0))
                {
                    detailEntities.Add(new ChepStokCikisDetay
                    {
                        StokCikisId = stokCikisId,
                        Miktar = item.DusulenMiktar,
                        StokGirisDetayId = item.StokGirisDetayId,
                    });
                }

                if (detailEntities.Count == 0)
                {
                    return NotFound();
                }

                _uow.ChepStokCikisDetay.AddRange(detailEntities);

                var result = _uow.Commit();

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        private ChepStokCikis Map(ChepStokCikisDTO obj)
        {
            if (obj == null)
            {
                return default;
            }

            var details = new List<ChepStokCikisDetay>();

            if (obj.ChepStokCikisDetayList != null && obj.ChepStokCikisDetayList.Count > 0)
            {
                details.AddRange(obj.ChepStokCikisDetayList.Select(item => Map(item)));
            }

            return new ChepStokCikis
            {
                BeyannameNo = obj.BeyannameNo,
                BeyannameTarihi = obj.BeyannameTarihi,
                IhracatciFirma = obj.IhracatciFirma,
                ReferansNo = obj.ReferansNo,
                StokCikisId = obj.StokCikisId,
                TPSNo = obj.TPSNo,

                ChepStokCikisDetayList = details,
            };
        }

        private ChepStokCikisDetay Map(ChepStokCikisDetayDTO obj)
        {
            if (obj == null)
            {
                return default;
            }

            return new ChepStokCikisDetay
            {
                Miktar = obj.Miktar,
                Kg = obj.Kg,
                StokCikisDetayId = obj.StokCikisDetayId,
                StokCikisId = obj.StokCikisId,
                StokGirisDetayId = obj.StokGirisDetayId
            };
        }

        private ChepStokCikisDetayDTO Map(ChepStokCikisDetay obj)
        {
            if (obj == null)
            {
                return default;
            }

            return new ChepStokCikisDetayDTO
            {
                Miktar = obj.Miktar,
                Kg = obj.Kg,
                StokCikisDetayId = obj.StokCikisDetayId,
                StokCikisId = obj.StokCikisId,
                StokGirisDetayId = obj.StokGirisDetayId,
            };
        }

        private ChepStokCikisDTO Map(ChepStokCikis obj)
        {
            if (obj == null)
            {
                return default;
            }

            var details = new List<ChepStokCikisDetayDTO>();

            if (obj.ChepStokCikisDetayList != null && obj.ChepStokCikisDetayList.Count > 0)
            {
                details.AddRange(obj.ChepStokCikisDetayList.Select(item => Map(item)));
            }

            return new ChepStokCikisDTO
            {
                BeyannameNo = obj.BeyannameNo,
                BeyannameTarihi = obj.BeyannameTarihi,
                IhracatciFirma = obj.IhracatciFirma,
                ReferansNo = obj.ReferansNo,
                StokCikisId = obj.StokCikisId,
                TPSNo = obj.TPSNo,
                IslemTarihi = obj.IslemTarihi,
                TPSTarih = obj.TPSTarih,

                ChepStokCikisDetayList = details
            };
        }
    }
}