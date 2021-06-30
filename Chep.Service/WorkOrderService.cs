using System;
using System.Collections.Generic;
using System.Net.Http;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;
using Newtonsoft.Json;

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

        string userName = "sparks";
        string passowrd = "1234";
        string webServiceUrl = "https://localhost:44398/WebService/Post";


        public int SetWorkOrderMastersModel(int id)
        {

            var master = _uow.VwWsWorkOrderMaster.Single(x => x.StokCikisId == id);
            var invoices = _uow.VwWsWorkOrderInvoice.Search(x => x.StokCikisId == id);
            var invoiceDetails = _uow.VwWsWorkOrderInvoiceDetails.Search(x => x.StokCikisId == id);


            WorkOrderMasterModelDTO dto = new WorkOrderMasterModelDTO();

            dto.VwWsWorkOrderMaster = new VwWsWorkOrderMasterDTO()
            {
                DeclarationType = master.DeclarationType,
                PasswordWs = passowrd,
                StokCikisId = master.StokCikisId,
                UserNameWs = userName,
                WorkOrderMasterId = master.WorkOrderMasterId,
                WorkOrderNo = master.WorkOrderNo
            };

            dto.VwWsWorkOrderInvoices = new List<VwWsWorkOrderInvoiceDTO>();

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

            dto.VwWsWorkOrderInvoiceDetails = new List<VwWsWorkOrderInvoiceDetailsDTO>();

            foreach (var detail in invoiceDetails)
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
                    WorkOrderNo = detail.WorkOrderNo
                });
            }

            var client = _clientFactory.CreateClient();
            var content = JsonConvert.SerializeObject(dto);
            var ws = client.PostAsync(webServiceUrl, new StringContent(content, System.Text.Encoding.UTF8, "application/json"));
            ws.Wait();
            var result = ws.Result;

            return 0;
        }

    }
}