Create View vw_StokGirisDetayListe
as
-- Stok Giriş Detay Raporu
select 
	Giris.StokGirisId,ReferansNo,TPSNo,TPSDurum,BasvuruTarihi,SureSonuTarihi,GumrukKod,BeyannameNo,BeyannameTarihi,BelgeAd,BelgeSart,TPSAciklama,IthalatciFirma,
	IhracatciFirma,KapAdet,StokGirisDetayId,TPSSiraNo,TPSBeyan,EsyaCinsi,EsyaGTIP,FaturaNo,FaturaTarih,FaturaTutar,FaturaDovizKod,Miktar,OlcuBirimi,Rejim,
	CikisRejimi,GidecegiUlke,MenseUlke,SozlesmeUlke,Marka,Model,UrunKod,PONo 
from ChepStokGiris Giris
join ChepStokGirisDetay Detay on Giris.StokGirisId=Detay.StokGirisId