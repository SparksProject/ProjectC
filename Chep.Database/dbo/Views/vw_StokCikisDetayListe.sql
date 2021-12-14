CREATE View [dbo].[vw_StokCikisDetayListe]
as
-- Stok Çıkış Detay Raporu
select 
	Cikis. StokCikisId, Cikis.ReferansNo, IslemTarihi, GirisDetay.BeyannameNo, GirisDetay.BeyannameTarihi, IhrFirma.Name as IhracatciFirma, Giris.TPSNo, Giris.BasvuruTarihi as TPSTarih, 
	StokCikisDetayId,EsyaCinsi, EsyaGtip, Marka, Model, UrunKod, PONo, GirisDetay.Miktar as GirisMiktar,
	Detay.Miktar as CikisMiktar,detay.StokGirisDetayId
from ChepStokCikis Cikis
	join ChepStokCikisDetay Detay on Cikis.StokCikisId=Detay.StokCikisId
	join ChepStokGirisDetay GirisDetay on Detay.StokGirisDetayId=GirisDetay.StokGirisDetayId
	JOIN ChepStokGiris Giris on  GirisDetay.StokGirisId=Giris.StokGirisId
	LEFT OUTER JOIN Customer IhrFirma on Cikis.IhracatciFirma=IhrFirma.CustomerId