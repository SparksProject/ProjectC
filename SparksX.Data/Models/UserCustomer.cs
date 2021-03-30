using System;
using System.ComponentModel.DataAnnotations;

namespace SparksX.Data.Models
{
   public class UserCustomer
    {
        [Key]
        public int UserCustomerId { get; set; }
        public int? UserId { get; set; }
        public Guid CustomerId { get; set; }


        public virtual Customer Customer { get; set; }
    }
}