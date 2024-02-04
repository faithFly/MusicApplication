using System.Security.Cryptography;
using System.Text;

namespace Music.Base;

public class MD5Helper
{
    public static string GetMD5Hash(string str)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2")); // 将每个字节转换为 2 位的十六进制表示
            }

            return stringBuilder.ToString();
        }
    }
}