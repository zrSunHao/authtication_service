using Hao.Authentication.Persistence.Database;

namespace Hao.Authentication.Persistence.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public string Nickname { get; set; }

        public byte[] Password { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Remark { get; set; }
    }
}
