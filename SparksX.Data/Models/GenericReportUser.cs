namespace SparksX.Data.Models
{
    public partial class GenericReportUser
    {
        public int GenericReportUserId { get; set; }
        public int GenericReportId { get; set; }
        public int UserId { get; set; }

        public virtual GenericReport GenericReport { get; set; }
        public virtual User User { get; set; }
    }
}
