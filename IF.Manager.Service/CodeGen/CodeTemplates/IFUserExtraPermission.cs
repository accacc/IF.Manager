using System;
using System.Collections.Generic;
using System.Text;

namespace {namespace }
{
    public partial class UserExtraPermission
    {

        [Key]
        public int Id { get; set; }
        public long UserId { get; set; }
        public int PermissionId { get; set; }
        public byte Type { get; set; }

        public Permission Permission { get; set; }
        public User User { get; set; }
    }
}
