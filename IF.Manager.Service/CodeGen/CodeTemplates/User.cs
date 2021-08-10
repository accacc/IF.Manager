using System;
using System.Collections.Generic;
using System.Text;

namespace {namespace}
{
    [Table("Users")]
    public class User
    {

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } // UserName

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // Name

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; } // Surname

        public DateTime CreationTime { get; set; }

        //[Required]
        [MaxLength(100)]
        public string EmailAddress { get; set; } // EmailAddress

        public bool IsEmailConfirmed { get; set; } // IsEmailConfirmed

        [MaxLength(50)]
        public string EmailConfirmationCode { get; set; } // EmailConfirmationCode

        [MaxLength(100)]
        public string Password { get; set; } // Password

        [MaxLength(50)]
        public string PasswordResetCode { get; set; } // PasswordResetCode

        public bool IsPasswordReset { get; set; } // IsPasswordReset

        public DateTime LastPasswordSetTime { get; set; } // LastPasswordSetTime

        public DateTime? LastLoginTime { get; set; } // LastLoginTime

        public int LoginCount { get; set; } // LoginCount

        public int FailedLoginCount { get; set; } // FailedLoginCount

        public bool IsBlocked { get; set; } // IsBlocked

        public bool IsActive { get; set; } // IsActive

        public bool IsSystemAdmin { get; set; }

        public UserType UserType { get; set; }

        [MaxLength(13)]
        public string Tckn { get; set; }

        [MaxLength(500)]
        public string Title { get; set; }

        //[MaxLength(50)]
        //public string VendorCode { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [Key]
        public int Id { get; set; }


        public User()
        {
            IsEmailConfirmed = false;
            CreationTime = DateTime.Now;
        }
    }
}
