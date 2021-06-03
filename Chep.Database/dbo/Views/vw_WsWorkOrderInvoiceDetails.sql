
CREATE view [dbo].[vw_WsWorkOrderInvoiceDetails]
as
select 
  cikis.StokCikisId,cikis.InvoiceId,cikis.WorkOrderMasterId,cikis.ReferansNo as WorkOrderNo
  ,cdetay.InvoiceDetailId as InvoiceDetailId,gdetay.EsyaGtip as HsCode,gdetay.EsyaCinsi as DescGoods
  ,gdetay.UrunKod as ProductNo,gdetay.MenseUlke as CountryOfOrigin,gdetay.OlcuBirimi as Uom,cdetay.Miktar as ActualQuantity,
  cdetay.Miktar as InvoiceQuantity,isnull(cdetay.BrutKg,0) as GrossWeight ,isnull(cdetay.NetKg,0) as NetWeight,'' as IntrnlAgmt,
  cikis.InvoiceNo as InvoiceNo,cikis.InvoiceDate as InvoiceDate,isnull(gdetay.FaturaTutar,0) as InvoiceAmount,'' as PkgType,'' as CommclDesc,
  '' as NumberOfPackages, ROW_NUMBER() OVER (ORDER BY  cikis.StokCikisId) as ItemNumber,'' as ProducerCompanyNo,'' as ProducerCompany,
  '' as IncentiveLineNo
from ChepStokCikis cikis
JOIN ChepStokCikisDetay cdetay on cikis.StokCikisId=cdetay.StokCikisId
JOIN ChepStokGirisDetay gdetay on cdetay.StokGirisDetayId=gdetay.StokGirisDetayId