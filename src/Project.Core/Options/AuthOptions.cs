using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Project.Core.Options
{
    public class AuthOptions
    {
        public const string ISSUER = "pva"; // издатель токена
        public const string AUDIENCE = "Users_of_my_Great_Finance_system"; // потребитель токена
        const string KEY = "Top_secret_key_101";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
