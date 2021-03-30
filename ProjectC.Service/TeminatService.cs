using AutoMapper;

using ProjectC.Data.Models;
using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service.Interface;

using System;
using System.Collections.Generic;

namespace ProjectC.Service
{
    public class TeminatService : BaseService, ITeminatService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TeminatService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public ResponseDTO Add(TeminatDTO obj)
        {
            try
            {
                var entity = Mapper.MapSingle<TeminatDTO,Teminat>(obj);

                var result = _uow.Teminats.Add(entity);

                _uow.Commit();

                return Success(result.TeminatId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO Edit(TeminatDTO obj)
        {
            try
            {
                var entity = Mapper.MapSingle<TeminatDTO, Teminat>(obj);

                var result = _uow.Teminats.Update(entity);

                _uow.Commit();

                return Success(result.TeminatId);
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
                var entity = _uow.Teminats.Single(x => x.TeminatId == id);

                var result = Mapper.MapSingle<Teminat,TeminatDTO>(entity);

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
                var entities = _uow.Teminats.GetAll();

                List<TeminatDTO> list = new List<TeminatDTO>();

                foreach (var item in entities)
                {
                    TeminatDTO obj = new TeminatDTO
                    {
                        TeminatId = item.TeminatId,
                        DosyaTipi = item.DosyaTipi,
                        DosyaNo = item.DosyaNo,
                        Gonderici = item.Gonderici,
                        Alici = item.Alici,
                        TescilNo = item.TescilNo,
                        TescilTarihi = item.TescilTarihi,
                        Gumruk = item.Gumruk,
                        Banka = item.Banka,
                        AntrepoKodu = item.AntrepoKodu,
                        TeminatRefNo = item.TeminatRefNo,
                        TeminatTutari = item.TeminatTutari,
                        OdenecekTutar = item.OdenecekTutar,
                        MuracatNo = item.MuracatNo,
                        MuracatTarihi = item.MuracatTarihi,
                        CozumTarihi = item.CozumTarihi,
                        Aciklama = item.Aciklama,
                        EvrakTeslimAlan = item.EvrakTeslimAlan,
                        EvrakTeslimAlmaTarihi = item.EvrakTeslimAlmaTarihi,
                        EvrakTeslimEden = item.EvrakTeslimEden,
                        EvrakTeslimTarihi = item.EvrakTeslimTarihi

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
