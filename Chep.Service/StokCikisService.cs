﻿using AutoMapper;

using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Chep.Service
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

        public ResponseDTO List(string referansNo, string beyannameNo, string tpsNo)
        {
            try
            {
                var entities = _uow.ChepStokCikis.GetAll();

                if (!string.IsNullOrEmpty(referansNo))
                {
                    referansNo = referansNo.ToLower();
                    entities = entities.Where(x => x.ReferansNo != null).Where(x => x.ReferansNo.ToLower().Contains(referansNo)).ToList();
                }

                if (!string.IsNullOrEmpty(beyannameNo))
                {
                    beyannameNo = beyannameNo.ToLower();
                    entities = entities.Where(x => x.BeyannameNo != null).Where(x => x.BeyannameNo.ToLower().Contains(beyannameNo)).ToList();
                }

                if (!string.IsNullOrEmpty(tpsNo))
                {
                    tpsNo = tpsNo.ToLower();
                    entities = entities.Where(x => x.TpsNo != null).Where(x => x.TpsNo.ToLower().Contains(tpsNo)).ToList();
                }

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

        public ResponseDTO GetStokDusumListe(string itemNo, int toplamCikisAdet)
        {
            try
            {
                if (string.IsNullOrEmpty(itemNo))
                {
                    return Warning("Geçersiz Ürün Kodu!");
                }

                itemNo = itemNo.ToLower();

                var entities = _uow.ViewStokDusumListe.Search(x => x.UrunKod != null && x.UrunKod.ToLower().Equals(itemNo));

                var target = new List<ViewStokDusumListeDto>();

                var cikisAltToplam = 0;
                foreach (var item in entities.OrderBy(x => x.SureSonuTarihi))
                {
                    var farkCikis = toplamCikisAdet - cikisAltToplam;

                    var obj = Mapper.MapSingle<VwStokDusumListe, ViewStokDusumListeDto>(item);

                    if (farkCikis > 0)
                    {
                        if (farkCikis >= obj.KalanMiktar)
                        {
                            obj.DusulenMiktar = obj.KalanMiktar;
                            cikisAltToplam += obj.KalanMiktar;
                        }
                        else
                        {
                            obj.DusulenMiktar = farkCikis;
                            cikisAltToplam += farkCikis;
                        }
                    }

                    target.Add(obj);
                }

                if (toplamCikisAdet - cikisAltToplam > 0)
                {
                    var message = $"Çıkış Adet, stoktan fazladır! Hesaplamalar stok miktarı baz alınarak yapılmıştır! Aşım adeti: {toplamCikisAdet - cikisAltToplam}";

                    return Success(target, message);
                }

                return Success(target);
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
                TpsNo = obj.TPSNo,

                ChepStokCikisDetay = details,
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

            if (obj.ChepStokCikisDetay != null && obj.ChepStokCikisDetay.Count > 0)
            {
                details.AddRange(obj.ChepStokCikisDetay.Select(item => Map(item)));
            }

            return new ChepStokCikisDTO
            {
                BeyannameNo = obj.BeyannameNo,
                BeyannameTarihi = obj.BeyannameTarihi,
                IhracatciFirma = obj.IhracatciFirma,
                ReferansNo = obj.ReferansNo,
                StokCikisId = obj.StokCikisId,
                TPSNo = obj.TpsNo,
                IslemTarihi = obj.IslemTarihi,
                TPSTarih = obj.TpsTarih,

                ChepStokCikisDetayList = details
            };
        }
    }
}