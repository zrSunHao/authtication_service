using Hao.Authentication.Persistence.Attributes;
using Hao.Authentication.Persistence.Consts;
using Hao.Authentication.Persistence.Database;

namespace Hao.Authentication.Persistence.Entities
{
    [TablePrefix(PrefixConsts.Customer)]
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public string Nickname { get; set; }

        public byte[] Password { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Remark { get; set; }
    }
}
