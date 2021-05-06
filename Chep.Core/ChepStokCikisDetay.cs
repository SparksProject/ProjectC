using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class ChepStokCikisDetay
    {
        public int StokCikisDetayId { get; set; }
        public int StokCikisId { get; set; }
        public int StokGirisDetayId { get; set; }
        public int? Miktar { get; set; }
        public decimal? Kg { get; set; }
        public Guid? InvoiceDetailId { get; set; }
        public decimal? InvoiceAmount { get; set; }

        public virtual ChepStokCikis StokCikis { get; set; }
        public virtual ChepStokGirisDetay StokGirisDetay { get; set; }
    }
}
