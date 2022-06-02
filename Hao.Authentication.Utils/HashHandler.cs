using System.Security.Cryptography;
using System.Text;

namespace Hao.Authentication.Utils
{
    public static class HashHandler
    {
        /// <summary>
        /// 创建 哈希数据 HMACSHA512
        /// </summary>
        /// <param name="text">密码</param>
        /// <param name="hash">哈希数据</param>
        /// <param name="salt">盐</param>
        public static void CreateHash(string text, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// 验证 哈希数据 HMACSHA512
        /// </summary>
        /// <param name="text">密码</param>
        /// <param name="hash">哈希数据</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static bool VerifyHash(string text, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
