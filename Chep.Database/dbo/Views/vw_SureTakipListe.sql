
CREATE View [dbo].[vw_SureTakipListe]
as
-- Süre Takip Raporu --Otomail
select 
DATEDIFF(day,SureSonuTarihi,GETDATE()) AS KalanSure,Giris.StokGirisId,ReferansNo,TPSNo,TPSDurum,BasvuruTarihi,SureSonuTarihi,GumrukKod,BeyannameNo,
BeyannameTarihi,BelgeAd,BelgeSart,TPSAciklama,IthalatciFirma,IhracatciFirma,KapAdet,StokGirisDetayId,TPSSiraNo,TPSBeyan,EsyaCinsi,detay.EsyaGtip,FaturaNo,
FaturaTarih,FaturaTutar,FaturaDovizKod,Miktar,OlcuBirimi,Rejim,CikisRejimi,GidecegiUlke,MenseUlke,SozlesmeUlke,Marka,Model,UrunKod,PONo,
isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as CikisMiktar,
isnull(Miktar,0) - isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as KalanMiktar 
from ChepStokGiris Giris
 JOIN ChepStokGirisDetay Detay on Giris.StokGirisId=Detay.StokGirisId
where  DATEDIFF(day,SureSonuTarihi,GETDATE()) between 0 and 15
and (isnull(Miktar,0)-isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) )>0