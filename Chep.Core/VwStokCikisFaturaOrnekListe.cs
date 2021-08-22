using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class VwStokCikisFaturaOrnekListe
    {
        public int ÇıkışReferansNo { get; set; }
        public string ÜrünKodu { get; set; }
        public string Gtip { get; set; }
        public string TicariTanım { get; set; }
        public int? Miktar { get; set; }
        public decimal? BirimFiyat { get; set; }
        public decimal? FaturaTutarı { get; set; }
        public decimal? NetKg { get; set; }
        public decimal? BrütKg { get; set; }
        public string Menşei { get; set; }
        public string Gümrük { get; set; }
        public string TeslimŞekli { get; set; }
        public string İthalatBeyannameNo { get; set; }
        public DateTime? İthalatBeyannameTarihi { get; set; }
        public int? İthalatBeyannameKalemNo { get; set; }
        public string İhracatTpsNo { get; set; }
        public DateTime? İhracatTpsTarihi { get; set; }
        public string AlıcıFirma { get; set; }
        public string AliciAdres { get; set; }
    }
}
