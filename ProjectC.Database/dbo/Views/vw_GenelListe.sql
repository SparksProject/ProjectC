
Create View vw_GenelListe
as
-- Genel Rapor
select 
	Giris.StokGirisId,Giris.ReferansNo as GirisReferansNo,Giris.TPSNo as GirisTPSNo,TPSDurum,BasvuruTarihi,SureSonuTarihi,GumrukKod,Giris.BeyannameNo as GirisBeyannameNo,
	Giris.BeyannameTarihi as GirisBeyannameTarihi,BelgeAd,BelgeSart,TPSAciklama,IthalatciFirma,Giris.IhracatciFirma as GirisIhracatciFirma,KapAdet,GirisDetay.StokGirisDetayId,
	GirisDetay.TPSSiraNo as GirisTPSSiraNo,TPSBeyan,EsyaCinsi,EsyaGTIP,GirisDetay.FaturaNo as GirisFaturaNo,GirisDetay.FaturaTarih as GirisFaturaTarih,FaturaTutar,FaturaDovizKod,
	GirisDetay.Miktar as GirisMiktar,OlcuBirimi,Rejim,CikisRejimi,GidecegiUlke,MenseUlke,SozlesmeUlke,Marka,Model,UrunKod,PONo,Cikis.StokCikisId,Cikis.ReferansNo as CikisReferansNo,
	Cikis.IslemTarihi as CikisIslemTarihi,Cikis.BeyannameNo as CikisBeyannameNo,Cikis.BeyannameTarihi as CikisBeyannameTarihi,Cikis.IhracatciFirma as CikisIhracatciFirma,
	Cikis.TPSNo as CikisTPSNo,Cikis.TPSTarih as CikisTPSTarih,CikisDetay.Miktar as CikisMiktar 
from ChepStokGiris Giris
	join ChepStokGirisDetay GirisDetay on Giris.StokGirisId=GirisDetay.StokGirisId
	left outer join ChepStokCikisDetay CikisDetay on GirisDetay.StokGirisDetayId=CikisDetay.StokGirisDetayId
	left outer join ChepStokCikis Cikis on Cikis.StokCikisId=CikisDetay.StokCikisId