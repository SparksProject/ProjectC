using System;
using System.Collections.Generic;

namespace ProjectC.DTO
{
    public class ChepStokCikisDTO
    {
        public int StokCikisId { get; set; }
        public string ReferansNo { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public DateTime? TPSTarih { get; set; }
        public string BeyannameNo { get; set; }
        public string IhracatciFirma { get; set; }
        public string TPSNo { get; set; }

        public string IslemTarihiDisplay => $"{IslemTarihi:dd.MM.yyyy}";
        public string TPSTarihDisplay => $"{TPSTarih:dd.MM.yyyy}";
        public string BeyannameTarihiDisplay => $"{BeyannameTarihi:dd.MM.yyyy}";

        public List<ChepStokCikisDetayDTO> ChepStokCikisDetayList { get; set; }
    }
}
