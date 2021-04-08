using System;
using System.Collections.Generic;

namespace Chep.DTO
{
    public class GenericReportDTO
    {
        public int GenericReportId { get; set; }
        public string GenericReportName { get; set; }
        public string SqlScript { get; set; }
        public bool IsDefaultReport { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public string RecordStatusName { get; set; }
        public string IsDefaultReportDisplay { get { return IsDefaultReport == true ? "Evet" : "Hayır"; } }
        public List<GenericReportParameterDTO> GenericReportParameterList { get; set; }
        public List<GenericReportUserDTO> GenericReportUserList { get; set; }
        public List<int> UserList { get; set; }
        public int UserId { get; set; }


        //  Only Get
        public int ParameterCount { get { return GenericReportParameterList == null ? 0 : GenericReportParameterList.Count; } }
        public int UserCount { get { return GenericReportUserList == null ? 0 : GenericReportUserList.Count; } }
        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy HH:mm"); } }
    }
}