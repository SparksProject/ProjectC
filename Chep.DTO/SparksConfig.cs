using System.ComponentModel.DataAnnotations;

namespace Chep.DTO
{
    public class SparksConfig
    {
        public string ConnectionString { get; set; }
        public TokenManagement TokenManagement { get; set; }
    }

    public class TokenManagement
    {
        [Display(Name = "secret")]
        public string Secret { get; set; }
        [Display(Name = "issuer")]
        public string Issuer { get; set; }
        [Display(Name = "audience")]
        public string Audience { get; set; }
        [Display(Name = "accessExpiration")]
        public int AccessExpiration { get; set; }
        [Display(Name = "refreshExpiration")]
        public int RefreshExpiration { get; set; }
        [Display(Name = "expiryDuration")]
        public int ExpiryDuration { get; set; }
    }
}