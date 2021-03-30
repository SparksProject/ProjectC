namespace ProjectC.Data.Models
{
    public partial class GenericReportParameter
    {
        public int GenericReportParameterId { get; set; }
        public int GenericReportId { get; set; }
        public string GenericReportParameterName { get; set; }
        public string ParameterLabel { get; set; }
        public short ParameterType { get; set; }

        public virtual GenericReport GenericReport { get; set; }
    }
}
