using System;

namespace SparksX.DTO
{
    public class GenericReportParameterDTO
    {
        public int GenericReportParameterId { get; set; }
        public int GenericReportId { get; set; }
        public string GenericReportParameterName { get; set; }
        public string ParameterLabel { get; set; }
        public short ParameterType { get; set; }

        public string GenericReportParameterValue { get; set; }
        public string ParameterTypeDisplay
        {
            get
            {
                return Enum.GetName(typeof(Enums.ParameterType), ParameterType);
            }
        }
    }
}