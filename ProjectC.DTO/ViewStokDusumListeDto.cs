using System;

namespace ProjectC.DTO
{
    public class ViewStokDusumListeDto
    {
        public int StokGirisDetayId { get; set; }
        public string GirisBeyannameNo { get; set; }
        public string GirisReferansNo { get; set; }
        public DateTime? SureSonuTarihi { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string UrunKod { get; set; }
        public string TPSNo { get; set; }
        public string PONo { get; set; }
        public int GirisMiktar { get; set; }
        public int KalanMiktar { get; set; }
        public int DusulenMiktar { get; set; }
        public int BakiyeMiktar { get { return KalanMiktar - DusulenMiktar; } }
    }
}