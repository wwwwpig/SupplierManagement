using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManagement.Common
{
    /// <summary>
    /// 密码加密解密辅助类
    /// </summary>
    public class PasswordCryptoHelper
    {
        // 密钥（实际使用时应从安全配置中读取，不要硬编码）
        private static readonly string DefaultKey = "YourSecretKey123!@#$%^&*()_+";

        /// <summary>
        /// 使用SHA256进行单向哈希加密（推荐用于密码存储）
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <returns>加密后的哈希字符串</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// 使用SHA256加盐哈希加密（更安全的密码存储方式）
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="salt">盐值（建议每个用户使用不同的盐）</param>
        /// <returns>加密后的哈希字符串</returns>
        public static string HashPasswordWithSalt(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("盐值不能为空", nameof(salt));

            string saltedPassword = password + salt;
            return HashPassword(saltedPassword);
        }

        /// <summary>
        /// 生成随机盐值
        /// </summary>
        /// <param name="length">盐值长度</param>
        /// <returns>盐值字符串</returns>
        public static string GenerateSalt(int length = 32)
        {
            byte[] saltBytes = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// 验证密码是否匹配
        /// </summary>
        /// <param name="inputPassword">输入的密码</param>
        /// <param name="storedHash">存储的哈希值</param>
        /// <param name="salt">盐值（如果使用了加盐）</param>
        /// <returns>是否匹配</returns>
        public static bool VerifyPassword(string inputPassword, string storedHash, string salt = null)
        {
            string hashToCompare = string.IsNullOrEmpty(salt)
                ? HashPassword(inputPassword)
                : HashPasswordWithSalt(inputPassword, salt);

            return hashToCompare.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 使用AES加密（可逆加密，适用于需要解密的场景）
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string EncryptAES(string plainText, string key = null)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("明文不能为空", nameof(plainText));

            key = key ?? DefaultKey;
            byte[] keyBytes = GetValidKey(key);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    // 将IV和加密数据组合
                    byte[] result = new byte[aes.IV.Length + encrypted.Length];
                    Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                    Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        /// <summary>
        /// 使用AES解密
        /// </summary>
        /// <param name="cipherText">加密的Base64字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的明文</returns>
        public static string DecryptAES(string cipherText, string key = null)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("密文不能为空", nameof(cipherText));

            key = key ?? DefaultKey;
            byte[] keyBytes = GetValidKey(key);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // 提取IV和加密数据
                byte[] iv = new byte[aes.IV.Length];
                byte[] encrypted = new byte[cipherBytes.Length - iv.Length];

                Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(cipherBytes, iv.Length, encrypted, 0, encrypted.Length);

                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                    return Encoding.UTF8.GetString(decrypted);
                }
            }
        }

        /// <summary>
        /// 获取有效的密钥（确保长度为32字节）
        /// </summary>
        private static byte[] GetValidKey(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] validKey = new byte[32]; // AES-256需要32字节密钥

            if (keyBytes.Length >= 32)
            {
                Array.Copy(keyBytes, validKey, 32);
            }
            else
            {
                Array.Copy(keyBytes, validKey, keyBytes.Length);
            }

            return validKey;
        }
    }
}
