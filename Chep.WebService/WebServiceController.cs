using Chep.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using Pfr360Ws;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chep.WebService
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WebServiceController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WorkOrderMasterModelDTO obj)
        {
            try
            {
                if (!obj.VwWsWorkOrderMaster.WorkOrderMasterId.HasValue)
                {
                    return BadRequest("WorkOrderMasterId eksik!");
                }

                var objMaster = new WorkOrderMasterModel
                {
                    DeclarationType = obj.VwWsWorkOrderMaster.DeclarationType,
                    WorkOrderNo = $"{obj.VwWsWorkOrderMaster.WorkOrderNo}",
                    WorkOrderMasterId = obj.VwWsWorkOrderMaster.WorkOrderMasterId.Value,
                    InvoiceList = new List<InvoiceModel>(),
                };

                foreach (var woInv in obj.VwWsWorkOrderInvoices)
                {
                    var objInvoice = new InvoiceModel
                    {
                        AgentName = woInv.AgentName,
                        AwbNo = woInv.AwbNo,
                        BLNo = woInv.Blno,
                        ConsgnAddress = woInv.ConsgnAddress,
                        ConsgnCity = woInv.ConsgnCity,
                        ConsgnCountry = woInv.ConsgnCountry,
                        ConsgnName = woInv.ConsgnName,
                        ConsgnNo = woInv.ConsgnNo,
                        ContainerNo = woInv.ContainerNo,
                        Customs = woInv.Customs,
                        DeliveryLocation = woInv.DeliveryLocation,
                        EntryExitCustoms = woInv.EntryExitCustoms,
                        FreightAmount = woInv.FreightAmount,
                        FreightCurrency = woInv.FreightCurrency,
                        GtbReferenceNo = woInv.GtbReferenceNo,
                        Incoterms = woInv.Incoterms,
                        InsuranceAmount = woInv.InsuranceAmount,
                        InsuranceCurrency = woInv.InsuranceCurrency,
                        InvoiceCurrency = woInv.InvoiceCurrency,
                        InvoiceId = woInv.InvoiceId.Value,
                        PaymentMethod = woInv.PaymentMethod,
                        PlateNo = woInv.PlateNo,
                        SenderAddress = woInv.SenderAddress,
                        SenderCity = woInv.SenderCity,
                        SenderCountry = woInv.SenderCountry,
                        SenderName = woInv.SenderName,
                        SenderNo = woInv.SenderNo,
                        TransptrName = woInv.TransptrName,
                        VesselName = woInv.VesselName,
                        InvoiceDetailList = new List<InvoiceDetailModel>(),
                    };

                    if (woInv.InvoiceAmount.HasValue)
                    {
                        objInvoice.InvoiceAmount = woInv.InvoiceAmount.Value;
                    }
                    if (woInv.WorkOrderMasterId.HasValue)
                    {
                        objInvoice.WorkOrderMasterId = woInv.WorkOrderMasterId.Value;
                    }

                    foreach (var itemInvoiceDetail in obj.VwWsWorkOrderInvoiceDetails)
                    {
                        var objInvoiceDetail = new InvoiceDetailModel
                        {
                            Uom = itemInvoiceDetail.Uom,
                            HsCode = itemInvoiceDetail.HsCode,
                            PkgType = itemInvoiceDetail.PkgType,
                            ProductNo = itemInvoiceDetail.ProductNo,
                            InvoiceNo = itemInvoiceDetail.InvoiceNo,
                            DescGoods = itemInvoiceDetail.DescGoods,
                            CommclDesc = itemInvoiceDetail.CommclDesc,
                            IntrnlAgmt = itemInvoiceDetail.IntrnlAgmt,
                            CountryOfOrigin = itemInvoiceDetail.CountryOfOrigin,
                            IncentiveLineNo = itemInvoiceDetail.IncentiveLineNo,
                            ProducerCompany = itemInvoiceDetail.ProducerCompany,
                            ProducerCompanyNo = itemInvoiceDetail.ProducerCompanyNo,
                            NetWeight = Convert.ToDouble(itemInvoiceDetail.NetWeight),
                            GrossWeight = Convert.ToDouble(itemInvoiceDetail.GrossWeight),
                            InvoiceAmount = Convert.ToDouble(itemInvoiceDetail.InvoiceAmount),
                        };

                        if (itemInvoiceDetail.ActualQuantity.HasValue)
                        {
                            objInvoiceDetail.ActualQuantity = itemInvoiceDetail.ActualQuantity.Value;
                        }
                        if (itemInvoiceDetail.InvoiceDate.HasValue)
                        {
                            objInvoiceDetail.InvoiceDate = itemInvoiceDetail.InvoiceDate.Value;
                        }
                        if (itemInvoiceDetail.InvoiceDetailId.HasValue)
                        {
                            objInvoiceDetail.InvoiceDetailId = itemInvoiceDetail.InvoiceDetailId.Value;
                        }
                        if (itemInvoiceDetail.InvoiceId.HasValue)
                        {
                            objInvoiceDetail.InvoiceId = itemInvoiceDetail.InvoiceId.Value;
                        }
                        if (itemInvoiceDetail.InvoiceQuantity.HasValue)
                        {
                            objInvoiceDetail.InvoiceQuantity = itemInvoiceDetail.InvoiceQuantity.Value;
                        }
                        if (itemInvoiceDetail.ItemNumber.HasValue)
                        {
                            objInvoiceDetail.ItemNumber = Convert.ToInt32(itemInvoiceDetail.ItemNumber.Value);
                        }
                        if (itemInvoiceDetail.NumberOfPackages.HasValue)
                        {
                            objInvoiceDetail.NumberOfPackages = itemInvoiceDetail.NumberOfPackages.Value;
                        }

                        objInvoice.InvoiceDetailList.Add(objInvoiceDetail);
                    }

                    objMaster.InvoiceList.Add(objInvoice);
                }

#if DEBUG
                //var xsSubmit = new XmlSerializer(typeof(WorkOrderMasterModel));
                //var xml = string.Empty;

                //using (var sww = new StringWriter())
                //using (var writer = XmlWriter.Create(sww))
                //{
                //    xsSubmit.Serialize(writer, objMaster);
                //    xml = sww.ToString();
                //} 
#endif

                var client = new Service1Client();

                var result = await client.SetWorkOrderMastersModelAsync(obj.VwWsWorkOrderMaster.UserNameWs, obj.VwWsWorkOrderMaster.PasswordWs, objMaster);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}