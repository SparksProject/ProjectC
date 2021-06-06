create view [dbo].[vw_WsWorkOrderInvoice]
as
select 
  cikis.StokCikisId,cikis.InvoiceId,cikis.WorkOrderMasterId,cikis.ReferansNo as WorkOrderNo,
  '' as SenderNo,'' as SenderName,'' as SenderAddress,'' as SenderCity,'' as SenderCountry,'' as ConsgnNo
  ,'' as ConsgnName,'' as ConsgnAddress,'' as ConsgnCity,'' as ConsgnCountry,'' as TransptrName,'' as VesselName
  ,'' as AgentName,'' as PlateNo,'' as AwbNo,'' as BLNo,cikis.TeslimSekli as Incoterms,'' as DeliveryLocation
  ,cikis.InvoiceAmount as InvoiceAmount,cikis.InvoiceCurrency as InvoiceCurrency,0 as FreightAmount,'' as FreightCurrency
  ,0 as InsuranceAmount,'' as InsuranceCurrency,'' as ContainerNo,'' as GtbReferenceNo,cikis.OdemeSekli as PaymentMethod
  ,cikis.CikisGumruk as EntryExitCustoms,'' as Customs
from ChepStokCikis cikis