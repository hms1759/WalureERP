using DataAccess.Model.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Password
{
    public class CustomPasswordHasher : PasswordHasher<WalureUser>
    {
        public override string HashPassword(WalureUser user, string password)
        {
           var enhancedHashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            return enhancedHashPassword;
        }

        public override PasswordVerificationResult VerifyHashedPassword(WalureUser user, string hashedPassword, string providedPassword)
        {
            var validatePassword = BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
            return validatePassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}