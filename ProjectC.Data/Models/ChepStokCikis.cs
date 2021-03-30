﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectC.Data.Models
{
    public class ChepStokCikis
    {
        [Key]
        public int StokCikisId { get; set; }
        public string ReferansNo { get; set; }
        public DateTime IslemTarihi { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime BeyannameTarihi { get; set; }
        public string IhracatciFirma { get; set; }
        public string TPSNo { get; set; }
        public DateTime TPSTarih { get; set; }
    }
}
