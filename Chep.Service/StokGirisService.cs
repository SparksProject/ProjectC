using AutoMapper;

using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Chep.Service
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


        public ResponseDTO Add(ChepStokGirisDTO obj)
        {
            try
            {
                var entity = Map(obj);

                var result = _uow.ChepStokGiris.Add(entity);

                _uow.Commit();

                return Success(result.StokGirisId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO Edit(ChepStokGirisDTO obj)
        {
            try
            {
                var entity = Map(obj);

                var result = _uow.ChepStokGiris.Update(entity);

                if (obj.ChepStokGirisDetayList != null)
                {
                    foreach (var item in obj.ChepStokGirisDetayList)
                    {
                        item.StokGirisId = obj.StokGirisId;

                        if (item.StokGirisDetayId > 0)
                        {
                            var rr = _uow.ChepStokGirisDetay.Update(Map(item));
                        }
                        else
                        {
                            var rr = _uow.ChepStokGirisDetay.Add(Map(item));
                        }
                    }
                }

                if (obj.DeletedChepStokGirisDetayIdList != null)
                {
                    foreach (var item in obj.DeletedChepStokGirisDetayIdList)
                    {
                        _uow.ChepStokGirisDetay.Delete(new ChepStokGirisDetay { StokGirisDetayId = item });
                    }
                }

                _uow.Commit();

                return Success(result.StokGirisId);
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
                var entity = _uow.ChepStokGiris.Set()
                                               .Include(x => x.ChepStokGirisDetay)
                                               .Include(x => x.IhracatciFirmaNavigation)
                                               .Include(x => x.IthalatciFirmaNavigation)
                                               .FirstOrDefault(x => x.StokGirisId == id);

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
                var entities = _uow.ChepStokGiris.Set()
                                                 .Include(x => x.ChepStokGirisDetay)
                                                 .Include(x => x.IhracatciFirmaNavigation)
                                                 .Include(x => x.IthalatciFirmaNavigation)
                                                 .ToList();

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

                var target = new List<ChepStokGirisDTO>();

                foreach (var item in entities)
                {
                    var obj = Map(item);

                    target.Add(obj);
                }

                return Success(target);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO ListDetails()
        {
            try
            {
                var entities = _uow.ChepStokGirisDetay.Set()
                                                      .Include(x => x.ChepStokCikisDetay)
                                                      .Include(x => x.StokGiris)
                                                      .ToList();

                var list = new List<ChepStokGirisDetayDTO>();

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


        private ChepStokGiris Map(ChepStokGirisDTO obj)
        {
            if (obj == null)
            {
                return default;
            }

            return new ChepStokGiris
            {
                BasvuruTarihi = obj.BasvuruTarihi,
                BelgeAd = obj.BelgeAd,
                BelgeSart = obj.BelgeSart,
                BeyannameNo = obj.BeyannameNo,
                BeyannameTarihi = obj.BeyannameTarihi,
                GumrukKod = obj.GumrukKod,
                IhracatciFirma = obj.IhracatciFirma,
                IthalatciFirma = obj.IthalatciFirma,
                KapAdet = obj.KapAdet,
                ReferansNo = obj.ReferansNo,
                StokGirisId = obj.StokGirisId,
                SureSonuTarihi = obj.SureSonuTarihi,
                TpsAciklama = obj.TpsAciklama,
                TpsDurum = obj.TpsDurum,
                TpsNo = obj.TpsNo,
            };
        }

        private ChepStokGirisDetay Map(ChepStokGirisDetayDTO obj)
        {
            if (obj == null)
            {
                return default;
            }

            return new ChepStokGirisDetay
            {
                CikisRejimi = obj.CikisRejimi,
                EsyaCinsi = obj.EsyaCinsi,
                EsyaGtip = obj.EsyaGtip,
                FaturaDovizKod = obj.FaturaDovizKod,
                FaturaNo = obj.FaturaNo,
                FaturaTarih = obj.FaturaTarih,
                FaturaTutar = obj.FaturaTutar,
                GidecegiUlke = obj.GidecegiUlke,
                Marka = obj.Marka,
                MenseUlke = obj.MenseUlke,
                Miktar = obj.Miktar,
                Model = obj.Model,
                OlcuBirimi = obj.OlcuBirimi,
                PoNo = obj.PoNo,
                Rejim = obj.Rejim,
                SozlesmeUlke = obj.SozlesmeUlke,
                StokGirisDetayId = obj.StokGirisDetayId,
                StokGirisId = obj.StokGirisId,
                TpsBeyan = obj.TpsBeyan,
                TpsSiraNo = obj.TpsSiraNo,
                UrunKod = obj.UrunKod,
            };
        }

        private ChepStokGirisDetayDTO Map(ChepStokGirisDetay obj)
        {
            if (obj == null)
            {
                return default;
            }

            var target = new ChepStokGirisDetayDTO
            {
                CikisRejimi = obj.CikisRejimi,
                EsyaCinsi = obj.EsyaCinsi,
                EsyaGtip = obj.EsyaGtip,
                FaturaDovizKod = obj.FaturaDovizKod,
                FaturaNo = obj.FaturaNo,
                FaturaTarih = obj.FaturaTarih,
                FaturaTutar = obj.FaturaTutar,
                GidecegiUlke = obj.GidecegiUlke,
                Marka = obj.Marka,
                MenseUlke = obj.MenseUlke,
                Miktar = obj.Miktar,
                Model = obj.Model,
                OlcuBirimi = obj.OlcuBirimi,
                PoNo = obj.PoNo,
                Rejim = obj.Rejim,
                SozlesmeUlke = obj.SozlesmeUlke,
                StokGirisDetayId = obj.StokGirisDetayId,
                StokGirisId = obj.StokGirisId.Value,
                TpsBeyan = obj.TpsBeyan,
                TpsSiraNo = obj.TpsSiraNo,
                UrunKod = obj.UrunKod,
                ChepStokCikisDetayList = new List<ChepStokCikisDetayDTO>()
            };

            if (obj.StokGiris != null)
            {
                target.BeyannameNo = obj.StokGiris.BeyannameNo;
                target.TpsNo = obj.StokGiris.TpsNo;
            }

            if (obj.ChepStokCikisDetay != null)
            {
                foreach (var item in obj.ChepStokCikisDetay)
                {
                    target.ChepStokCikisDetayList.Add(new ChepStokCikisDetayDTO
                    {
                        Kg = item.Kg,
                        Miktar = item.Miktar,
                        StokCikisDetayId = item.StokCikisDetayId,
                        StokCikisId = item.StokCikisId,
                        StokGirisDetayId = item.StokGirisDetayId,
                    });
                }
            }

            return target;
        }

        private ChepStokGirisDTO Map(ChepStokGiris obj)
        {
            if (obj == null)
            {
                return default;
            }

            var details = new List<ChepStokGirisDetayDTO>();

            if (obj.ChepStokGirisDetay != null && obj.ChepStokGirisDetay.Count > 0)
            {
                details.AddRange(obj.ChepStokGirisDetay.Select(item => Map(item)));
            }

            var target = new ChepStokGirisDTO
            {
                BasvuruTarihi = obj.BasvuruTarihi,
                BelgeAd = obj.BelgeAd,
                BelgeSart = obj.BelgeSart,
                BeyannameNo = obj.BeyannameNo,
                BeyannameTarihi = obj.BeyannameTarihi,
                GumrukKod = obj.GumrukKod,
                IhracatciFirma = obj.IhracatciFirma,
                IthalatciFirma = obj.IthalatciFirma,
                KapAdet = obj.KapAdet,
                ReferansNo = obj.ReferansNo,
                StokGirisId = obj.StokGirisId,
                SureSonuTarihi = obj.SureSonuTarihi,
                TpsAciklama = obj.TpsAciklama,
                TpsDurum = obj.TpsDurum,
                TpsNo = obj.TpsNo,

                ChepStokGirisDetayList = details
            };

            if (obj.IhracatciFirmaNavigation != null)
            {
                target.IhracatciFirmaName = obj.IhracatciFirmaNavigation.Name;
            }
            if (obj.IthalatciFirmaNavigation != null)
            {
                target.IthalatciFirmaName = obj.IthalatciFirmaNavigation.Name;
            }

            return target;
        }
    }
}