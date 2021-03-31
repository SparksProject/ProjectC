using System.ComponentModel.DataAnnotations;

namespace ProjectC.Data.Models
{
    public class ChepStokCikisDetay
    {
        [Key]
        public int StokCikisDetayId { get; set; }
        public int StokCikisId { get; set; }
        public int StokGirisDetayId { get; set; }
        public int? Miktar { get; set; }
        public int? Kg { get; set; }
    }
}