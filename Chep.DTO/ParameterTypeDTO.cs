namespace Chep.DTO
{
    public class ParameterTypeDTO // bu class'ın DB'de karşılığı yok. manuel olarak Enum'dan generate ediliyor.
    {
        public short ParameterTypeId { get; set; }
        public string ParameterTypeName { get; set; }
    }
}