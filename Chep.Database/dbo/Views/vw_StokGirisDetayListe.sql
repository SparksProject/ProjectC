


CREATE View [dbo].[vw_StokGirisDetayListe]
as
-- Stok Giriş Detay Raporu
select 
	Giris.StokGirisId,ReferansNo,TPSNo,TPSDurum,BasvuruTarihi,SureSonuTarihi,GumrukKod,Detay.BeyannameNo,Detay.BeyannameTarihi,BelgeAd,BelgeSart,TPSAciklama,IthalatciFirma,
	IhracatciFirma,KapAdet,StokGirisDetayId,TPSSiraNo,TPSBeyan,EsyaCinsi,EsyaGtip,FaturaNo,FaturaTarih,FaturaTutar,FaturaDovizKod,Miktar,OlcuBirimi,Rejim,
	CikisRejimi,GidecegiUlke,MenseUlke,SozlesmeUlke,Marka,Model,UrunKod,PONo,1 as UserId 
from ChepStokGiris Giris
join ChepStokGirisDetay Detay on Giris.StokGirisId=Detay.StokGirisId