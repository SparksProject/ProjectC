using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SparksX.Data.Models
{
    public class ChepStokCikisDetay
    {
        [Key]
        public int StokCikisDetayId { get; set; }
        public int StokCikisId { get; set; }
        public int StokGirisDetayId { get; set; }
        public int Miktar { get; set; }
    }
}
