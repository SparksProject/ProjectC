CREATE View [dbo].[vw_StokCikisFaturaOrnekListe]
as
select 	
	cikis.ReferansNo as 'Çıkış ReferansNo',cikisdetay.SiraNo as KalemNo,girisdetay.UrunKod as 'Ürün Kodu', p.HsCode as Gtip,
	
	(girisdetay.UrunKod +'-'+ girisdetay.EsyaCinsi) as 'Ticari Tanım',cikisdetay.Miktar as Miktar,cikisdetay.BirimTutar as 'Birim Fiyat',
	cikisdetay.InvoiceAmount as 'Fatura Tutarı',cikisdetay.NetKg as NetKg,cikisdetay.BrutKg as 'BrütKg',girisdetay.MenseUlke as 'Menşei',cikis.CikisGumruk +'-'+ Gum.CustomsName as 'Gümrük',
	cikis.TeslimSekli as 'Teslim Şekli',girisdetay.BeyannameNo as 'İthalat Beyanname No',girisdetay.BeyannameTarihi as 'İthalat Beyanname Tarihi',girisdetay.BeyannameKalemNo as 'İthalat Beyanname Kalem No',
	(giris.TpsNo +'/'+ cast(girisdetay.TpsCikisSiraNo as nvarchar(3))) as 'İhracat TPS No',giris.BasvuruTarihi as 'İhracat TPS Tarihi',
	alif.Name as 'Alıcı Firma', alif.Adress as 'Alici Adres',githf.Name as 'Giriş İthalatcı Firma'
	
	
	
from ChepStokCikis cikis
 JOIN ChepStokCikisDetay cikisdetay on cikis.StokCikisId=cikisdetay.StokCikisId
 LEFT OUTER JOIN ChepStokGirisDetay girisdetay on cikisdetay.StokGirisDetayId=girisdetay.StokGirisDetayId
 JOIN ChepStokGiris giris on girisdetay.StokGirisId=giris.StokGirisId
 LEFT OUTER JOIN Customs Gum on cikis.CikisGumruk=Gum.EdiCode 
 LEFT OUTER JOIN Customer Alif on cikis.AliciFirma=alif.CustomerId 
 JOIN Product p on girisdetay.UrunKod=p.ProductNo and giris.IthalatciFirma=p.CustomerId
 LEFT OUTER JOIN Customer githf on giris.IthalatciFirma=githf.CustomerId