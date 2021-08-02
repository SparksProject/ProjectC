


CREATE View [dbo].[vw_StokDusumListe]
as
select  
	StokGirisDetayId,Giris.StokGirisId,Giris.TPSNo,ReferansNo as GirisReferansNo,SureSonuTarihi,detay.BeyannameNo as GirisBeyannameNo,detay.BeyannameTarihi as GirisBeyannameTarihi, 
	Marka, Model, UrunKod ,PONo,detay.Miktar as GirisMiktar,
	isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as CikisMiktar,
    isnull(Miktar,0) - isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) as KalanMiktar,
    Detay.TpsCikisSiraNo,(select isnull(p.UnitPrice,0) from Product p where detay.UrunKod=p.ProductNo and p.CustomerId=Giris.IthalatciFirma) as BirimFiyat,
	(select isnull(p.NetWeight,0) from Product p where detay.UrunKod=p.ProductNo and p.CustomerId=Giris.IthalatciFirma) as NetKg,
	(select isnull(p.GrossWeight,0) from Product p where detay.UrunKod=p.ProductNo and p.CustomerId=Giris.IthalatciFirma) as BrutKg,
	giris.IthalatciFirma
from ChepStokGiris Giris
 join ChepStokGirisDetay Detay on Giris.StokGirisId=Detay.StokGirisId
where   (Miktar - isnull((select sum(CikisDetay.Miktar) from ChepStokCikisDetay CikisDetay where Detay.StokGirisDetayId=CikisDetay.StokGirisDetayId),0) )>0
  and DATEDIFF(day,GETDATE(),SureSonuTarihi)>0