


CREATE View [dbo].[vw_StokCikisDetayListe]
as
-- Stok Çıkış Detay Raporu
select 
	Cikis. StokCikisId, ReferansNo, IslemTarihi, BeyannameNo, BeyannameTarihi, IhracatciFirma, TPSNo, TPSTarih, 
	StokCikisDetayId,EsyaCinsi, EsyaGtip, Marka, Model, UrunKod, PONo, GirisDetay.Miktar as GirisMiktar,
	Detay.Miktar as CikisMiktar,detay.StokGirisDetayId,1 as UserId
from ChepStokCikis Cikis
	join ChepStokCikisDetay Detay on Cikis.StokCikisId=Detay.StokCikisId
	join ChepStokGirisDetay GirisDetay on Detay.StokGirisDetayId=GirisDetay.StokGirisDetayId