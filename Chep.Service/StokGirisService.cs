﻿using AutoMapper;
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
                BeyannameKalemNo = obj.BeyannameKalemNo
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
        private const string ExcelTPSCikisSiraNo = "TPS Cikis Sira No";
        private const string ExcelBeyannameKalemNo = "Beyanname Kalem No";

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
                    logs.Add("excel dosyası okundu.Sheet 1 alındı.");
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

                    logs.Add($"{cellValue} sütunu algılandı.Headers okunuyor.");

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
                }

                if (importColumnNames.Select(x => x.Value).Any(x => x < 0)) // index'i bulunamayan bir kolon bile varsa
                {
                    var list = new List<string>();

                    foreach (var item in importColumnNames.Where(x => x.Value < 0).ToList())
                    {
                        list.Add($"{item.Key} isimli sütun bulunamadı!");
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
                var stokGirisDto = new ChepStokGirisDTO();
                var detailDto = new ChepStokGirisDetayDTO();


                for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {   // exceli tek tek okumaya başla
                    try
                    {
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

                        logs.Add($"{i}. satır okunmaya başlandı.");

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

                            logs.Add($"{i}. satırda cellerin içindeki veriler okunuyor.Şuan {j}. cell okunuyor. Ve Dto'lar ile eşleştiriliyor.");

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
                                stokGirisDto.BeyannameNo = cellValue;
                            }
                            else if (j == importColumnNames[ExcelBeyannameTarihi])
                            {
                                if (!string.IsNullOrEmpty(cellValue.Trim()))
                                {
                                    stokGirisDto.BeyannameTarihi = cell.DateCellValue;
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
                        }
                        var existlistdeneme = stokGirisInsertList.Any(x => x.BeyannameNo == stokGirisDto.BeyannameNo && x.TpsNo == stokGirisDto.TpsNo);
                        var existStokGirisEntities = _uow.ChepStokGiris.Search(x => x.TpsNo == stokGirisDto.TpsNo && x.BeyannameNo == stokGirisDto.BeyannameNo);
                        var existStokGirisEntity = existStokGirisEntities.FirstOrDefault();

                        logs.Add($"Exceldeki alanlar Dto ile eşleştirildi! DB'ye olan verilerin daha önce kayıt edilmemesi için bakıldı.");



                        var customsEntities = _uow.Customs.Search(x => x.EdiCode == stokGirisDto.GumrukKod);
                        //eğer Customs tablosunda exceldeki GumrukKod ile eşleşen alan yoksa başa dön
                        if (customsEntities.Count != 1)
                        {
                            var tempList = new List<string>();

                            if (customsEntities.Count == 0) // beyanname no bulunmayınca
                            {
                                tempList.Add($"Gumruk Kod Bulunamadı. Gumruk Kod:{stokGirisDto.GumrukKod}");
                                logs.Add($"Gumruk Kod Bulunamadı. Gumruk Kod:{stokGirisDto.GumrukKod}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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
                                logs.Add($"Gumruk Kod beklenenden fazla değer döndü. GumrukKod:{stokGirisDto.GumrukKod}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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
                        var customs = customsEntities.FirstOrDefault();

                        var ithalatcıFirmaEntities = _uow.Customers.Search(x => x.Name.Trim().ToUpper() == stokGirisDto.IthalatciFirmaName.Trim().ToUpper() || x.TaxNo.Trim().ToUpper() == stokGirisDto.IthalatciFirmaName.Trim().ToUpper());
                        if (ithalatcıFirmaEntities.Count != 1)
                        {
                            var tempList = new List<string>();
                            if (ithalatcıFirmaEntities.Count == 0)
                            {
                                tempList.Add($"IthalatciFirma Bulunamadı. IthalatciFirma:{stokGirisDto.IthalatciFirmaName}");
                                logs.Add($"IthalatciFirma Bulunamadı. IthalatciFirma:{stokGirisDto.IthalatciFirmaName}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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

                        var ithalatcıFirma = ithalatcıFirmaEntities.FirstOrDefault();

                        var ihracatciFirmaEntities = _uow.Customers.Search(x => x.Name.Trim().ToUpper() == stokGirisDto.IhracatciFirmaName.Trim().ToUpper() || x.TaxNo.Trim().ToUpper() == stokGirisDto.IhracatciFirmaName.Trim().ToUpper());
                        if (ihracatciFirmaEntities.Count != 1)
                        {
                            var tempList = new List<string>();
                            if (ihracatciFirmaEntities.Count == 0)
                            {
                                tempList.Add($"IhracatciFirmaName Bulunamadı. IhracatciFirmaName:{stokGirisDto.IhracatciFirmaName}");
                                logs.Add($"IhracatciFirmaName Bulunamadı. IhracatciFirmaName:{stokGirisDto.IhracatciFirmaName}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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

                        var ihracatciFirma = ihracatciFirmaEntities.FirstOrDefault();

                        var sozlesmeCounryEntities = _uow.Country.Search(x => x.IsoCode.Trim().ToUpper() == detailDto.SozlesmeUlke.Trim().ToUpper() || x.EdiCode == detailDto.SozlesmeUlke);
                        if (sozlesmeCounryEntities.Count != 1)
                        {
                            var tempList = new List<string>();
                            if (sozlesmeCounryEntities.Count == 0)
                            {
                                tempList.Add($"SozlesmeUlke Bulunamadı. SozlesmeUlke:{detailDto.SozlesmeUlke}");
                                logs.Add($"SozlesmeUlke Bulunamadı. SozlesmeUlke:{detailDto.SozlesmeUlke}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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

                        var sozlesmeUlke = sozlesmeCounryEntities.FirstOrDefault()?.EdiCode;


                        var gidecegiUlkeEntities = _uow.Country.Search(x => x.IsoCode.Trim().ToUpper() == detailDto.GidecegiUlke.Trim().ToUpper() || x.EdiCode == detailDto.GidecegiUlke);
                        if (gidecegiUlkeEntities.Count != 1)
                        {
                            var tempList = new List<string>();
                            if (gidecegiUlkeEntities.Count == 0)
                            {
                                tempList.Add($"GidecegiUlke Bulunamadı. GidecegiUlke:{detailDto.GidecegiUlke}");
                                logs.Add($"GidecegiUlke Bulunamadı. GidecegiUlke:{detailDto.GidecegiUlke}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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

                        var gidecegiUlke = gidecegiUlkeEntities.FirstOrDefault()?.EdiCode;


                        var menseUlkeEntities = _uow.Country.Search(x => x.IsoCode.Trim().ToUpper() == detailDto.MenseUlke.Trim().ToUpper() || x.EdiCode == detailDto.MenseUlke);
                        //eğer MenseUlke tablosunda exceldek MenseUlke ile eşleşen alan yoksa başa dön
                        if (menseUlkeEntities.Count != 1)
                        {
                            var tempList = new List<string>();
                            if (menseUlkeEntities.Count == 0)
                            {
                                tempList.Add($"MenseUlke Bulunamadı. MenseUlke:{detailDto.MenseUlke}");
                                logs.Add($"MenseUlke Bulunamadı. MenseUlke:{detailDto.MenseUlke}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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

                        var menseUlke = gidecegiUlkeEntities.FirstOrDefault()?.EdiCode;

                        //GoodsName excelden gelir veritabanındaki ile karşılaştırır eşit olanları getirir.
                        var productEntities = _uow.Products.Search(x => x.ProductNo.Trim().ToUpper() == detailDto.UrunKod.Trim().ToUpper());
                        //eğer product tablosunda aynı adla 1'den fazla veri var ise başa dön kullanıcıya uyarı ver.
                        if (productEntities.Count > 1)
                        {
                            var tempList = new List<string>();
                            //product nameleri al aralara virgül koy ve string olarak dön
                            var ambiguousValues = string.Join(",", productEntities.Select(x => x.ProductNo));

                            tempList.Add($"TPS No: {stokGirisDto.TpsNo}.{detailDto.UrunKod} adlı \"Ürün\" için birden fazla değer döndü. Kararsız kalınan değerler: {ambiguousValues}");
                            logs.Add($"TPS No: {stokGirisDto.TpsNo}.{detailDto.UrunKod} adlı \"Ürün\" için birden fazla değer döndü. Kararsız kalınan değerler: {ambiguousValues}. {i} satırındaki bilgiler kayıt edilmeyecek.");

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

                        var product = productEntities.FirstOrDefault();
                        //eğer product tablosunda tanım ile eşleşen yok ise tabloya insert atar.
                        if (product == null)
                        {
                            product = _uow.Products.Add(new Product
                            {
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
                            logs.Add($"Product tablosuna ekleme yapıldı.Ekleme yapılan değer :{product.ProductNo}. {i} satırındaki kayıt işlemine devam ediliyor.");
                        }


                        if (stokGirisInsertList.Any(x => x.TpsNo == stokGirisDto.TpsNo && x.BeyannameNo == stokGirisDto.BeyannameNo))//INSERT
                        {
                            logs.Add($"{i} satırı için detay inserti yapılacak.");

                            var endDetayList = stokGirisInsertList.LastOrDefault().ChepStokGirisDetay.LastOrDefault();
                            //kayıt var ise ilk satırı alır
                            //TpsSira no alanı exceldeki önceki alanla eşit ise eklemeyi yapma
                            if (endDetayList.TpsSiraNo == detailDto.TpsSiraNo && endDetayList.EsyaGtip == detailDto.EsyaGtip && endDetayList.FaturaNo == detailDto.FaturaNo
                                             && endDetayList.FaturaTutar == detailDto.FaturaTutar && endDetayList.FaturaDovizKod == detailDto.FaturaDovizKod
                                             && endDetayList.Miktar == detailDto.Miktar
                                             && endDetayList.OlcuBirimi == detailDto.OlcuBirimi && endDetayList.UrunKod == product.ProductNo)
                            {
                                continue;
                            }

                            var existItemMasterGiris = stokGirisInsertList.FirstOrDefault(x => x.TpsNo == stokGirisDto.TpsNo && x.BeyannameNo == stokGirisDto.BeyannameNo);

                            if (existItemMasterGiris != null && existItemMasterGiris.ChepStokGirisDetay != null && existItemMasterGiris.ChepStokGirisDetay.Count > 0
                                && detailDto.FaturaTutar != null && detailDto.FaturaTutar > 0 && detailDto.FaturaDovizKod != null
                                && detailDto.EsyaGtip != null && detailDto.FaturaNo != null
                                && detailDto.Miktar != null && detailDto.Miktar > 0)
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
                                });
                                _uow.Commit();

                                logs.Add($" {i} satırı için existItemMasterGiris'in detay listesine ekleme yapıldı.");

                            }
                        }

                        else if (stokGirisUpdateList.Any(x => x.TpsNo == stokGirisDto.TpsNo && x.BeyannameNo == stokGirisDto.BeyannameNo) && existStokGirisEntities != null)
                        {
                            logs.Add($"{i} satırı için detay update i yapılacak.");

                            var existStokGirisDetayEntities = _uow.ChepStokGirisDetay.Search(x => x.StokGirisId == existStokGirisEntity.StokGirisId);
                            var existStokGirisDetayEntity = existStokGirisDetayEntities.FirstOrDefault();

                            var oldEntityGirisDetayList = _uow.ChepStokGirisDetay.Set()
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId
                                             && x.TpsSiraNo == detailDto.TpsSiraNo && x.EsyaGtip == detailDto.EsyaGtip && x.FaturaNo == detailDto.FaturaNo
                                             && x.FaturaTutar == detailDto.FaturaTutar && x.FaturaDovizKod == detailDto.FaturaDovizKod && x.Miktar == detailDto.Miktar
                                             && x.OlcuBirimi == detailDto.OlcuBirimi && x.UrunKod == product.ProductNo);

                            ChepStokGiris old83InEntity = null;

                            if (oldEntityGirisDetayList == null) // giriş detay bulunamadı!
                            {
                                old83InEntity = _uow.ChepStokGiris.Set()
                                    .Include(x => x.ChepStokGirisDetay)
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId);
                                if (old83InEntity == null)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                old83InEntity = oldEntityGirisDetayList.StokGiris;
                            }

                            if (oldEntityGirisDetayList != null && oldEntityGirisDetayList.StokGirisId > 0)
                            {
                                oldEntityGirisDetayList.TpsBeyan = detailDto.TpsBeyan;
                                oldEntityGirisDetayList.SozlesmeUlke = sozlesmeUlke;
                                oldEntityGirisDetayList.Rejim = detailDto.Rejim;
                                oldEntityGirisDetayList.CikisRejimi = detailDto.CikisRejimi;
                                oldEntityGirisDetayList.PoNo = detailDto.PoNo;
                                oldEntityGirisDetayList.Model = detailDto.Model;
                                oldEntityGirisDetayList.TpsCikisSiraNo = detailDto.TpsCikisSiraNo;
                                oldEntityGirisDetayList.BeyannameKalemNo = detailDto.BeyannameKalemNo;
                                oldEntityGirisDetayList.MenseUlke = menseUlke;
                                oldEntityGirisDetayList.Marka = detailDto.Marka;
                                oldEntityGirisDetayList.GidecegiUlke = gidecegiUlke;
                                oldEntityGirisDetayList.EsyaCinsi = detailDto.EsyaCinsi;
                                oldEntityGirisDetayList.FaturaTarih = detailDto.FaturaTarih;
                                oldEntityGirisDetayList.UrunKod = product.ProductNo;
                                oldEntityGirisDetayList.TpsSiraNo = detailDto.TpsSiraNo;
                                var addition83indetailUpdate = _uow.ChepStokGirisDetay.Update(oldEntityGirisDetayList);
                                _uow.Commit();

                                logs.Add($" {i} satırı için oldEntityGirisDetayList listesine update için ekleme yapıldı.");


                                continue;
                            }

                        }


                        //Excelde aynı fileMasterId var ise detay için devam eder.
                        if (existlistdeneme == false && existStokGirisEntities.Count == 0)
                        {
                            logs.Add($"{i} satırı için veri tabanından gelen bir veri olmadığı için insert listesine kayıt atılacak.");

                            var stokGirisEntity = new ChepStokGiris
                            {
                                GumrukKod = customs?.EdiCode,
                                BasvuruTarihi = stokGirisDto.BasvuruTarihi,
                                BelgeAd = stokGirisDto.BelgeAd,
                                BelgeSart = stokGirisDto.BelgeSart,
                                BeyannameNo = stokGirisDto.BeyannameNo,
                                BeyannameTarihi = stokGirisDto.BeyannameTarihi,
                                IhracatciFirma = ihracatciFirma?.CustomerId,
                                TpsNo = stokGirisDto.TpsNo,
                                KapAdet = stokGirisDto.KapAdet,
                                IthalatciFirma = ithalatcıFirma?.CustomerId,
                                ReferansNo = stokGirisDto.ReferansNo,
                                SureSonuTarihi = stokGirisDto.SureSonuTarihi,
                                TpsAciklama = stokGirisDto.TpsAciklama,
                                TpsDurum = stokGirisDto.TpsDurum,
                            };

                            logs.Add($"{i} satırı için veri tabanından gelen bir veri olmadığı için insert listesine kayıt atıldı. Detaya geçiliyor.");

                            stokGirisEntity.ChepStokGirisDetay = new List<ChepStokGirisDetay>();
                            //birden fazla aynı beyanname no varsa burda ilk detay insertini atar. diğer detay insertlerini 1011. satırdaki ifte atar. 
                            var stokGirisDetay = new ChepStokGirisDetay
                            {
                                UrunKod = product.ProductNo,
                                TpsSiraNo = detailDto.TpsSiraNo,
                                TpsBeyan = detailDto.TpsBeyan,
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

                            logs.Add($"{i} satırı için veri tabanından gelen bir veri olmadığı için insert listesine kayıt atıldı. Detaya içinde kayıt atıldı.");

                        }
                        //sadece exceldeki kırmızı alanları update eder.
                        else if (existStokGirisEntities.Count > 0)
                        {
                            logs.Add($"{i} satırı için veri tabanında veri bulunduğu için update listesine ekleme yapıldacak.");

                            var oldEntityGirisDetay = _uow.ChepStokGirisDetay.Set()
                                             .Include(x => x.StokGiris)
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId
                                             && x.TpsSiraNo == detailDto.TpsSiraNo && x.EsyaGtip == detailDto.EsyaGtip && x.FaturaNo == detailDto.FaturaNo
                                             && x.FaturaTutar == detailDto.FaturaTutar && x.FaturaDovizKod == detailDto.FaturaDovizKod && x.Miktar == detailDto.Miktar
                                             && x.OlcuBirimi == detailDto.OlcuBirimi && x.UrunKod == product.ProductNo);

                            ChepStokGiris oldGirisEntity = null;

                            if (oldEntityGirisDetay == null) // giriş detay bulunamadı!
                            {
                                oldGirisEntity = _uow.ChepStokGiris.Set().AsNoTracking()
                                    .Include(x => x.ChepStokGirisDetay)
                                             .FirstOrDefault(x => x.StokGirisId == existStokGirisEntity.StokGirisId);
                                if (oldGirisEntity == null)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                oldGirisEntity = oldEntityGirisDetay.StokGiris;
                            }

                            if (!stokGirisUpdateList.Any(x => x.StokGirisId == existStokGirisEntity.StokGirisId))
                            {
                                existStokGirisEntity.ReferansNo = stokGirisDto.ReferansNo;
                                existStokGirisEntity.KapAdet = stokGirisDto.KapAdet;
                                existStokGirisEntity.GumrukKod = customs?.EdiCode;
                                existStokGirisEntity.TpsAciklama = stokGirisDto.TpsAciklama;
                                existStokGirisEntity.SureSonuTarihi = stokGirisDto.SureSonuTarihi;
                                existStokGirisEntity.TpsDurum = stokGirisDto.TpsDurum;
                                existStokGirisEntity.IthalatciFirma = ithalatcıFirma?.CustomerId;
                                existStokGirisEntity.TpsNo = stokGirisDto.TpsNo;
                                existStokGirisEntity.IhracatciFirma = ihracatciFirma?.CustomerId;
                                existStokGirisEntity.BeyannameTarihi = stokGirisDto.BeyannameTarihi;
                                existStokGirisEntity.BeyannameNo = stokGirisDto.BeyannameNo;
                                existStokGirisEntity.BelgeSart = stokGirisDto.BelgeSart;
                                existStokGirisEntity.BelgeAd = stokGirisDto.BelgeAd;
                                existStokGirisEntity.BasvuruTarihi = stokGirisDto.BasvuruTarihi;

                                if (oldEntityGirisDetay != null && oldEntityGirisDetay.StokGirisDetayId > 0)
                                {
                                    oldEntityGirisDetay.TpsSiraNo = oldEntityGirisDetay.TpsSiraNo;
                                    oldEntityGirisDetay.EsyaGtip = oldEntityGirisDetay.EsyaGtip;
                                    oldEntityGirisDetay.TpsBeyan = detailDto.TpsBeyan;
                                    //oldEntityInDetail.StokGirisId = oldEntityInDetail.StokGirisId;
                                    oldEntityGirisDetay.SozlesmeUlke = sozlesmeUlke;
                                    oldEntityGirisDetay.Rejim = detailDto.Rejim;
                                    oldEntityGirisDetay.OlcuBirimi = detailDto.OlcuBirimi;
                                    oldEntityGirisDetay.CikisRejimi = detailDto.CikisRejimi;
                                    oldEntityGirisDetay.PoNo = detailDto.PoNo;
                                    oldEntityGirisDetay.Miktar = oldEntityGirisDetay.Miktar;
                                    oldEntityGirisDetay.Model = detailDto.Model;
                                    oldEntityGirisDetay.TpsCikisSiraNo = detailDto.TpsCikisSiraNo;
                                    oldEntityGirisDetay.BeyannameKalemNo = detailDto.BeyannameKalemNo;
                                    oldEntityGirisDetay.MenseUlke = menseUlke;
                                    oldEntityGirisDetay.Marka = detailDto.Marka;
                                    oldEntityGirisDetay.GidecegiUlke = gidecegiUlke;
                                    oldEntityGirisDetay.FaturaTutar = oldEntityGirisDetay.FaturaTutar;
                                    oldEntityGirisDetay.EsyaCinsi = detailDto.EsyaCinsi;
                                    oldEntityGirisDetay.FaturaDovizKod = oldEntityGirisDetay.FaturaDovizKod;
                                    oldEntityGirisDetay.FaturaNo = oldEntityGirisDetay.FaturaNo;
                                    oldEntityGirisDetay.FaturaTarih = detailDto.FaturaTarih;
                                    oldEntityGirisDetay.TpsSiraNo = detailDto.TpsSiraNo;

                                    var stokGirisDetailUpdate = _uow.ChepStokGirisDetay.Update(oldEntityGirisDetay);
                                    _uow.Commit();
                                }
                                stokGirisUpdateList.Add(existStokGirisEntity);
                                logs.Add($"{i} satırı için veri tabanında veri bulunduğu için update listesine ekleme yapıldı..");
                            }


                        }
                        //bir hata var ise string tipinde virgül ile listeler ve yazdırır.
                        else
                        {
                            var tempList = new List<string>();

                            var ambiguousValues = string.Join(",", productEntities.Select(x => x.ProductNo));

                            tempList.Add($"Beyanname No: {stokGirisDto.TpsNo} için birden fazla değer döndü. Kararsız kalınan değerler: {ambiguousValues}");

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
                            logs.Add("Master için insert veya update yapılmadı. Detay için yapıldı.");
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
                    logs.Add("İnsert listesi insert edildi.");
                    if (resultInsert.Count > 0)
                    {
                        summaryList.Insert(0, $"{resultInsert.Count} adet yeni kayıt atıldı.");
                    }
                }

                if (stokGirisUpdateList != null && stokGirisUpdateList.Count > 0)
                {
                    var resultUpdate = _uow.ChepStokGiris.UpdateRange(stokGirisUpdateList);
                    _uow.Commit();
                    logs.Add("Update listesi update edildi.");
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