CREATE view [dbo].[vw_WsWorkOrderInvoiceDetails]
as
select 
  cikis.StokCikisId,cikis.InvoiceId,cikis.InvoiceId as WorkOrderMasterId,cikis.ReferansNo as WorkOrderNo,cdetay.InvoiceDetailId as InvoiceDetailId,
  Urun.HsCode as HsCode,(Urun.ProductNameTr +'-'+urun.ProductNo) as DescGoods
  ,gdetay.UrunKod as ProductNo,Urun.CountryOfOrigin as CountryOfOrigin,Urun.Uom as Uom,cdetay.Miktar as ActualQuantity,
  cdetay.Miktar as InvoiceQuantity,isnull(cdetay.BrutKg,0) as GrossWeight ,isnull(cdetay.NetKg,0) as NetWeight,'' as IntrnlAgmt,
  cikis.InvoiceNo as InvoiceNo,cikis.InvoiceDate as InvoiceDate,isnull(cikis.InvoiceAmount,0) as InvoiceAmount,isnull(cikis.KapCinsi,'BI')  as PkgType,Urun.ProductNameTr as CommclDesc,
  isnull(cikis.KapMiktari,1) as NumberOfPackages, ROW_NUMBER() OVER (PARTITION BY  cikis.StokCikisId Order by cikis.StokCikisId) as ItemNumber,'' as ProducerCompanyNo,'' as ProducerCompany,
  '' as IncentiveLineNo
from ChepStokCikis cikis
JOIN ChepStokCikisDetay cdetay on cikis.StokCikisId=cdetay.StokCikisId
JOIN ChepStokGirisDetay gdetay on cdetay.StokGirisDetayId=gdetay.StokGirisDetayId
JOIN ChepStokGiris Giris on gdetay.StokGirisId=Giris.StokGirisId
JOIN Product Urun on gdetay.UrunKod=Urun.ProductNo and Giris.IthalatciFirma=Urun.CustomerId

--where --Cikis.ReferansNo='202100002'
-- cdetay.StokCikisId=40