using System;

namespace Chep.DTO
{
    public class UserCustomerDTO
    {

        public int UserCustomerId { get; set; }

        public int? UserId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }
    }
}