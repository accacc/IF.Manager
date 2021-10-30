using System;
using System.Collections.Generic;
using System.Text;

namespace { namespace  }
{
    public class UserRole
    {
        public long UserId { get; set; } // UserId

        public int RoleId { get; set; } // RoleId

        public bool IsStatic { get; set; } // IsStatic

        public bool? IsCustom { get; set; } // IsStatic

        // Foreign keys
        public virtual IFRole Role { get; set; } // FK_KsUserRole_KsRole

        public virtual User CreatorUser { get; set; } // FK_KsUserRole_CreatorKsUser

        public virtual User User { get; set; } // FK_KsUserRole_KsUser

        public UserRole()
        {
        }
    }
}
