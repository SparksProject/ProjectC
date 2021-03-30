using System.Collections.Generic;

namespace ProjectC.Data.Models
{
    public partial class UserType
    {
        public UserType()
        {
            User = new HashSet<User>();
        }

        public byte UserTypeId { get; set; }
        public string UserTypeName { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
