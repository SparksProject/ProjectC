using Chep.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using LocalWs;
using System.Collections.Generic;

namespace Chep.WebService
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WebServiceController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] WorkOrderMasterModelDTO dto)
        {
            try
            {
                Service1Client client = new Service1Client();
                WorkOrderMasterModel wo = new WorkOrderMasterModel();

                wo.DeclarationType = dto.VwWsWorkOrderMaster.DeclarationType;
                wo.WorkOrderNo = dto.VwWsWorkOrderMaster.WorkOrderNo.ToString();
                wo.WorkOrderMasterId = dto.VwWsWorkOrderMaster.WorkOrderMasterId.Value;

                InvoiceModel inv = new InvoiceModel();
                wo.InvoiceList = new List<InvoiceModel>();

                ResultModel resultModel = new ResultModel();

                foreach (var woInv in dto.VwWsWorkOrderInvoices)
                {
                    inv.AgentName = woInv.AgentName;
                    inv.AwbNo = woInv.AwbNo;
                    inv.BLNo = woInv.Blno;
                    inv.ConsgnAddress = woInv.ConsgnAddress;
                    inv.ConsgnCity = woInv.ConsgnCity;
                    inv.ConsgnCountry = woInv.ConsgnCountry;
                    inv.ConsgnName = woInv.ConsgnName;
                    inv.ConsgnNo = woInv.ConsgnNo;
                    inv.ContainerNo = woInv.ContainerNo;
                    inv.Customs = woInv.Customs;
                    inv.DeliveryLocation = woInv.DeliveryLocation;
                    inv.EntryExitCustoms = woInv.EntryExitCustoms;
                    inv.FreightAmount = woInv.FreightAmount;
                    inv.FreightCurrency = woInv.FreightCurrency;
                    inv.GtbReferenceNo = woInv.GtbReferenceNo;
                    inv.Incoterms = woInv.Incoterms;
                    inv.InsuranceAmount = woInv.InsuranceAmount;
                    inv.InsuranceCurrency = woInv.InsuranceCurrency;
                    if (woInv.InvoiceAmount.HasValue)
                        inv.InvoiceAmount = woInv.InvoiceAmount.Value;
                    inv.InvoiceCurrency = woInv.InvoiceCurrency;
                    inv.InvoiceId = woInv.InvoiceId.Value;
                    inv.PaymentMethod = woInv.PaymentMethod;
                    inv.PlateNo = woInv.PlateNo;
                    inv.SenderAddress = woInv.SenderAddress;
                    inv.SenderCity = woInv.SenderCity;
                    inv.SenderCountry = woInv.SenderCountry;
                    inv.SenderName = woInv.SenderName;
                    inv.SenderNo = woInv.SenderNo;
                    inv.TransptrName = woInv.TransptrName;
                    inv.VesselName = woInv.VesselName;
                    if (woInv.WorkOrderMasterId.HasValue)
                        inv.WorkOrderMasterId = woInv.WorkOrderMasterId.Value;

                    inv.InvoiceDetailList = new List<InvoiceDetailModel>();
                    InvoiceDetailModel detail = new InvoiceDetailModel();

                    foreach (var invDetail in dto.VwWsWorkOrderInvoiceDetails)
                    {
                        if (invDetail.ActualQuantity.HasValue)
                            detail.ActualQuantity = invDetail.ActualQuantity.Value;

                        detail.CommclDesc = invDetail.CommclDesc;
                        detail.CountryOfOrigin = invDetail.CountryOfOrigin;
                        detail.DescGoods = invDetail.DescGoods;
                        detail.GrossWeight = Convert.ToDouble(invDetail.GrossWeight);
                        detail.HsCode = invDetail.HsCode;
                        detail.IncentiveLineNo = invDetail.IncentiveLineNo;
                        detail.IntrnlAgmt = invDetail.IntrnlAgmt;
                        detail.InvoiceAmount = Convert.ToDouble(invDetail.InvoiceAmount);
                        if (invDetail.InvoiceDate.HasValue)
                            detail.InvoiceDate = invDetail.InvoiceDate.Value;
                        if (invDetail.InvoiceDetailId.HasValue)
                            detail.InvoiceDetailId = invDetail.InvoiceDetailId.Value;
                        if (invDetail.InvoiceId.HasValue)
                            detail.InvoiceId = invDetail.InvoiceId.Value;
                        detail.InvoiceNo = invDetail.InvoiceNo;
                        if (invDetail.InvoiceQuantity.HasValue)
                            detail.InvoiceQuantity = invDetail.InvoiceQuantity.Value;
                        if (invDetail.ItemNumber.HasValue)
                            detail.ItemNumber = Convert.ToInt32(invDetail.ItemNumber.Value);
                        detail.NetWeight = Convert.ToDouble(invDetail.NetWeight);
                        if (invDetail.NumberOfPackages.HasValue)
                            detail.NumberOfPackages = invDetail.NumberOfPackages.Value;
                        detail.PkgType = invDetail.PkgType;
                        detail.ProducerCompany = invDetail.ProducerCompany;
                        detail.ProducerCompanyNo = invDetail.ProducerCompanyNo;
                        detail.ProductNo = invDetail.ProductNo;
                        detail.Uom = invDetail.Uom;

                        inv.InvoiceDetailList.Add(detail);
                    }

                    wo.InvoiceList.Add(inv);
                }

                var wcf = client.SetWorkOrderMastersModelAsync(dto.VwWsWorkOrderMaster.UserNameWs, dto.VwWsWorkOrderMaster.PasswordWs, wo);
                wcf.Wait();
                resultModel = wcf.Result;

                return Ok(resultModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}