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
    public class StokCikisService : BaseService, IStokCikisService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IDefinitionService _definitionService;

        public StokCikisService(IUnitOfWork uow, IMapper mapper, IDefinitionService definitionService)
        {
            _uow = uow;
            _mapper = mapper;
            _definitionService = definitionService;
        }


        public ResponseDTO Add(ChepStokCikisDTO obj)
        {
            try
            {
                if (obj == null)
                {
                    return BadRequest();
                }

                obj.ReferansNo = Convert.ToInt32(_definitionService.GetNextReferenceNumber("Cikis").Result);
                obj.InvoiceId = Guid.NewGuid();

                var entity = Map(obj);

                var result = _uow.ChepStokCikis.Add(entity);
                foreach (var item in result.ChepStokCikisDetay)
                {
                    if (item.InvoiceDetailId == null)
                    {
                        item.InvoiceDetailId = Guid.NewGuid();
                    }
                }
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
                if (obj == null)
                {
                    return BadRequest();
                }

                var entity = Map(obj);

                entity.ChepStokCikisDetay = null;

                var result = _uow.ChepStokCikis.Update(entity);

                if (obj.DeletedChepStokCikisDetayIdList != null)
                {
                    foreach (var item in obj.DeletedChepStokCikisDetayIdList)
                    {
                        _uow.ChepStokCikisDetay.Delete(new ChepStokCikisDetay { StokCikisDetayId = item });
                    }
                }

                if (obj.ChepStokCikisDetayList != null)
                {
                    foreach (var item in obj.ChepStokCikisDetayList)
                    {
                        item.StokCikisId = obj.StokCikisId;

                        var detailEntity = Map(item);

                        if (detailEntity.StokCikisDetayId > 0)
                        {
                            _uow.ChepStokCikisDetay.Update(detailEntity);
                        }
                        else
                        {
                            if (!detailEntity.InvoiceDetailId.HasValue)
                            {
                                detailEntity.InvoiceDetailId = Guid.NewGuid();
                            }

                            _uow.ChepStokCikisDetay.Add(detailEntity);
                        }
                    }
                }

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
                var entity = _uow.ChepStokCikis.Set()
                                               .Include(x => x.ChepStokCikisDetay)
                                               .ThenInclude(x => x.StokGirisDetay)
                                               .AsNoTracking()
                                               .FirstOrDefault(x => x.StokCikisId == id);

                var result = Map(entity);

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO GetByUrunKod(string id)
        {
            try
            {
                var entity = _uow.Products.Set()
                                               .AsNoTracking()
                                               .FirstOrDefault(x => x.ProductNo == id);

                var result = new ProductDTO
                {
                    ProductId = entity.ProductId,
                    ProductNo = entity.ProductNo,
                    BirimTutar = entity.UnitPrice,
                    NetWeight = entity.NetWeight,
                    GrossWeight = entity.GrossWeight
                };

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        public ResponseDTO List(int? referansNo, string beyannameNo, string tpsNo)
        {
            try
            {
                var entities = _uow.ChepStokCikis.Set()
                                                 .Include(x => x.ChepStokCikisDetay)
                                                 .Include(x => x.IhracatciFirmaNavigation)
                                                 .Include(x => x.IhracatciFirmaNavigation)
                                                 .ToList();

                if (referansNo.HasValue && referansNo.Value > 0)
                {
                    entities = entities.Where(x => x.ReferansNo.Equals(referansNo.Value)).ToList();
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


        public ResponseDTO Delete(int id)
        {
            try
            {
                var entity = _uow.ChepStokCikis.Set()
                                               .Include(x => x.ChepStokCikisDetay)
                                               .FirstOrDefault(x => x.StokCikisId == id);

                if (entity.ChepStokCikisDetay.Count > 0)
                {
                    foreach (var item in entity.ChepStokCikisDetay)
                    {
                        _uow.ChepStokCikisDetay.Delete(item);
                    }
                }

                _uow.ChepStokCikis.Delete(entity);

                _uow.Commit();

                return Success(entity.StokCikisId, "Silme işlemi tamamlandı!");
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
                        InvoiceAmount = item.FaturaTutar,
                        BirimTutar = item.BirimTutar,
                        InvoiceDetailId = Guid.NewGuid()
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

        public ResponseDTO GetStokDusumListe(string itemNo, int toplamCikisAdet, Guid ithalatciFirma)
        {
            try
            {
                if (string.IsNullOrEmpty(itemNo))
                {
                    return Warning("Geçersiz Ürün Kodu!");
                }

                itemNo = itemNo.ToLower();

                var entities = _uow.ViewStokDusumListe.Search(x => x.UrunKod != null && x.UrunKod.ToLower().Equals(itemNo) && x.IthalatciFirma == ithalatciFirma);

                var target = new List<ViewStokDusumListeDto>();

                var cikisAltToplam = 0;
                foreach (var item in entities.OrderBy(x => x.SureSonuTarihi))
                {
                    var farkCikis = toplamCikisAdet - cikisAltToplam;

                    var obj = Mapper.MapSingle<VwStokDusumListe, ViewStokDusumListeDto>(item);
                    if (farkCikis > 0)
                    {
                        if (obj.KalanMiktar.HasValue && farkCikis >= obj.KalanMiktar)
                        {
                            obj.DusulenMiktar = obj.KalanMiktar.Value;
                            cikisAltToplam += obj.KalanMiktar.Value;
                        }
                        else
                        {
                            obj.DusulenMiktar = farkCikis;
                            cikisAltToplam += farkCikis;
                        }
                    }
                    obj.FaturaTutar = (Convert.ToDecimal(obj.DusulenMiktar) * item.BirimFiyat);
                    obj.BirimTutar = item.BirimFiyat;
                    obj.NetKg = (Convert.ToDecimal(obj.DusulenMiktar) * item.NetKg);
                    obj.BrutKg = (Convert.ToDecimal(obj.DusulenMiktar) * item.BrutKg);
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

        public ResponseDTO StokDusumListeAdd(string itemNo, int dusulenMiktar, List<ChepStokCikisDetayDTO> details)
        {
            try
            {
                if (string.IsNullOrEmpty(itemNo))
                {
                    return Warning("Geçersiz Ürün Kodu!");
                }
                if (dusulenMiktar < 1)
                {
                    return Warning("Çıkış yapılacak ürün sayısı girilmedi!");
                }

                itemNo = itemNo.ToLower();

                var entities = _uow.ViewStokDusumListe.Search(x => x.UrunKod != null && x.UrunKod.ToLower().Equals(itemNo));

                var detaylarToplamMiktar = 0;

                if (details != null)
                {
                    foreach (var item in details.Where(x => !string.IsNullOrEmpty(x.UrunKod) && x.Miktar.HasValue).Where(x => x.UrunKod.ToLower().Equals(itemNo)))
                    {
                        detaylarToplamMiktar += item.Miktar.Value;
                    }
                }

                var target = new List<ViewStokDusumListeDto>();
                var dusulenToplamMiktar = 0;


                var kalanlarDusulenToplam = 0;
                foreach (var item in entities.Where(x => x.KalanMiktar.HasValue).OrderBy(x => x.SureSonuTarihi))
                {
                    var farkCikis = dusulenMiktar - dusulenToplamMiktar;
                    var obj = Mapper.MapSingle<VwStokDusumListe, ViewStokDusumListeDto>(item);

                    if (detaylarToplamMiktar > 0 && kalanlarDusulenToplam < detaylarToplamMiktar)
                    {
                        if (obj.KalanMiktar >= detaylarToplamMiktar)
                        {
                            obj.KalanMiktar -= detaylarToplamMiktar;
                            kalanlarDusulenToplam += detaylarToplamMiktar;
                        }
                        else
                        {
                            obj.KalanMiktar -= obj.KalanMiktar;
                            kalanlarDusulenToplam += obj.KalanMiktar.Value;
                        }
                    }

                    if (farkCikis > 0)
                    {
                        if (farkCikis >= obj.KalanMiktar)
                        {
                            obj.DusulenMiktar = obj.KalanMiktar.Value;
                            dusulenToplamMiktar += obj.KalanMiktar.Value;
                        }
                        else
                        {
                            obj.DusulenMiktar = farkCikis;
                            dusulenToplamMiktar += farkCikis;
                        }
                    }

                    obj.FaturaTutar = (Convert.ToDecimal(obj.DusulenMiktar) * item.BirimFiyat);
                    obj.BirimTutar = item.BirimFiyat;
                    obj.NetKg = (Convert.ToDecimal(obj.DusulenMiktar) * item.NetKg);
                    obj.BrutKg = (Convert.ToDecimal(obj.DusulenMiktar) * item.BrutKg);

                    target.Add(obj);
                }

                if (dusulenMiktar - dusulenToplamMiktar > 0)
                {
                    var message = $"Çıkış Adet, stoktan fazladır! Hesaplamalar stok miktarı baz alınarak yapılmıştır! Aşım adeti: {dusulenMiktar - dusulenToplamMiktar}";

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
                TpsNo = obj.TpsNo,
                IslemTarihi = obj.IslemTarihi,
                TpsTarih = obj.TpsTarih,
                GtbReferenceNo = obj.GtbReferenceNo,
                InvoiceAmount = obj.InvoiceAmount,
                InvoiceCurrency = obj.InvoiceCurrency,
                InvoiceDate = obj.InvoiceDate,
                InvoiceId = obj.InvoiceId,
                InvoiceNo = obj.InvoiceNo,
                AliciFirma = obj.AliciFirma,
                CikisGumruk = obj.CikisGumruk,
                NakliyeciFirma = obj.NakliyeciFirma,
                OdemeSekli = obj.OdemeSekli,
                TeslimSekli = obj.TeslimSekli,
                CikisAracKimligi = obj.CikisAracKimligi,

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
                StokGirisDetayId = obj.StokGirisDetayId,
                TpsCikisSiraNo = obj.TpsCikisSiraNo,
                InvoiceAmount = obj.InvoiceAmount,
                BirimTutar = obj.BirimTutar,
                BrutKg = obj.BrutKg,
                NetKg = obj.NetKg,
                InvoiceDetailId = obj.InvoiceDetailId

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
                TpsCikisSiraNo = obj.TpsCikisSiraNo,
                InvoiceAmount = obj.InvoiceAmount,
                InvoiceDetailId = obj.InvoiceDetailId,
                BirimTutar = obj.BirimTutar,
                BrutKg = obj.BrutKg,
                NetKg = obj.NetKg,
                UrunKod = obj.StokGirisDetay?.UrunKod,
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

            var target = new ChepStokCikisDTO
            {
                BeyannameNo = obj.BeyannameNo,
                BeyannameTarihi = obj.BeyannameTarihi,
                IhracatciFirma = obj.IhracatciFirma,
                ReferansNo = obj.ReferansNo,
                StokCikisId = obj.StokCikisId,
                TpsNo = obj.TpsNo,
                IslemTarihi = obj.IslemTarihi,
                TpsTarih = obj.TpsTarih,
                GtbReferenceNo = obj.GtbReferenceNo,
                InvoiceAmount = obj.InvoiceAmount,
                InvoiceCurrency = obj.InvoiceCurrency,
                InvoiceDate = obj.InvoiceDate,
                InvoiceId = obj.InvoiceId,
                InvoiceNo = obj.InvoiceNo,
                AliciFirma = obj.AliciFirma,
                CikisGumruk = obj.CikisGumruk,
                NakliyeciFirma = obj.NakliyeciFirma,
                OdemeSekli = obj.OdemeSekli,
                TeslimSekli = obj.TeslimSekli,
                CikisAracKimligi = obj.CikisAracKimligi,

                ChepStokCikisDetayList = details
            };

            if (obj.IhracatciFirmaNavigation != null)
            {
                target.IhracatciFirmaName = obj.IhracatciFirmaNavigation.Name;
            }


            return target;
        }
    }
}