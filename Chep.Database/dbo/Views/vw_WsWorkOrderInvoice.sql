

CREATE view [dbo].[vw_WsWorkOrderInvoice]
as
select 
  cikis.StokCikisId,cikis.InvoiceId,cikis.InvoiceId as WorkOrderMasterId,cikis.ReferansNo as WorkOrderNo,
  gon.TaxNo as SenderNo,gon.Name as SenderName,gon.Adress as SenderAddress,gon.City as SenderCity,gon.Country as SenderCountry,ali.TaxNo as ConsgnNo
  ,ali.Name as ConsgnName,ali.Adress as ConsgnAddress,ali.City as ConsgnCity,ali.Country as ConsgnCountry,(isnull(Nak.TaxNo,'') + ' '+ Nak.Name) as TransptrName,'' as VesselName
  ,'' as AgentName,cikis.CikisAracKimligi as PlateNo,'' as AwbNo,'' as BLNo,cikis.TeslimSekli as Incoterms,'' as DeliveryLocation  ,cikis.InvoiceAmount as InvoiceAmount,
  cikis.InvoiceCurrency as InvoiceCurrency,0 as FreightAmount,'' as FreightCurrency,0 as InsuranceAmount,'' as InsuranceCurrency,'' as ContainerNo,
  cikis.GtbReferenceNo as GtbReferenceNo,cikis.OdemeSekli as PaymentMethod,cikis.CikisGumruk as EntryExitCustoms,cikis.CikisGumruk as Customs
from ChepStokCikis cikis
JOIN Customer Gon on cikis.IhracatciFirma=gon.CustomerId
JOIN Customer Ali on cikis.AliciFirma=ali.CustomerId
LEFT OUTER JOIN Customer Nak on cikis.NakliyeciFirma=Nak.CustomerId