using System;

namespace Chep.DTO
{
    public class ViewStokDusumListeDto
    {
        public int StokGirisDetayId { get; set; }
        public int StokGirisId { get; set; }
        public string TPSNo { get; set; }
        public string GirisReferansNo { get; set; }
        public DateTime? SureSonuTarihi { get; set; }
        public string GirisBeyannameNo { get; set; }
        public DateTime? GirisBeyannameTarihi { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string UrunKod { get; set; }
        public string Pono { get; set; }
        public int? GirisMiktar { get; set; }
        public int CikisMiktar { get; set; }
        public int? KalanMiktar { get; set; }
        public int UserId { get; set; }
        public int DusulenMiktar { get; set; }
        public decimal? BirimTutar { get; set; }
        public int BakiyeMiktar
        {
            get
            {
                if (!KalanMiktar.HasValue)
                {
                    return 0;
                }

                return KalanMiktar.Value - DusulenMiktar;
            }
        }
        public int? TpsCikisSiraNo { get; set; }
        public decimal? FaturaTutar { get; set; }
      
    }
}