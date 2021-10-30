using System;
using System.Collections.Generic;
using System.Text;

namespace {namespace}
{
    public class IFRolePermission : Entity
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; } // RoleId

        public int PermissionId { get; set; } // PermissionId

        //public bool IsStatic { get; set; } // IsStatic



        public virtual IFRole Role { get; set; } // FK_KsRolePermission_KsRole

        //public virtual User User { get; set; } // FK_KsRolePermission_CreatorKsUser

        public RolePermission()
        {
            //CreationTime = DateTime.Now;
        }
    }
}

