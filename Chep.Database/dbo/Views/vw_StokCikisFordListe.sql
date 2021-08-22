
CREATE View [dbo].[vw_StokCikisFordListe]
as
select 
	substring(giris.BelgeAd,1,2) as 'İTHAL İŞLEM TÜRÜ',('       '+rtrim((ltrim(girisdetay.UrunKod)))) as 'PARÇA NO',cikisdetay.Miktar as 'PARÇA ADEDİ',
	girisdetay.MenseUlke as 'MENŞEİ',girisdetay.EsyaGtip as 'GTİP',	girisdetay.BeyannameNo as 'İTHALAT BEYANNAME NO',
	format(girisdetay.BeyannameTarihi  ,'dd.MM.yyyy' )as 'İTHALAT BEYANNAME TARİHİ',
	(giris.TpsNo +'/'+ cast(girisdetay.TpsCikisSiraNo as nvarchar(3))) as 'İHRACAT TPS NO',format(giris.BasvuruTarihi ,'dd.MM.yyyy' ) as 'İHRACAT TPS TARİHİ',
	cikis.BeyannameNo as 'İHRACAT BEYANNAME NUMARASI',format(cikis.BeyannameTarihi,'dd.MM.yyyy' ) as 'İHRACAT BEYANNAME TARİHİ',
	cikis.ReferansNo as CikisReferansNo

from ChepStokCikis cikis
 JOIN ChepStokCikisDetay cikisdetay on cikis.StokCikisId=cikisdetay.StokCikisId
 LEFT OUTER JOIN ChepStokGirisDetay girisdetay on cikisdetay.StokGirisDetayId=girisdetay.StokGirisDetayId
 JOIN ChepStokGiris giris on girisdetay.StokGirisId=giris.StokGirisId