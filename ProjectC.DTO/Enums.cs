namespace Chep.DTO
{
    public class Enums
    {
        public enum ResponseMessage : byte
        {
            OK = 1,
            ERROR = 2,
            NOTFOUND = 3,
            UNAUTHORIZED = 4,
            WARNING = 5,
        }

        public enum PeriodType
        {
            Saatlik_Gunluk = 1,
            Haftalik = 2,
            Aylik = 3,
            IsGunu = 4,
        }

        public enum ReceiverType : byte
        {
            To = 1,
            Cc = 2
        }

        public enum UserType : byte
        {
            Admin = 1,
            User = 2
        }

        public enum ParameterType : byte
        {
            Metin = 0,
            Tarih = 1,
        }
    }
}