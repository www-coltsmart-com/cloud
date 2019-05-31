using ColtSmart.Data;
using System;

namespace ColtSmart.Entity.Entities
{
    public class TUser
    {
        [IgnoreInsert]
        [Key]
        public int Id { get; set; }

        public string UserNo { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RegEmall { get; set; }

        public DateTime RegDate { get; set; }

        public string MobilePhone { get; set; }

        public string Company { get; set; }

        public EUserType UserType { get; set; } = EUserType.Custom;

        [Ignore]
        public int DevCount { get; set; }

        [Ignore]
        public string NewPassword { get; set; }
    }

    public enum EUserType
    {
        Admin=1,
        Custom=2
    }
}
