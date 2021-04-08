using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class ChepStokCikis
    {
        public ChepStokCikis()
        {
            ChepStokCikisDetay = new HashSet<ChepStokCikisDetay>();
        }

        public int StokCikisId { get; set; }
        public string ReferansNo { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public Guid? IhracatciFirma { get; set; }
        public string TpsNo { get; set; }
        public DateTime? TpsTarih { get; set; }

        public virtual Customer IhracatciFirmaNavigation { get; set; }
        public virtual ICollection<ChepStokCikisDetay> ChepStokCikisDetay { get; set; }
    }
}
