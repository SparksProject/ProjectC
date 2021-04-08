using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class VwStokCikisDetayListe
    {
        public int StokCikisId { get; set; }
        public string ReferansNo { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public string IhracatciFirma { get; set; }
        public string Tpsno { get; set; }
        public DateTime? Tpstarih { get; set; }
        public int StokCikisDetayId { get; set; }
        public string EsyaCinsi { get; set; }
        public string EsyaGtip { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string UrunKod { get; set; }
        public string Pono { get; set; }
        public int? GirisMiktar { get; set; }
        public int? CikisMiktar { get; set; }
        public int StokGirisDetayId { get; set; }
        public int UserId { get; set; }
    }
}
