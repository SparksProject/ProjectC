CREATE View [dbo].[vw_StokDurumListe]
as
-- Stok Durum Raporu
select 
	Giris.StokGirisId,ReferansNo,TPSNo,TPSDurum,BasvuruTarihi,SureSonuTarihi,GumrukKod,Detay.BeyannameNo,Detay.BeyannameTarihi,BelgeAd,BelgeSart,TPSAciklama,ithf.Name as IthalatciFirma,
	ihrf.Name  as IhracatciFirma,KapAdet,StokGirisDetayId,TPSSiraNo,TPSBeyan,EsyaCinsi,EsyaGtip,FaturaNo,FaturaTarih,FaturaTutar,FaturaDovizKod,Miktar,OlcuBirimi,Rejim,
	CikisRejimi,GidecegiUlke,MenseUlke,SozlesmeUlke,Marka,Model,UrunKod,PONo,
    isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as CikisMiktar,
    isnull(Miktar,0)-isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as KalanMiktar 

from ChepStokGiris Giris
 JOIN ChepStokGirisDetay Detay on Giris.StokGirisId=Detay.StokGirisId
 LEFT OUTER JOIN Customer ithf on giris.IthalatciFirma=ithf.CustomerId
 LEFT OUTER JOIN Customer ihrf on giris.IhracatciFirma=ihrf.CustomerId