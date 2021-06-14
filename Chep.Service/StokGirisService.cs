using AutoMapper;
using System.Collections.Generic;
using System.Net.Http;
using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.Globalization;

namespace Chep.Service
{
    public class StokGirisService : BaseService, IStokGirisService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IDefinitionService _definitionService;

        public StokGirisService(IUnitOfWork uow, IMapper mapper, IDefinitionService definitionService)
        {
            _uow = uow;
            _mapper = mapper;
            _definitionService = definitionService;
        }


        public ResponseDTO Add(ChepStokGirisDTO obj)
        {
            try
            {
                if (obj == null)
                {
                    return BadRequest();
                }

                obj.ReferansNo = Convert.ToInt32(_definitionService.GetNextReferenceNumber("Giris").Result);

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


                //if (obj.ChepStokGirisDetayList != null)
                //{
                //    foreach (var item in obj.ChepStokGirisDetayList)
                //    {
                //        //item.StokGirisId = obj.StokGirisId;

                //        if (item.StokGirisDetayId > 0)
                //        {
                //            var rr = _uow.ChepStokGirisDetay.Update(Map(item));
                //        }
                //        else
                //        {
                //            var rr = _uow.ChepStokGirisDetay.Add(Map(item));
                //        }
                //    }
                //}
                var entity = Map(obj);
                var result = _uow.ChepStokGiris.Update(entity);

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

        public ResponseDTO List(int? referansNo, string beyannameNo, string tpsNo)
        {
            try
            {
                var entities = _uow.ChepStokGiris.Set()
                                                 .Include(x => x.ChepStokGirisDetay)
                                                 .Include(x => x.IhracatciFirmaNavigation)
                                                 .Include(x => x.IthalatciFirmaNavigation)
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

        public ResponseDTO Delete(int id)
        {
            try
            {
                var entity = _uow.ChepStokGiris.Set()
                                               .Include(x => x.ChepStokGirisDetay)
                                               .ThenInclude(x => x.ChepStokCikisDetay)
                                               .FirstOrDefault(x => x.StokGirisId == id);

                if (entity.ChepStokGirisDetay.Count > 0)
                {
                    foreach (var item in entity.ChepStokGirisDetay)
                    {
                        if (item.ChepStokCikisDetay.Count > 0)//detaylar çıkışta kullanılmış mı diye bakar eğer kullanıldıysa işlemi bitirir.
                        {
                            return Warning($"Referans Numarası {entity.ReferansNo} olan kaydın {item.TpsSiraNo} TPS Sıra Numaralı ve {item.UrunKod} Ürün Kodlu kalemi için Stok Çıkış sayfasının kaleminde eşleşen kayıt bulunuyor! Silme işlemi yapılmayacak!");
                        }
                        _uow.ChepStokGirisDetay.Delete(item);
                    }
                }

                _uow.ChepStokGiris.Delete(entity);

                _uow.Commit();

                return Success(entity.StokGirisId, "Silme işlemi tamamlandı!");
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
            var details = new List<ChepStokGirisDetay>();

            if (obj.ChepStokGirisDetayList != null && obj.ChepStokGirisDetayList.Count > 0)
            {
                details.AddRange(obj.ChepStokGirisDetayList.Select(item => Map(item)));
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
                AktarimTarihi = obj.AktarimTarihi,

                ChepStokGirisDetay = details,
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
                TpsBeyan = obj.TpsBeyan,
                TpsSiraNo = obj.TpsSiraNo,
                UrunKod = obj.UrunKod,
                TpsCikisSiraNo = obj.TpsCikisSiraNo,
                StokGirisDetayId = obj.StokGirisDetayId,
                StokGirisId = obj.StokGirisId,
                BeyannameKalemNo = obj.BeyannameKalemNo,
                BeyannameNo = obj.BeyannameNo,
                BeyannameTarihi = obj.BeyannameTarihi,
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
                StokGirisId = obj.StokGirisId,
                TpsBeyan = obj.TpsBeyan,
                TpsSiraNo = obj.TpsSiraNo,
                UrunKod = obj.UrunKod,
                TpsCikisSiraNo = obj.TpsCikisSiraNo,
                BeyannameKalemNo = obj.BeyannameKalemNo,
                BeyannameTarihi = obj.BeyannameTarihi,
                BeyannameNo = obj.BeyannameNo,
                TpsNo = obj.StokGiris.TpsNo,

                ChepStokCikisDetayList = new List<ChepStokCikisDetayDTO>()
            };

            if (obj.StokGiris != null)
            {
                target.StokGirisBeyannameNo = obj.StokGiris.BeyannameNo;
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
                AktarimTarihi = obj.AktarimTarihi,

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

        #region ExcelImport

        private const string ExcelTpsNo = "TPS No";
        private const string ExcelTpsDurum = "TPS Durum";
        private const string ExcelBasvuruTarihi = "Başvuru Tarihi";
        private const string ExcelSureSonuTarihi = "Süre Sonu Tarihi";
        private const string ExcelGumrukKod = "Gumruk";
        private const string ExcelBeyannameNo = "Beyanname No";
        private const string ExcelBeyannameTarihi = "Beyanname Tarihi";
        private const string ExcelBelgeAd = "Belge Adi";
        private const string ExcelBelgeSart = "Belge Ozel Sart";
        private const string ExcelTpsAciklama = "TPS Aciklama";
        private const string ExcelIthalatciFirma = "İthalatci Firma";
        private const string ExcelIhracatciFirma = "İhracatci Firma";
        private const string ExcelTpsSiraNo = "TPS Sıra No";
        private const string ExcelTpsBeyan = "TPS Beyan";
        private const string ExcelEsyaCinsi = "Eşya Cinsi";
        private const string ExcelEsyaGtip = "Eşya Gtip";
        private const string ExcelFaturaNo = "Fatura No";
        private const string ExcelFaturaTarih = "Fatura Tarih";
        private const string ExcelFaturaTutar = "Fatura Tutar";
        private const string ExcelFaturaDovizKod = "Fatura Doviz Kod";
        private const string ExcelMiktar = "Miktar";
        private const string ExcelOlcuBirimi = "Olçü Birimi";
        private const string ExcelRejim = "Rejim";
        private const string ExcelCikisRejimi = "Çıkış Rejim";
        private const string ExcelGidecegiUlke = "Gideceği Ülke";
        private const string ExcelMenseUlke = "Menşe Ülke";
        private const string ExcelSozlesmeUlke = "Sözleşme Ülke";
        private const string ExcelMarka = "Marka";
        private const string ExcelModel = "Model";
        private const string ExcelUrunKod = "Ürün Kod";
        private const string ExcelTPSCikisSiraNo = "TPS Çıkış Sıra No";
        private const string ExcelBeyannameKalemNo = "Beyanname Kalem No";
        private const string ExcelAktarimTarihi = "Aktarım Tarihi";

        private const string ImportNoIndexKey = "NoIndex";
        private const string ImportNoFileMasterKey = "NoFileMaster";
        private const string ImportMultipleFileMasterKey = "MultipleFileMaster";
        private const string ImportNoContractKey = "NoContract";
        private const string ImportMaxValueKey = "MaxValue";
        private const string ImportMultipleProductKey = "MultipleProduct";
        private const string ImportNoProductKey = "NoProduct";

        public ResponseDTO Import(IFormFile file, int userId)
        {


            try
            {
                var logs = new List<string>();
                var summaryList = new List<string>();
                var informationDictionary = new Dictionary<string, List<string>>();
                var user = _uow.Users.Set().Include(x => x.UserType)
                                          .Include(x => x.UserCustomer)
                                          .ThenInclude(x => x.Customer)
                                          .Include(x => x.UserPermission)
                                          .Include(x => x.RecordStatus)
                                          .Include(x => x.CreatedByNavigation)
                                          .Include(x => x.ModifiedByNavigation)
                                          .Include(x => x.DeletedByNavigation)
                                          .FirstOrDefault(x => x.UserId == userId);

                using var stream = new MemoryStream();

                file.CopyTo(stream);
                stream.Position = 0;

                ISheet sheet;

                if (Path.GetExtension(file.FileName).ToLower() == ".xls")
                {
                    var workbook = new HSSFWorkbook(stream); //This will read the Excel 97-2000 format
                    sheet = workbook.GetSheetAt(0); //get first sheet from workbook  
                }
                else
                {
                    var workbook = new XSSFWorkbook(stream); //This will read 2007 Excel format  

                    sheet = workbook.GetSheetAt(0); //get first sheet from workbook  
                }

                var headerRow = sheet.GetRow(0); //Get Header Row

                var cellCount = headerRow.LastCellNum;

                //colon isimlerini Dictionary ile bir listeye atar ve -1 değeri verilir. Kod ilerlediğinde güncellenecektir.
                var importColumnNames = new Dictionary<string, int>
                    {
                        { ExcelTpsNo, -1 },
                        { ExcelTpsDurum, -1 },
                        { ExcelBasvuruTarihi, -1 },
                        { ExcelSureSonuTarihi, -1 },
                        { ExcelGumrukKod, -1 },
                        { ExcelBeyannameNo, -1 },
                        { ExcelBeyannameTarihi, -1 },
                        { ExcelBelgeAd, -1 },
                        { ExcelBelgeSart, -1 },
                        { ExcelTpsAciklama, -1 },
                        { ExcelIthalatciFirma, -1 },
                        { ExcelIhracatciFirma, -1 },
                        { ExcelTpsSiraNo, -1 },
                        { ExcelTpsBeyan, -1 },
                        { ExcelEsyaCinsi, -1 },
                        { ExcelEsyaGtip, -1 },
                        { ExcelFaturaNo, -1 },
                        { ExcelFaturaTarih, -1 },
                        { ExcelFaturaTutar, -1 },
                        { ExcelFaturaDovizKod, -1 },
                        { ExcelMiktar, -1 },
                        { ExcelOlcuBirimi, -1 },
                        { ExcelRejim, -1 },
                        { ExcelCikisRejimi, -1 },
                        { ExcelGidecegiUlke, -1 },
                        { ExcelMenseUlke, -1 },
                        { ExcelSozlesmeUlke, -1 },
                        { ExcelMarka, -1 },
                        { ExcelModel, -1 },
                        { ExcelUrunKod, -1 },
                        { ExcelTPSCikisSiraNo, -1 },
                        { ExcelBeyannameKalemNo, -1 },
                        { ExcelAktarimTarihi, -1 },
                    };


                for (int j = 0; j < cellCount; j++) // header loop
                {
                    var cell = headerRow.GetCell(j);

                    if (cell == null)
                    {
                        continue;
                    }

                    switch (cell.CellType)
                    {
                        case CellType.String: break;

                        case CellType.Numeric:
                        case CellType.Unknown:
                        case CellType.Formula:
                        case CellType.Blank:
                        case CellType.Boolean:
                        case CellType.Error:
                            continue;
                    }

                    var cellValue = cell.ToString();

                    //eğer excelden gelen veri ve dışarıda tanımlanan isim eşit ise j değerini günceller.
                    if (cellValue.Equals(ExcelTpsNo) && importColumnNames.ContainsKey(ExcelTpsNo))
                    {
                        importColumnNames[ExcelTpsNo] = j;
                    }
                    else if (cellValue.Equals(ExcelTpsDurum) && importColumnNames.ContainsKey(ExcelTpsDurum))
                    {
                        importColumnNames[ExcelTpsDurum] = j;
                    }
                    else if (cellValue.Equals(ExcelBasvuruTarihi) && importColumnNames.ContainsKey(ExcelBasvuruTarihi))
                    {
                        importColumnNames[ExcelBasvuruTarihi] = j;
                    }
                    else if (cellValue.Equals(ExcelSureSonuTarihi) && importColumnNames.ContainsKey(ExcelSureSonuTarihi))
                    {
                        importColumnNames[ExcelSureSonuTarihi] = j;
                    }
                    else if (cellValue.Equals(ExcelGumrukKod) && importColumnNames.ContainsKey(ExcelGumrukKod))
                    {
                        importColumnNames[ExcelGumrukKod] = j;
                    }
                    else if (cellValue.Equals(ExcelBeyannameNo) && importColumnNames.ContainsKey(ExcelBeyannameNo))
                    {
                        importColumnNames[ExcelBeyannameNo] = j;
                    }
                    else if (cellValue.Equals(ExcelBeyannameTarihi) && importColumnNames.ContainsKey(ExcelBeyannameTarihi))
                    {
                        importColumnNames[ExcelBeyannameTarihi] = j;
                    }
                    else if (cellValue.Equals(ExcelBelgeAd) && importColumnNames.ContainsKey(ExcelBelgeAd))
                    {
                        importColumnNames[ExcelBelgeAd] = j;
                    }
                    else if (cellValue.Equals(ExcelBelgeSart) && importColumnNames.ContainsKey(ExcelBelgeSart))
                    {
                        importColumnNames[ExcelBelgeSart] = j;
                    }
                    else if (cellValue.Equals(ExcelTpsAciklama) && importColumnNames.ContainsKey(ExcelTpsAciklama))
                    {
                        importColumnNames[ExcelTpsAciklama] = j;
                    }
                    else if (cellValue.Equals(ExcelIthalatciFirma) && importColumnNames.ContainsKey(ExcelIthalatciFirma))
                    {
                        importColumnNames[ExcelIthalatciFirma] = j;
                    }
                    else if (cellValue.Equals(ExcelIhracatciFirma) && importColumnNames.ContainsKey(ExcelIhracatciFirma))
                    {
                        importColumnNames[ExcelIhracatciFirma] = j;
                    }
                    else if (cellValue.Equals(ExcelTpsSiraNo) && importColumnNames.ContainsKey(ExcelTpsSiraNo))
                    {
                        importColumnNames[ExcelTpsSiraNo] = j;
                    }
                    else if (cellValue.Equals(ExcelTpsBeyan) && importColumnNames.ContainsKey(ExcelTpsBeyan))
                    {
                        importColumnNames[ExcelTpsBeyan] = j;
                    }
                    else if (cellValue.Equals(ExcelEsyaCinsi) && importColumnNames.ContainsKey(ExcelEsyaCinsi))
                    {
                        importColumnNames[ExcelEsyaCinsi] = j;
                    }
                    else if (cellValue.Equals(ExcelEsyaGtip) && importColumnNames.ContainsKey(ExcelEsyaGtip))
                    {
                        importColumnNames[ExcelEsyaGtip] = j;
                    }
                    else if (cellValue.Equals(ExcelFaturaNo) && importColumnNames.ContainsKey(ExcelFaturaNo))
                    {
                        importColumnNames[ExcelFaturaNo] = j;
                    }
                    else if (cellValue.Equals(ExcelFaturaTarih) && importColumnNames.ContainsKey(ExcelFaturaTarih))
                    {
                        importColumnNames[ExcelFaturaTarih] = j;
                    }
                    else if (cellValue.Equals(ExcelFaturaTutar) && importColumnNames.ContainsKey(ExcelFaturaTutar))
                    {
                        importColumnNames[ExcelFaturaTutar] = j;
                    }
                    else if (cellValue.Equals(ExcelFaturaDovizKod) && importColumnNames.ContainsKey(ExcelFaturaDovizKod))
                    {
                        importColumnNames[ExcelFaturaDovizKod] = j;
                    }
                    else if (cellValue.Equals(ExcelMiktar) && importColumnNames.ContainsKey(ExcelMiktar))
                    {
                        importColumnNames[ExcelMiktar] = j;
                    }
                    else if (cellValue.Equals(ExcelOlcuBirimi) && importColumnNames.ContainsKey(ExcelOlcuBirimi))
                    {
                        importColumnNames[ExcelOlcuBirimi] = j;
                    }
                    else if (cellValue.Equals(ExcelRejim) && importColumnNames.ContainsKey(ExcelRejim))
                    {
                        importColumnNames[ExcelRejim] = j;
                    }
                    else if (cellValue.Equals(ExcelCikisRejimi) && importColumnNames.ContainsKey(ExcelCikisRejimi))
                    {
                        importColumnNames[ExcelCikisRejimi] = j;
                    }
                    else if (cellValue.Equals(ExcelGidecegiUlke) && importColumnNames.ContainsKey(ExcelGidecegiUlke))
                    {
                        importColumnNames[ExcelGidecegiUlke] = j;
                    }
                    else if (cellValue.Equals(ExcelMenseUlke) && importColumnNames.ContainsKey(ExcelMenseUlke))
                    {
                        importColumnNames[ExcelMenseUlke] = j;
                    }
                    else if (cellValue.Equals(ExcelSozlesmeUlke) && importColumnNames.ContainsKey(ExcelSozlesmeUlke))
                    {
                        importColumnNames[ExcelSozlesmeUlke] = j;
                    }
                    else if (cellValue.Equals(ExcelMarka) && importColumnNames.ContainsKey(ExcelMarka))
                    {
                        importColumnNames[ExcelMarka] = j;
                    }
                    else if (cellValue.Equals(ExcelModel) && importColumnNames.ContainsKey(ExcelModel))
                    {
                        importColumnNames[ExcelModel] = j;
                    }
                    else if (cellValue.Equals(ExcelUrunKod) && importColumnNames.ContainsKey(ExcelUrunKod))
                    {
                        importColumnNames[ExcelUrunKod] = j;
                    }
                    else if (cellValue.Equals(ExcelTPSCikisSiraNo) && importColumnNames.ContainsKey(ExcelTPSCikisSiraNo))
                    {
                        importColumnNames[ExcelTPSCikisSiraNo] = j;
                    }
                    else if (cellValue.Equals(ExcelBeyannameKalemNo) && importColumnNames.ContainsKey(ExcelBeyannameKalemNo))
                    {
                        importColumnNames[ExcelBeyannameKalemNo] = j;
                    }
                    else if (cellValue.Equals(ExcelAktarimTarihi) && importColumnNames.ContainsKey(ExcelAktarimTarihi))
                    {
                        importColumnNames[ExcelAktarimTarihi] = j;
                    }
                }

                if (importColumnNames.Select(x => x.Value).Any(x => x < 0)) // index'i bulunamayan bir kolon bile varsa
                {
                    var list = new List<string>();

                    foreach (var item in importColumnNames.Where(x => x.Value < 0).ToList())
                    {
                        list.Add($"\n\"{item.Key}\" isimli sütun bulunamadı!");
                    }

                    var isKeyContains = informationDictionary.TryGetValue(ImportNoIndexKey, out List<string> noIndexList);

                    if (isKeyContains)
                    {
                        if (noIndexList == null)
                        {
                            noIndexList = new List<string>();
                        }

                        noIndexList.AddRange(list);

                        informationDictionary[ImportNoIndexKey] = noIndexList;
                    }
                    else
                    {
                        informationDictionary.Add(ImportNoIndexKey, list);
                    }

                    return Warning("Beklenen formatta bir excel dosyası verilmedi!" + string.Join("\n", list));

                }

                var stokGirisInsertList = new List<ChepStokGiris>();
                var stokGirisUpdateList = new List<ChepStokGiris>();

                for (int i = sheet.FirstRowNum + 1; i < sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);

                    var cell = row.GetCell(importColumnNames[ExcelAktarimTarihi]);

                    if (cell == null)
                    {   // boş satırları es geç
                        return Warning("Aktarım Tarihi Alanı Boş Geldi! Hiçbir işlem yapılmayacak!");
                    }

                    var cellValue = cell.ToString();

                    if (string.IsNullOrEmpty(cellValue))
                    {
                        return Warning("Aktarım Tarihi Alanı Boş Geldi! Hiçbir işlem yapılmayacak!");
                    }

                    if (!DateTime.TryParse(cell.DateCellValue.ToString(), out DateTime _))
                    {
                        return Warning("Aktarım Tarihi alanı DateTime'a parse edilemedi! Hiçbir işlem yapılmayacak!");
                    }
                }
                
                for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {   // exceli tek tek okumaya başla
                    try
                    {
                        var stokGirisDto = new ChepStokGirisDTO();
                        var detailDto = new ChepStokGirisDetayDTO();

                        if (i > 500)
                        {
                            var tempList = new List<string>();
                            tempList.Insert(0, "Tek seferde işlenecek maksimum veri sayısına ulaşıldı. İşlem durduruldu.");

                            var isKeyContains = informationDictionary.TryGetValue(ImportMaxValueKey, out List<string> valueList);

                            if (isKeyContains)
                            {
                                if (valueList == null)
                                {
                                    valueList = new List<string>();
                                }

                                valueList.AddRange(tempList);

                                informationDictionary[ImportMaxValueKey] = valueList;
                            }
                            else
                            {
                                informationDictionary.Add(ImportMaxValueKey, tempList);
                            }

                            logs.Add("Maximum veri girme değerine ulaşıldı. Program bitirilecek.");

                            break;
                        }

                        var row = sheet.GetRow(i);

                        if (row == null || row.Cells.All(d => d.CellType == CellType.Blank))
                        {   // boş satırları es geç
                            continue;
                        }

                        for (var j = row.FirstCellNum; j < cellCount; j++)
                        {   // satırdaki tüm sütunları okur
                            var cell = row.GetCell(j);

                            if (cell == null)
                            {
                                continue;
                            }
                            //excelden gelen veri tipi numeric ve string ise kodu okumaya devam eder.
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                case CellType.String: break;

                                case CellType.Unknown:
                                case CellType.Formula:
                                case CellType.Blank:
                                case CellType.Boolean:
                                case CellType.Error:
                                    continue;
                            }

                            var cellValue = cell.ToString();

                            //bu kontroller sütün sayısının içindeki isim ve dışarıda tanımlanan ExcelWarehouseName in karşılığına gelen isim eşit ise dto yu doldurmaya yarar.
                            if (j == importColumnNames[ExcelTpsNo])
                            {
                                stokGirisDto.TpsNo = cellValue;
                            }
                            else if (j == importColumnNames[ExcelTpsDurum])
                            {
                                stokGirisDto.TpsDurum = cellValue;
                            }
                            else if (j == importColumnNames[ExcelBasvuruTarihi])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()))
                                {
                                    stokGirisDto.BasvuruTarihi = cell.DateCellValue;
                                }
                            }
                            else if (j == importColumnNames[ExcelSureSonuTarihi])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()))
                                {
                                    stokGirisDto.SureSonuTarihi = cell.DateCellValue;
                                }
                            }
                            else if (j == importColumnNames[ExcelGumrukKod])
                            {
                                stokGirisDto.GumrukKod = cellValue;
                            }
                            else if (j == importColumnNames[ExcelBeyannameNo])
                            {
                                detailDto.BeyannameNo = cellValue;
                            }
                            else if (j == importColumnNames[ExcelBeyannameTarihi])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()))
                                {
                                    detailDto.BeyannameTarihi = cell.DateCellValue;
                                }
                            }
                            else if (j == importColumnNames[ExcelBelgeAd])
                            {
                                stokGirisDto.BelgeAd = cellValue;
                            }
                            else if (j == importColumnNames[ExcelBelgeSart])
                            {
                                stokGirisDto.BelgeSart = cellValue;

                            }
                            else if (j == importColumnNames[ExcelTpsAciklama])
                            {
                                stokGirisDto.TpsAciklama = cellValue;
                            }
                            else if (j == importColumnNames[ExcelIthalatciFirma])
                            {
                                stokGirisDto.IthalatciFirmaName = cellValue;
                            }
                            else if (j == importColumnNames[ExcelIhracatciFirma])
                            {
                                stokGirisDto.IhracatciFirmaName = cellValue;
                            }
                            else if (j == importColumnNames[ExcelTpsSiraNo])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()) && int.TryParse(cellValue, out int tpsSiraNo))
                                {
                                    detailDto.TpsSiraNo = tpsSiraNo;
                                }
                            }
                            else if (j == importColumnNames[ExcelTpsBeyan])
                            {
                                detailDto.TpsBeyan = cellValue;
                            }
                            else if (j == importColumnNames[ExcelEsyaCinsi])
                            {
                                detailDto.EsyaCinsi = cellValue;
                            }
                            else if (j == importColumnNames[ExcelEsyaGtip])
                            {
                                detailDto.EsyaGtip = cellValue;
                            }
                            else if (j == importColumnNames[ExcelFaturaNo])
                            {
                                detailDto.FaturaNo = cellValue;
                            }
                            else if (j == importColumnNames[ExcelFaturaTarih])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()))
                                {
                                    detailDto.FaturaTarih = cell.DateCellValue;
                                }
                            }
                            else if (j == importColumnNames[ExcelFaturaTutar])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()) && decimal.TryParse(cellValue, out decimal faturaTutar))
                                {
                                    detailDto.FaturaTutar = faturaTutar;

                                }
                            }
                            else if (j == importColumnNames[ExcelFaturaDovizKod])
                            {
                                detailDto.FaturaDovizKod = cellValue;
                            }
                            else if (j == importColumnNames[ExcelMiktar])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()) && int.TryParse(cellValue, out int miktar))
                                {
                                    detailDto.Miktar = miktar;

                                }
                            }
                            else if (j == importColumnNames[ExcelOlcuBirimi])
                            {
                                detailDto.OlcuBirimi = cellValue;
                            }
                            else if (j == importColumnNames[ExcelRejim])
                            {
                                detailDto.Rejim = cellValue;
                            }
                            else if (j == importColumnNames[ExcelCikisRejimi])
                            {
                                detailDto.CikisRejimi = cellValue;
                            }
                            else if (j == importColumnNames[ExcelGidecegiUlke])
                            {
                                detailDto.GidecegiUlke = cellValue;
                            }
                            else if (j == importColumnNames[ExcelMenseUlke])
                            {
                                detailDto.MenseUlke = cellValue;
                            }
                            else if (j == importColumnNames[ExcelSozlesmeUlke])
                            {
                                detailDto.SozlesmeUlke = cellValue;
                            }
                            else if (j == importColumnNames[ExcelMarka])
                            {
                                detailDto.Marka = cellValue;
                            }
                            else if (j == importColumnNames[ExcelModel])
                            {
                                detailDto.Model = cellValue;
                            }
                            else if (j == importColumnNames[ExcelUrunKod])
                            {
                                detailDto.UrunKod = cellValue;
                            }
                            else if (j == importColumnNames[ExcelTPSCikisSiraNo])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()) && int.TryParse(cellValue, out int tpsCikisSiraNo))
                                {
                                    detailDto.TpsCikisSiraNo = tpsCikisSiraNo;
                                }
                            }
                            else if (j == importColumnNames[ExcelBeyannameKalemNo])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()) && int.TryParse(cellValue, out int beyannameKalemNo))
                                {
                                    detailDto.BeyannameKalemNo = beyannameKalemNo;
                                }
                            }
                            else if (j == importColumnNames[ExcelAktarimTarihi])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()))
                                {
                                    stokGirisDto.AktarimTarihi = cell.DateCellValue;
                                }
                            }
                        }


                        if (stokGirisDto.TpsNo == null)
                        {
                            continue;
                        }
                        if (detailDto.TpsSiraNo == null)
                        {
                            continue;
                        }
                        var existlistdeneme = stokGirisInsertList.Any(x => x.TpsNo == stokGirisDto.TpsNo);
                        var existStokGirisEntities = _uow.ChepStokGiris.Search(x => x.TpsNo == stokGirisDto.TpsNo);
                        var existStokGirisEntity = existStokGirisEntities.FirstOrDefault();
                        if (existStokGirisEntity != null)
                        {
                            var deneme = _uow.ChepStokGiris.Search(x => x.TpsNo == stokGirisDto.TpsNo && x.AktarimTarihi != null && x.AktarimTarihi >= stokGirisDto.AktarimTarihi);
                            var denemefirst = deneme.FirstOrDefault();
                            if (denemefirst != null)
                            {
                                continue;
                            }
                        }
                        Customs customs = null;
                        if (stokGirisDto.GumrukKod != null)
                        {
                            var customsEntities = _uow.Customs.Search(x => x.EdiCode == stokGirisDto.GumrukKod);
                            //eğer Customs tablosunda exceldeki GumrukKod ile eşleşen alan yoksa başa dön
                            if (customsEntities.Count != 1)
                            {
                                var tempList = new List<string>();

                                if (customsEntities.Count == 0) // beyanname no bulunmayınca
                                {
                                    tempList.Add($"Gumruk Kod Bulunamadı. Gumruk Kod:{stokGirisDto.GumrukKod}");
                                    logs.Add($"Gumruk Kod Bulunamadı. Gumruk Kod:{stokGirisDto.GumrukKod}.");

                                    // dicitonary'den "GumrukKod bulunamadı" tipindeki listeyi getir.
                                    var isKeyContains = informationDictionary.TryGetValue(ImportNoFileMasterKey, out List<string> valueList);

                                    if (isKeyContains)
                                    {
                                        if (valueList == null)
                                        {
                                            valueList = new List<string>();
                                        }

                                        valueList.AddRange(tempList);

                                        informationDictionary[ImportNoFileMasterKey] = valueList;
                                    }
                                    else
                                    {
                                        informationDictionary.Add(ImportNoFileMasterKey, tempList);
                                    }
                                }
                                else if (customsEntities.Count > 1) // birden çok beyanname no bulunnunca
                                {
                                    tempList.Add($"Gumruk Kod beklenenden fazla değer döndü. GumrukKod:{stokGirisDto.GumrukKod}");
                                    logs.Add($"Gumruk Kod beklenenden fazla değer döndü. GumrukKod:{stokGirisDto.GumrukKod}.");

                                    // dicitonary'den "Beklenenden fazla değer döndü" tipindeki listeyi getir.
                                    var isKeyContains = informationDictionary.TryGetValue(ImportMultipleFileMasterKey, out List<string> valueList);

                                    if (isKeyContains)
                                    {
                                        if (valueList == null)
                                        {
                                            valueList = new List<string>();
                                        }

                                        valueList.AddRange(tempList);

                                        informationDictionary[ImportMultipleFileMasterKey] = valueList;
                                    }
                                    else
                                    {
                                        informationDictionary.Add(ImportMultipleFileMasterKey, tempList);
                                    }
                                }
                            }
                            customs = customsEntities.FirstOrDefault();
                        }

                        //var ithalatcıFirma = new Customer();
                        Customer ithalatcıFirma = null;
                        if (stokGirisDto.IthalatciFirmaName != null)
                        {
                            var ithalatcıFirmaEntities = _uow.Customers.Search(x => x.Name.Trim().ToUpper() == stokGirisDto.IthalatciFirmaName.Trim().ToUpper() || x.TaxNo.Trim().ToUpper() == stokGirisDto.IthalatciFirmaName.Trim().ToUpper());
                            if (ithalatcıFirmaEntities.Count != 1)
                            {
                                var tempList = new List<string>();
                                if (ithalatcıFirmaEntities.Count == 0)
                                {
                                    tempList.Add($"IthalatciFirma Bulunamadı. IthalatciFirma:{stokGirisDto.IthalatciFirmaName}");
                                    logs.Add($"IthalatciFirma Bulunamadı. IthalatciFirma:{stokGirisDto.IthalatciFirmaName}.");

                                    var isKeyContains = informationDictionary.TryGetValue(ImportNoContractKey, out List<string> valueList);

                                    if (isKeyContains)
                                    {
                                        if (valueList == null)
                                        {
                                            valueList = new List<string>();
                                        }

                                        valueList.AddRange(tempList);

                                        informationDictionary[ImportNoContractKey] = valueList;
                                    }
                                    else
                                    {
                                        informationDictionary.Add(ImportNoContractKey, tempList);
                                    }
                                }
                            }
                            ithalatcıFirma = ithalatcıFirmaEntities.FirstOrDefault();

                        }

                        //var ihracatciFirma = new Customer();
                        Customer ihracatciFirma = null;
                        if (stokGirisDto.IhracatciFirmaName != null)
                        {
                            var ihracatciFirmaEntities = _uow.Customers.Search(x => x.Name.Trim().ToUpper() == stokGirisDto.IhracatciFirmaName.Trim().ToUpper() || x.TaxNo.Trim() == stokGirisDto.IhracatciFirmaName.Trim());
                            if (ihracatciFirmaEntities.Count != 1)
                            {
                                var tempList = new List<string>();
                                if (ihracatciFirmaEntities.Count == 0)
                                {
                                    tempList.Add($"IhracatciFirmaName Bulunamadı. IhracatciFirmaName:{stokGirisDto.IhracatciFirmaName}");
                                    logs.Add($"IhracatciFirmaName Bulunamadı. IhracatciFirmaName:{stokGirisDto.IhracatciFirmaName}.");

                                    var isKeyContains = informationDictionary.TryGetValue(ImportNoContractKey, out List<string> valueList);

                                    if (isKeyContains)
                                    {
                                        if (valueList == null)
                                        {
                                            valueList = new List<string>();
                                        }

                                        valueList.AddRange(tempList);

                                        informationDictionary[ImportNoContractKey] = valueList;
                                    }
                                    else
                                    {
                                        informationDictionary.Add(ImportNoContractKey, tempList);
                                    }
                                }

                            }

                            ihracatciFirma = ihracatciFirmaEntities.FirstOrDefault();

                        }

                        var sozlesmeUlke = "";
                        if (detailDto.SozlesmeUlke != null)
                        {
                            var sozlesmeCounryEntities = _uow.Country.Search(x => x.IsoCode.Trim().ToUpper() == detailDto.SozlesmeUlke.Trim().ToUpper() || x.EdiCode == detailDto.SozlesmeUlke);
                            if (sozlesmeCounryEntities.Count != 1)
                            {
                                var tempList = new List<string>();
                                if (sozlesmeCounryEntities.Count == 0)
                                {
                                    tempList.Add($"SozlesmeUlke Bulunamadı. SozlesmeUlke:{detailDto.SozlesmeUlke}");
                                    logs.Add($"SozlesmeUlke Bulunamadı. SozlesmeUlke:{detailDto.SozlesmeUlke}.");

                                    var isKeyContains = informationDictionary.TryGetValue(ImportNoContractKey, out List<string> valueList);

                                    if (isKeyContains)
                                    {
                                        if (valueList == null)
                                        {
                                            valueList = new List<string>();
                                        }

                                        valueList.AddRange(tempList);

                                        informationDictionary[ImportNoContractKey] = valueList;
                                    }
                                    else
                                    {
                                        informationDictionary.Add(ImportNoContractKey, tempList);
                                    }
                                }

                            }
                            sozlesmeUlke = sozlesmeCounryEntities.FirstOrDefault()?.EdiCode;
                        }

                        var gidecegiUlke = "";
                        if (detailDto.GidecegiUlke != null)
                        {
                            var gidecegiUlkeEntities = _uow.Country.Search(x => x.IsoCode.Trim().ToUpper() == detailDto.GidecegiUlke.Trim().ToUpper() || x.EdiCode == detailDto.GidecegiUlke);
                            if (gidecegiUlkeEntities.Count != 1)
                            {
                                var tempList = new List<string>();
                                if (gidecegiUlkeEntities.Count == 0)
                                {
                                    tempList.Add($"GidecegiUlke Bulunamadı. GidecegiUlke:{detailDto.GidecegiUlke}");
                                    logs.Add($"GidecegiUlke Bulunamadı. GidecegiUlke:{detailDto.GidecegiUlke}.");

                                    var isKeyContains = informationDictionary.TryGetValue(ImportNoContractKey, out List<string> valueList);

                                    if (isKeyContains)
                                    {
                                        if (valueList == null)
                                        {
                                            valueList = new List<string>();
                                        }

                                        valueList.AddRange(tempList);

                                        informationDictionary[ImportNoContractKey] = valueList;
                                    }
                                    else
                                    {
                                        informationDictionary.Add(ImportNoContractKey, tempList);
                                    }
                                }
                            }
                            gidecegiUlke = gidecegiUlkeEntities.FirstOrDefault()?.EdiCode;
                        }

                        var menseUlke = "";
                        if (detailDto.MenseUlke != null)
                        {
                            var menseUlkeEntities = _uow.Country.Search(x => x.IsoCode.Trim().ToUpper() == detailDto.MenseUlke.Trim().ToUpper() || x.EdiCode == detailDto.MenseUlke);
                            //eğer MenseUlke tablosunda exceldek MenseUlke ile eşleşen alan yoksa başa dön
                            if (menseUlkeEntities.Count != 1)
                            {
                                var tempList = new List<string>();
                                if (menseUlkeEntities.Count == 0)
                                {
                                    tempList.Add($"MenseUlke Bulunamadı. MenseUlke:{detailDto.MenseUlke}");
                                    logs.Add($"MenseUlke Bulunamadı. MenseUlke:{detailDto.MenseUlke}.");

                                    // dicitonary'den "Sözleşme No Bulunamadı" tipindeki listeyi getir.
                                    var isKeyContains = informationDictionary.TryGetValue(ImportNoContractKey, out List<string> valueList);

                                    if (isKeyContains)
                                    {
                                        if (valueList == null)
                                        {
                                            valueList = new List<string>();
                                        }

                                        valueList.AddRange(tempList);

                                        informationDictionary[ImportNoContractKey] = valueList;
                                    }
                                    else
                                    {
                                        informationDictionary.Add(ImportNoContractKey, tempList);
                                    }
                                }

                            }

                            menseUlke = menseUlkeEntities.FirstOrDefault()?.EdiCode;
                        }

                        Product product = null;
                        var productEntities = new List<Product>();
                        if (detailDto.UrunKod != null)
                        {
                            //GoodsName excelden gelir veritabanındaki ile karşılaştırır eşit olanları getirir.
                            productEntities = _uow.Products.Search(x => x.ProductNo.Trim().ToUpper() == detailDto.UrunKod.Trim().ToUpper());
                            //eğer product tablosunda aynı adla 1'den fazla veri var ise başa dön kullanıcıya uyarı ver.
                            if (productEntities.Count > 1)
                            {
                                var tempList = new List<string>();
                                //product nameleri al aralara virgül koy ve string olarak dön
                                var ambiguousValues = string.Join(",", productEntities.Select(x => x.ProductNo));

                                tempList.Add($"TPS No: {stokGirisDto.TpsNo}.{detailDto.UrunKod} adlı \"Ürün\" için birden fazla değer döndü. Kararsız kalınan değerler: {ambiguousValues}");
                                logs.Add($"TPS No: {stokGirisDto.TpsNo}.{detailDto.UrunKod} adlı \"Ürün\" için birden fazla değer döndü. Kararsız kalınan değerler: {ambiguousValues}.");

                                var isKeyContains = informationDictionary.TryGetValue(ImportMultipleProductKey, out List<string> valueList);

                                if (isKeyContains)
                                {
                                    if (valueList == null)
                                    {
                                        valueList = new List<string>();
                                    }

                                    valueList.AddRange(tempList);

                                    informationDictionary[ImportMultipleProductKey] = valueList;
                                }
                                else
                                {
                                    informationDictionary.Add(ImportMultipleProductKey, tempList);
                                }

                                continue;
                            }

                            product = productEntities.FirstOrDefault();
                            //eğer product tablosunda tanım ile eşleşen yok ise tabloya insert atar.
                            if (product == null)
                            {
                                var GuidKey = Guid.NewGuid();
                                product = _uow.Products.Add(new Product
                                {
                                    ProductId = GuidKey,
                                    ProductNo = detailDto.UrunKod.ToUpper(),
                                    HsCode = detailDto.EsyaGtip,
                                    CreatedDate = DateTime.Now,
                                    RecordStatusId = 1,
                                    ProductNameTr = detailDto.EsyaCinsi,
                                    CustomerId = ihracatciFirma.CustomerId
                                });

                                _uow.Commit();
                                if (product == null || product.ProductId == null)
                                {
                                    continue;
                                }
                                logs.Add($"Ürünler tablosuna da ekleme yapıldı.Ekleme yapılan değer : \"{product.ProductNo}\".");
                            }
                        }


                        if (stokGirisInsertList.Any(x => x.TpsNo == stokGirisDto.TpsNo))//INSERT
                        {
                            var endDetayList = stokGirisInsertList.LastOrDefault().ChepStokGirisDetay.LastOrDefault();
                            //kayıt var ise ilk satırı alır
                            //TpsSira no alanı exceldeki önceki alanla eşit ise eklemeyi yapma
                            if (endDetayList.TpsSiraNo == detailDto.TpsSiraNo && endDetayList.FaturaNo == detailDto.FaturaNo
                                && endDetayList.BeyannameNo == detailDto.BeyannameNo && endDetayList.BeyannameTarihi == detailDto.BeyannameTarihi && endDetayList.BeyannameKalemNo == detailDto.BeyannameKalemNo
                                             && endDetayList.FaturaTutar == detailDto.FaturaTutar && endDetayList.FaturaDovizKod == detailDto.FaturaDovizKod && detailDto.Miktar == null//yeni alandan gelen miktar boş değil ise devam etmeli
                                             && endDetayList.OlcuBirimi == detailDto.OlcuBirimi && endDetayList.UrunKod == product.ProductNo)
                            {
                                continue;
                            }

                            var existItemMasterGiris = stokGirisInsertList.FirstOrDefault(x => x.TpsNo == stokGirisDto.TpsNo);

                            if (existItemMasterGiris != null && existItemMasterGiris.ChepStokGirisDetay != null && existItemMasterGiris.ChepStokGirisDetay.Count > 0
                                && existItemMasterGiris.ChepStokGirisDetay.Any(x => x.BeyannameKalemNo == detailDto.BeyannameKalemNo && x.BeyannameNo == detailDto.BeyannameNo
                                && x.FaturaDovizKod == detailDto.FaturaDovizKod && x.FaturaNo == detailDto.FaturaNo && x.OlcuBirimi == detailDto.OlcuBirimi
                                && x.MenseUlke == menseUlke && x.BeyannameTarihi == detailDto.BeyannameTarihi && x.TpsSiraNo == detailDto.TpsSiraNo && x.FaturaTutar == detailDto.FaturaTutar && x.UrunKod == product.ProductNo) == false)
                            {
                                existItemMasterGiris.ChepStokGirisDetay.Add(new ChepStokGirisDetay
                                {
                                    TpsSiraNo = detailDto.TpsSiraNo,
                                    EsyaGtip = detailDto.EsyaGtip,
                                    TpsBeyan = detailDto.TpsBeyan,
                                    SozlesmeUlke = sozlesmeUlke,
                                    Rejim = detailDto.Rejim,
                                    OlcuBirimi = detailDto.OlcuBirimi,
                                    CikisRejimi = detailDto.CikisRejimi,
                                    PoNo = detailDto.PoNo,
                                    Miktar = detailDto.Miktar,
                                    Model = detailDto.Model,
                                    TpsCikisSiraNo = detailDto.TpsCikisSiraNo,
                                    BeyannameKalemNo = detailDto.BeyannameKalemNo,
                                    MenseUlke = menseUlke,
                                    Marka = detailDto.Marka,
                                    GidecegiUlke = gidecegiUlke,
                                    FaturaTutar = detailDto.FaturaTutar,
                                    EsyaCinsi = detailDto.EsyaCinsi,
                                    FaturaDovizKod = detailDto.FaturaDovizKod,
                                    FaturaNo = detailDto.FaturaNo,
                                    FaturaTarih = detailDto.FaturaTarih,
                                    UrunKod = product.ProductNo,
                                    BeyannameNo = detailDto.BeyannameNo,
                                    BeyannameTarihi = detailDto.BeyannameTarihi,

                                });

                            }
                            else
                            {
                                var insertListDetailUpdate = existItemMasterGiris.ChepStokGirisDetay.FirstOrDefault(x => x.BeyannameKalemNo == detailDto.BeyannameKalemNo && x.BeyannameNo == detailDto.BeyannameNo
                                && x.FaturaDovizKod == detailDto.FaturaDovizKod && x.FaturaNo == detailDto.FaturaNo && x.OlcuBirimi == detailDto.OlcuBirimi
                                && x.MenseUlke == menseUlke && x.BeyannameTarihi == detailDto.BeyannameTarihi && x.TpsSiraNo == detailDto.TpsSiraNo && x.FaturaTutar == detailDto.FaturaTutar && x.UrunKod == product.ProductNo);

                                if (insertListDetailUpdate != null && insertListDetailUpdate.Miktar != null && detailDto.Miktar != null)//detaildto null olduğu durumda diğerini de null yapıyor.
                                {
                                    insertListDetailUpdate.Miktar += detailDto.Miktar;
                                }
                                else if (insertListDetailUpdate != null && insertListDetailUpdate.Miktar == null && detailDto.Miktar != null)
                                {
                                    insertListDetailUpdate.Miktar = detailDto.Miktar;
                                }
                            }
                        }

                        else if (stokGirisUpdateList.Any(x => x.TpsNo == stokGirisDto.TpsNo) && existStokGirisEntity != null)
                        {
                            var existStokGirisDetayEntities = _uow.ChepStokGirisDetay.Search(x => x.StokGirisId == existStokGirisEntity.StokGirisId);
                            var existStokGirisDetayEntity = existStokGirisDetayEntities.FirstOrDefault();

                            var oldEntityGirisDetay = _uow.ChepStokGirisDetay.Set()
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId
                                             && x.BeyannameNo == detailDto.BeyannameNo && x.BeyannameTarihi == detailDto.BeyannameTarihi && x.BeyannameKalemNo == detailDto.BeyannameKalemNo
                                             && x.FaturaTutar == detailDto.FaturaTutar && x.FaturaDovizKod == detailDto.FaturaDovizKod && x.FaturaNo == detailDto.FaturaNo
                                             && x.OlcuBirimi == detailDto.OlcuBirimi && x.UrunKod == product.ProductNo && x.TpsSiraNo == detailDto.TpsSiraNo);

                            ChepStokGiris stokGirisEntity = null;

                            if (oldEntityGirisDetay == null) // giriş detay bulunamadı!
                            {
                                stokGirisEntity = _uow.ChepStokGiris.Set()
                                    .Include(x => x.ChepStokGirisDetay)
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId);
                                if (stokGirisEntity == null)
                                {
                                    continue;
                                }

                                var detayinsert = new ChepStokGirisDetay
                                {
                                    StokGirisId = stokGirisEntity.StokGirisId,
                                    TpsBeyan = detailDto.TpsBeyan,
                                    SozlesmeUlke = sozlesmeUlke,
                                    Rejim = detailDto.Rejim,
                                    CikisRejimi = detailDto.CikisRejimi,
                                    PoNo = detailDto.PoNo,
                                    Model = detailDto.Model,
                                    TpsCikisSiraNo = detailDto.TpsCikisSiraNo,
                                    BeyannameKalemNo = detailDto.BeyannameKalemNo,
                                    MenseUlke = menseUlke,
                                    Marka = detailDto.Marka,
                                    GidecegiUlke = gidecegiUlke,
                                    EsyaCinsi = detailDto.EsyaCinsi,
                                    FaturaTarih = detailDto.FaturaTarih,
                                    UrunKod = product.ProductNo,
                                    TpsSiraNo = detailDto.TpsSiraNo,
                                    BeyannameTarihi = detailDto.BeyannameTarihi,
                                    BeyannameNo = detailDto.BeyannameNo,
                                    EsyaGtip = detailDto.EsyaGtip,
                                    FaturaNo = detailDto.FaturaNo,
                                    FaturaDovizKod = detailDto.FaturaDovizKod,
                                    FaturaTutar = detailDto.FaturaTutar,
                                    Miktar = detailDto.Miktar,
                                    OlcuBirimi = detailDto.OlcuBirimi,

                                };
                                _uow.ChepStokGirisDetay.Add(detayinsert);
                                _uow.Commit();
                            }
                            else
                            {
                                stokGirisEntity = oldEntityGirisDetay.StokGiris;
                            }

                            if (oldEntityGirisDetay != null && oldEntityGirisDetay.StokGirisId > 0 && detailDto.Miktar.HasValue)
                            {
                                if (oldEntityGirisDetay.Miktar != null && detailDto.Miktar != null)
                                {
                                    oldEntityGirisDetay.Miktar += detailDto.Miktar;
                                }
                                else if (oldEntityGirisDetay.Miktar == null && detailDto.Miktar != null)
                                {
                                    oldEntityGirisDetay.Miktar = detailDto.Miktar;
                                }
                                var addition83indetailUpdate = _uow.ChepStokGirisDetay.Update(oldEntityGirisDetay);
                                _uow.Commit();

                                continue;
                            }

                        }


                        //Excelde aynı fileMasterId var ise detay için devam eder.
                        if (existlistdeneme == false && existStokGirisEntities.Count == 0)
                        {

                            var stokGirisEntity = new ChepStokGiris
                            {
                                ReferansNo = Convert.ToInt32(_definitionService.GetNextReferenceNumber("Giris").Result) + stokGirisInsertList.Count,
                                GumrukKod = customs?.EdiCode,
                                BelgeAd = stokGirisDto.BelgeAd,
                                BelgeSart = stokGirisDto.BelgeSart,
                                IhracatciFirma = ihracatciFirma?.CustomerId,
                                TpsNo = stokGirisDto.TpsNo,
                                KapAdet = stokGirisDto.KapAdet,
                                IthalatciFirma = ithalatcıFirma?.CustomerId,
                                SureSonuTarihi = stokGirisDto.SureSonuTarihi,
                                TpsAciklama = stokGirisDto.TpsAciklama,
                                TpsDurum = stokGirisDto.TpsDurum,
                                BasvuruTarihi = stokGirisDto.BasvuruTarihi,
                                AktarimTarihi = stokGirisDto.AktarimTarihi,
                            };


                            stokGirisEntity.ChepStokGirisDetay = new List<ChepStokGirisDetay>();
                            //birden fazla aynı beyanname no varsa burda ilk detay insertini atar. diğer detay insertlerini 1011. satırdaki ifte atar. 
                            var stokGirisDetay = new ChepStokGirisDetay
                            {
                                UrunKod = product?.ProductNo,
                                TpsSiraNo = detailDto.TpsSiraNo,
                                TpsBeyan = detailDto.TpsBeyan,
                                BeyannameNo = detailDto.BeyannameNo,
                                BeyannameTarihi = detailDto.BeyannameTarihi,
                                //StokGirisId = detailDto.StokGirisId,
                                //StokGirisDetayId = detailDto.StokGirisDetayId,
                                SozlesmeUlke = sozlesmeUlke,
                                Rejim = detailDto.Rejim,
                                OlcuBirimi = detailDto.OlcuBirimi,
                                CikisRejimi = detailDto.CikisRejimi,
                                PoNo = detailDto.PoNo,
                                Miktar = detailDto.Miktar,
                                Model = detailDto.Model,
                                BeyannameKalemNo = detailDto.BeyannameKalemNo,
                                TpsCikisSiraNo = detailDto.TpsCikisSiraNo,
                                MenseUlke = menseUlke,
                                Marka = detailDto.Marka,
                                GidecegiUlke = gidecegiUlke,
                                FaturaTutar = detailDto.FaturaTutar,
                                EsyaCinsi = detailDto.EsyaCinsi,
                                EsyaGtip = detailDto.EsyaGtip,
                                FaturaDovizKod = detailDto.FaturaDovizKod,
                                FaturaNo = detailDto.FaturaNo,
                                FaturaTarih = detailDto.FaturaTarih,
                            };

                            stokGirisEntity.ChepStokGirisDetay.Add(stokGirisDetay);

                            stokGirisInsertList.Add(stokGirisEntity);

                        }
                        //tps noya göre dbde veri var ise update için buraya girer
                        else if (existStokGirisEntities.Count > 0)
                        {
                            var oldEntityGirisDetay = _uow.ChepStokGirisDetay.Set()
                                             .Include(x => x.StokGiris)
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId
                                            && x.BeyannameNo == detailDto.BeyannameNo && x.BeyannameTarihi == detailDto.BeyannameTarihi && x.BeyannameKalemNo == detailDto.BeyannameKalemNo
                                             && x.FaturaTutar == detailDto.FaturaTutar && x.FaturaDovizKod == detailDto.FaturaDovizKod && x.FaturaNo == detailDto.FaturaNo
                                             && x.OlcuBirimi == detailDto.OlcuBirimi && x.UrunKod == product.ProductNo && x.TpsSiraNo == detailDto.TpsSiraNo);

                            ChepStokGiris stokGirisEntity = null;

                            if (oldEntityGirisDetay == null) // giriş detay bulunamadı!
                            {
                                stokGirisEntity = _uow.ChepStokGiris.Set().AsNoTracking()
                                    .Include(x => x.ChepStokGirisDetay)
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId);

                                if (stokGirisEntity == null)
                                {
                                    continue;
                                }

                                var detayinsert = new ChepStokGirisDetay
                                {
                                    StokGirisId = stokGirisEntity.StokGirisId,
                                    TpsBeyan = detailDto.TpsBeyan,
                                    SozlesmeUlke = sozlesmeUlke,
                                    Rejim = detailDto.Rejim,
                                    CikisRejimi = detailDto.CikisRejimi,
                                    PoNo = detailDto.PoNo,
                                    Model = detailDto.Model,
                                    TpsCikisSiraNo = detailDto.TpsCikisSiraNo,
                                    BeyannameKalemNo = detailDto.BeyannameKalemNo,
                                    MenseUlke = menseUlke,
                                    Marka = detailDto.Marka,
                                    GidecegiUlke = gidecegiUlke,
                                    EsyaCinsi = detailDto.EsyaCinsi,
                                    FaturaTarih = detailDto.FaturaTarih,
                                    UrunKod = product.ProductNo,
                                    TpsSiraNo = detailDto.TpsSiraNo,
                                    BeyannameTarihi = detailDto.BeyannameTarihi,
                                    BeyannameNo = detailDto.BeyannameNo,
                                    EsyaGtip = detailDto.EsyaGtip,
                                    FaturaNo = detailDto.FaturaNo,
                                    FaturaDovizKod = detailDto.FaturaDovizKod,
                                    FaturaTutar = detailDto.FaturaTutar,
                                    Miktar = detailDto.Miktar,
                                    OlcuBirimi = detailDto.OlcuBirimi,

                                };

                                _uow.ChepStokGirisDetay.Add(detayinsert);
                                _uow.Commit();
                            }
                            else
                            {
                                stokGirisEntity = oldEntityGirisDetay.StokGiris;
                            }

                            if (!stokGirisUpdateList.Any(x => x.StokGirisId == existStokGirisEntity.StokGirisId))
                            {
                                existStokGirisEntity.AktarimTarihi = stokGirisDto.AktarimTarihi;
                                if (oldEntityGirisDetay != null && oldEntityGirisDetay.StokGirisDetayId > 0 && detailDto.Miktar.HasValue)
                                {
                                    if (oldEntityGirisDetay.Miktar.HasValue && detailDto.Miktar != null)
                                    {
                                        oldEntityGirisDetay.Miktar += detailDto.Miktar;
                                    }
                                    else if (!oldEntityGirisDetay.Miktar.HasValue && detailDto.Miktar != null)
                                    {
                                        oldEntityGirisDetay.Miktar = detailDto.Miktar;
                                    }
                                    var stokGirisDetailUpdate = _uow.ChepStokGirisDetay.Update(oldEntityGirisDetay);
                                    _uow.Commit();
                                }
                                stokGirisUpdateList.Add(existStokGirisEntity);
                            }
                        }
                        //bir hata var ise string tipinde virgül ile listeler ve yazdırır.
                        else
                        {
                            var tempList = new List<string>();

                            var ambiguousValues = string.Join(",", productEntities.Select(x => x.ProductNo));

                            tempList.Add($"Aynı TPS No için {i}. satırda veri bulundu. Detaya kayıt atılacak ya da güncelleme işlemi yapılacak. TPS No: {stokGirisDto.TpsNo}");

                            var isKeyContains = informationDictionary.TryGetValue(ImportNoProductKey, out List<string> valueList);

                            if (isKeyContains)
                            {
                                if (valueList == null)
                                {
                                    valueList = new List<string>();
                                }

                                valueList.AddRange(tempList);

                                informationDictionary[ImportNoProductKey] = valueList;
                            }
                            else
                            {
                                informationDictionary.Add(ImportNoProductKey, tempList);
                            }
                            continue;
                        }

                    }
                    catch (Exception ex)
                    {
                        return Error(ex);
                    }
                } // END FOR
                if (stokGirisInsertList != null && stokGirisInsertList.Count > 0)
                {

                    var resultInsert = _uow.ChepStokGiris.AddRange(stokGirisInsertList);
                    _uow.Commit();
                    if (resultInsert.Count > 0)
                    {
                        summaryList.Insert(0, $"{resultInsert.Count} adet yeni kayıt atıldı.");
                    }
                }

                if (stokGirisUpdateList != null && stokGirisUpdateList.Count > 0)
                {
                    //foreach (var item in stokGirisUpdateList)
                    //{
                    //    foreach (var item1 in item.ChepStokGirisDetay)
                    //    {
                    //        if (item1.StokGirisDetayId > 0)
                    //        {
                    //            _uow.ChepStokGirisDetay.Update(item1);
                    //        }
                    //        else
                    //        {
                    //            _uow.ChepStokGirisDetay.Add(item1);
                    //        }
                    //    }
                    //}
                    var resultUpdate = _uow.ChepStokGiris.UpdateRange(stokGirisUpdateList);
                    _uow.Commit();
                    if (resultUpdate.Count > 0)
                    {
                        summaryList.Insert(0, $"{resultUpdate.Count} adet güncelleme yapıldı.");
                    }
                }
                foreach (var item in logs)
                {
                    summaryList.Add($"\n {item}");
                }

                return Success(summaryList);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion
    }
}