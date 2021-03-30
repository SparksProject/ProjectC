using System;

namespace SparksX.Api.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
    }
}