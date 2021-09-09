using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;


namespace Chep.Service
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpClientFactory _clientFactory;

        public WorkOrderService(IUnitOfWork uow, IHttpClientFactory clientFactory)
        {
            _uow = uow;
            _clientFactory = clientFactory;

        }

        public string WebServiceUrl
        {
            get
            {
                var baseUrl = "http://chepws.us-east-1.elasticbeanstalk.com/";

#if DEBUG
                baseUrl = "http://localhost:65404/";
#endif

                var url = $"{baseUrl}WebService/Post";

                return url;
            }
        }

        public async Task<string> SetWorkOrderMastersModel(int id)
        {
            try
            {
#if DEBUG
                // id = 34;
#endif
                var master = _uow.VwWsWorkOrderMaster.Single(x => x.StokCikisId == id);
                var invoices = _uow.VwWsWorkOrderInvoice.Search(x => x.StokCikisId == id);
                var invoiceDetails = _uow.VwWsWorkOrderInvoiceDetails.Search(x => x.StokCikisId == id);
                var invoiceDetailsTcgb = _uow.VwWsWorkOrderInvoiceDetailsTcgb.Search(x => x.StokCikisId == id);

                if (master == null)
                {
                    return ("Master kaydı null geldi. Bir sorun var!");
                }

                var dto = new WorkOrderMasterModelDTO
                {
                    VwWsWorkOrderMaster = new VwWsWorkOrderMasterDTO()
                    {
                        DeclarationType = master.DeclarationType,
                        UserNameWs = master.UserNameWs,
                        PasswordWs = master.PasswordWs,
                        StokCikisId = master.StokCikisId,
                        WorkOrderMasterId = master.WorkOrderMasterId,
                        WorkOrderNo = master.WorkOrderNo
                    },
                    VwWsWorkOrderInvoices = new List<VwWsWorkOrderInvoiceDTO>(),
                    VwWsWorkOrderInvoiceDetails = new List<VwWsWorkOrderInvoiceDetailsDTO>(),
                    VwWsWorkOrderInvoiceDetailsTcgb = new List<VwWsWorkOrderInvoiceDetailsTcgbDto>()
                };

                foreach (var invoice in invoices)
                {
                    dto.VwWsWorkOrderInvoices.Add(new VwWsWorkOrderInvoiceDTO()
                    {
                        AgentName = invoice.AgentName,
                        AwbNo = invoice.AwbNo,
                        Blno = invoice.Blno,
                        ConsgnAddress = invoice.ConsgnAddress,
                        ConsgnCity = invoice.ConsgnCity,
                        ConsgnCountry = invoice.ConsgnCountry,
                        ConsgnName = invoice.ConsgnName,
                        ConsgnNo = invoice.ConsgnNo,
                        ContainerNo = invoice.ContainerNo,
                        Customs = invoice.Customs,
                        DeliveryLocation = invoice.DeliveryLocation,
                        EntryExitCustoms = invoice.EntryExitCustoms,
                        FreightAmount = invoice.FreightAmount,
                        FreightCurrency = invoice.FreightCurrency,
                        GtbReferenceNo = invoice.GtbReferenceNo,
                        Incoterms = invoice.Incoterms,
                        InsuranceAmount = invoice.InsuranceAmount,
                        InsuranceCurrency = invoice.InsuranceCurrency,
                        InvoiceAmount = invoice.InvoiceAmount,
                        InvoiceCurrency = invoice.InvoiceCurrency,
                        InvoiceId = invoice.InvoiceId,
                        PaymentMethod = invoice.PaymentMethod,
                        PlateNo = invoice.PlateNo,
                        SenderAddress = invoice.SenderAddress,
                        SenderCity = invoice.SenderCity,
                        SenderCountry = invoice.SenderCountry,
                        SenderName = invoice.SenderName,
                        SenderNo = invoice.SenderNo,
                        StokCikisId = invoice.StokCikisId,
                        TransptrName = invoice.TransptrName,
                        VesselName = invoice.VesselName,
                        WorkOrderMasterId = invoice.WorkOrderMasterId,
                        WorkOrderNo = invoice.WorkOrderNo
                    });
                }

                foreach (var detail in invoiceDetails.OrderBy(x => x.ItemNumber))
                {
                    dto.VwWsWorkOrderInvoiceDetails.Add(new VwWsWorkOrderInvoiceDetailsDTO()
                    {
                        ActualQuantity = detail.ActualQuantity,
                        CommclDesc = detail.CommclDesc,
                        CountryOfOrigin = detail.CountryOfOrigin,
                        DescGoods = detail.DescGoods,
                        GrossWeight = detail.GrossWeight,
                        HsCode = detail.HsCode,
                        IncentiveLineNo = detail.IncentiveLineNo,
                        IntrnlAgmt = detail.IntrnlAgmt,
                        InvoiceAmount = detail.InvoiceAmount,
                        InvoiceDate = detail.InvoiceDate,
                        InvoiceDetailId = detail.InvoiceDetailId,
                        InvoiceId = detail.InvoiceId,
                        InvoiceNo = detail.InvoiceNo,
                        InvoiceQuantity = detail.InvoiceQuantity,
                        ItemNumber = detail.ItemNumber,
                        NetWeight = detail.NetWeight,
                        NumberOfPackages = detail.NumberOfPackages,
                        PkgType = detail.PkgType,
                        ProducerCompany = detail.ProducerCompany,
                        ProducerCompanyNo = detail.ProducerCompanyNo,
                        ProductNo = detail.ProductNo,
                        StokCikisId = detail.StokCikisId,
                        Uom = detail.Uom,
                        WorkOrderMasterId = detail.WorkOrderMasterId,
                        WorkOrderNo = detail.WorkOrderNo,
                    });

                }

                foreach (var detailTcgb in invoiceDetailsTcgb)
                {
                    dto.VwWsWorkOrderInvoiceDetailsTcgb.Add(new VwWsWorkOrderInvoiceDetailsTcgbDto()
                    {
                        StokCikisId = detailTcgb.StokCikisId,
                        DeclarationDate = detailTcgb.DeclarationDate,
                        DeclarationNo = detailTcgb.DeclarationNo,
                        Description = detailTcgb.Description,
                        InvoiceDetailId = detailTcgb.InvoiceDetailId,
                        InvoiceDetailsTcgbId = detailTcgb.InvoiceDetailsTcgbId,
                        ItemNo = detailTcgb.ItemNo,
                        Quantity = detailTcgb.Quantity
                    });
                }

                var client = _clientFactory.CreateClient();
                var content = JsonConvert.SerializeObject(dto);
                var ws = await client.PostAsync(WebServiceUrl, new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

                ws.EnsureSuccessStatusCode();

                var deger = (await ws.Content.ReadAsStringAsync()).ToString();

                return deger;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}