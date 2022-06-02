using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Domain.Models
{
    public class LoginM
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class RegisterM
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public CustomerGender Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public CustomerEducation Education { get; set; }

        public string Profession { get; set; }

        public string Intro { get; set; }
    }

    public class AuthCtmM
    {
        public string Id { get; set; }

        public string Avatar { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class AuthPeopleM
    {
        public string CustomerId { get; set; }

        public string FullName { get; set; }

        public CustomerGender Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public CustomerEducation Education { get; set; }

        public string Profession { get; set; }

        public string Intro { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }

    public class AuthRoleM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public SysRoleRank Rank { get; set; }
    }

    public class AuthSectPermsM
    {
        public string SectCodes { get; set; }
        public string FunctCodes { get; set; }
    }

    public class AuthResultM
    {
        /// <summary>
        /// 账号信息
        /// </summary>
        public AuthCtmM Customer { get; set; }

        /// <summary>
        /// 个人信息
        /// </summary>
        public AuthPeopleM People { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public AuthRoleM Role { get; set; }

        /// <summary>
        /// 可进入模块
        /// </summary>
        public List<AuthSectPermsM> Perms { get; set; }

        /// <summary>
        /// JWT
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 本次登录主键
        /// </summary>
        public string Key { get; set; }
    }
}
