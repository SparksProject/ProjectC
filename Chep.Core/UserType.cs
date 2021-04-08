using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
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
