﻿namespace Chep.DTO
{
    public class ChepStokCikisDetayDTO
    {
        public int StokCikisDetayId { get; set; }
        public int StokCikisId { get; set; }
        public int StokGirisDetayId { get; set; }
        public int? Miktar { get; set; }
        public decimal? Kg { get; set; }
    }
}
