using System;
using System.Collections.Generic;

namespace Chep.DTO
{
    public class ChepStokCikisDTO
    {
        public int StokCikisId { get; set; }
        public string ReferansNo { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public DateTime? TpsTarih { get; set; }
        public string BeyannameNo { get; set; }
        public Guid? IhracatciFirma { get; set; }
        public string TpsNo { get; set; }

        public string IhracatciFirmaName { get; set; }
        public string IslemTarihiDisplay => $"{IslemTarihi:dd.MM.yyyy}";
        public string TpsTarihDisplay => $"{TpsTarih:dd.MM.yyyy}";
        public string BeyannameTarihiDisplay => $"{BeyannameTarihi:dd.MM.yyyy}";

        public List<ChepStokCikisDetayDTO> ChepStokCikisDetayList { get; set; }
    }
}
