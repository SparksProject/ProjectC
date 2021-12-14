

CREATE view [dbo].[vw_WsWorkOrderInvoiceDetailsTcgb]
as
select 
  cikis.StokCikisId,cdetay.InvoiceDetailId  as InvoiceDetailsTcgbId,cdetay.InvoiceDetailId as InvoiceDetailId,
  gdetay.BeyannameNo as DeclarationNo,gdetay.BeyannameKalemNo as ItemNo,cdetay.Miktar as Quantity,'' as  Description,
  gdetay.BeyannameTarihi as DeclarationDate
from ChepStokCikis cikis
JOIN ChepStokCikisDetay cdetay on cikis.StokCikisId=cdetay.StokCikisId
JOIN ChepStokGirisDetay gdetay on cdetay.StokGirisDetayId=gdetay.StokGirisDetayId



--where Cikis.ReferansNo='202100002'
-- cdetay.StokCikisId=40