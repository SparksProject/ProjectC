Create View vw_EFaturaTaslak
as
select 
	ROW_NUMBER () over (order by c.StokcikisId) as SIRA_NO,'IHRACAT' as FATURA_PROFILI,'' as UUID, c.TpsNo as FATURA_NO,c.InvoiceDate as FATURA_TARIHI,
	'' as FATURA_SAATI,'ISTISNA' as FATURA_TIPI,c.InvoiceCurrency as PARA_BIRIMI,'' as DOVIZ_KURU,'' as ALICI_VKN_TCKN,'' as ALICI_UNVAN_ADI_SOYADI,
	'' as ALICI_VD,'' as ALICI_BINA_ADI,'' as ALICI_BINA_NO,'' as ALICI_KAPI_NO,'' as ALICI_ILCE,'' as ALICI_SEHIR,'' as ALICI_SOKAK,'' as ALICI_ULKE,
	'' as ALICI_POSTA_KODU,'' as ALICI_TELEFON,'' as ALICI_FAX,''as	ALICI_EMAIL,''as ALICI_ETIKET,
	('Toplam Brüt Ağırlık ' + cast((select sum(BrutKg) from ChepStokCikisDetay cd where c.StokCikisId=cd.StokCikisId) as nvarchar )+ ' Kg, Net Ağırlık ' +
	cast((select sum(NetKg) from ChepStokCikisDetay cd where c.StokCikisId=cd.StokCikisId) as nvarchar )+ ' Kg.Toplam Kap '+ cast(c.KapMiktari as nvarchar) + ' Kap, '+
	cast((select sum(Miktar) from ChepStokCikisDetay cd where c.StokCikisId=cd.StokCikisId) as nvarchar)+' Adet.')as FATURA_NOTU_1,
	'Döviz Kuru:----- Total Value is For Customs Purpose Only.' as FATURA_NOTU_2,
	('Malların Tamamı &&&&& Menşeilidir.Çıkış Gümrüğü '+ (select CustomsName from Customs gum where c.CikisGumruk=gum.EdiCode) +'.İş Bu E-Fatura İrsaliye Yerine Geçer.' )as FATURA_NOTU_3,
	'' as SIPARIS_NO,''as SIPARIS_TARIHI,''as IRSALIYE_NO_1,'' as IRSALIYE_TARIHI_1,'' as IRSALIYE_NO_2,'' as IRSALIYE_TARIHI_2,'' as IRSALIYE_NO_3,'' as IRSALIYE_TARIHI_3,
	c.InvoiceAmount as TOPLAM_TUTAR,'' as ISKONTO_TUTARI, c.InvoiceAmount as VERGILER_HARIC_TOPLAM_TUTAR,c.InvoiceAmount as VERGILER_DAHIL_TOPLAM_TUTAR, '0,00' as ODENECEK_TUTAR,
	'0015' as VERGI_KOD_1,'KDV'as VERGI_KATEGORI_1, 0 as VERGI_ORAN_1,c.InvoiceAmount as VERGI_MATRAH_1,'0,00' as VERGI_TUTAR_1,1 as VERGI_HESAPLAMA_SIRASI_1,301 as VERGI_MUAFIYET_KODU_1,
	'11/1-a Mal İhracatı' as VERGI_MUAFIYET_SEBEBI_1,'' as VERGI_KOD_2,''as	VERGI_KATEGORI_2,''as VERGI_ORAN_2,''as	VERGI_MATRAH_2,''as	VERGI_TUTAR_2,''as	VERGI_HESAPLAMA_SIRASI_2,''as VERGI_MUAFIYET_KODU_2,
	''as VERGI_MUAFIYET_SEBEBI_2,''as VERGI_KOD_3,'' as	VERGI_KATEGORI_3,'' as VERGI_ORAN_3,''as VERGI_MATRAH_3,''as VERGI_TUTAR_3,'' as VERGI_HESAPLAMA_SIRASI_3,'' as VERGI_MUAFIYET_KODU_3,'' as	VERGI_MUAFIYET_SEBEBI_3,
	'' as VERGI_KOD_4,'' as	VERGI_KATEGORI_4,'' as VERGI_ORAN_4,'' as VERGI_MATRAH_4,'' as	VERGI_TUTAR_4,'' as	VERGI_HESAPLAMA_SIRASI_4,'' as  VERGI_MUAFIYET_KODU_4,'' as	VERGI_MUAFIYET_SEBEBI_4,'' as VERGI_KOD_5,
	'' as VERGI_KATEGORI_5,'' as VERGI_ORAN_5,'' as	VERGI_MATRAH_5,'' as VERGI_TUTAR_5,'' as VERGI_HESAPLAMA_SIRASI_5,'' as	VERGI_MUAFIYET_KODU_5,'' as	VERGI_MUAFIYET_SEBEBI_5,'2222222222' as IHR_ALICI_VKN,
	AliciFirma.Name as IHR_ALICI_UNVAN,AliciFirma.Name as IHR_ALICI_RESMI_UNVAN,'' as IHR_ALICI_ADI,''as IHR_ALICI_SOYADI,AliciFirma.Adress as IHR_ALICI_ILCE,AliciFirma.City as IHR_ALICI_SEHIR,
	AliciFirma.Country as IHR_ALICI_ULKE,c.OdemeSekli as IHR_ODEME_SEKLI_SARTLARI,'' as IHR_ODEME_KANALI,'' as IHR_ODEME_BANKA_HESAP_NO,'123123' as IHR_TESLIMAT_ID,
	AliciFirma.Adress as IHR_TESLIMAT_ILCE,AliciFirma.City as IHR_TESLIMAT_SEHIR,AliciFirma.Country as IHR_TESLIMAT_ULKE,c.TeslimSekli as IHR_TESLIM_SARTI,c.ReferansNo as IHR_GONDERIM_NO,
	3 as IHR_GONDERIM_SEKLI,'' as IHR_GTIP_NO
from ChepStokCikis c
JOIN Customer AliciFirma on c.AliciFirma=AliciFirma.CustomerId