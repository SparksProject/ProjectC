using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class VwStokDusumListe
    {
        public int StokGirisDetayId { get; set; }
        public int StokGirisId { get; set; }
        public string Tpsno { get; set; }
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
    }
}
