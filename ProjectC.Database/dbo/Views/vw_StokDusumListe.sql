Create View [vw_StokDusumListe]
as
select  
	StokGirisDetayId,Giris.StokGirisId,ReferansNo as GirisReferansNo,SureSonuTarihi,BeyannameNo as GirisBeyannameNo,BeyannameTarihi as GirisBeyannameTarihi, 
	Marka, Model, UrunKod ,PONo,
	isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as CikisMiktar,
    isnull(Miktar,0) - isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as KalanMiktar
from ChepStokGiris Giris
 join ChepStokGirisDetay Detay on Giris.StokGirisId=Detay.StokGirisId
where   (Miktar - isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) )>0
  and DATEDIFF(day,SureSonuTarihi,GETDATE())>0