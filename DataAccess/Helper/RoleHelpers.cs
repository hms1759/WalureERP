using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper
{
    public static class RoleHelpers
    {
        public static Guid INBUILT_ADMIN_ID() => Guid.Parse("62BA02D3-FDC4-4A57-9DB5-6608212F1106");
        public const string INBUILT_ADMIN = nameof(INBUILT_ADMIN);
        public static Guid WALURE_ADMIN_ID() => Guid.Parse("57240EF7-DABA-4CE2-9224-34F5EA110F55");
        public const string WALURE_ADMIN = nameof(WALURE_ADMIN);
        public static Guid WALURE_BASIC_USER_ID() => Guid.Parse("5A30AC88-4CCE-45E5-95C3-4F540B682402");
        public const string WALURE_BASIC_USER = nameof(WALURE_BASIC_USER);

    }
}