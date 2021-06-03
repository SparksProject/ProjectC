create View vw_StokCikisFordListe
as
select 
	giris.BelgeAd as 'İTHAL İŞLEM TÜRÜ',('       '+girisdetay.UrunKod) as 'PARÇA NO',cikisdetay.Miktar as 'PARÇA ADEDİ',girisdetay.MenseUlke as 'MENŞEİ',girisdetay.EsyaGtip as 'GTİP',girisdetay.BeyannameNo as 'İTHALAT BEYANNAME NO',
	girisdetay.BeyannameTarihi as 'İTHALAT BEYANNAME TARİHİ',(giris.TpsNo +'/'+ cast(girisdetay.TpsCikisSiraNo as nvarchar(3))) as 'İHRACAT TPS NO',giris.BasvuruTarihi as 'İHRACAT TPS TARİHİ',
	cikis.BeyannameNo as 'İHRACAT BEYANNAME NUMARASI',cikis.BeyannameTarihi as 'İHRACAT BEYANNAME TARİHİ',cikis.ReferansNo as 'Çıkış ReferansNo',1 as UserId

from ChepStokCikis cikis
 JOIN ChepStokCikisDetay cikisdetay on cikis.StokCikisId=cikisdetay.StokCikisId
 LEFT OUTER JOIN ChepStokGirisDetay girisdetay on cikisdetay.StokGirisDetayId=girisdetay.StokGirisDetayId
 JOIN ChepStokGiris giris on girisdetay.StokGirisId=giris.StokGirisId