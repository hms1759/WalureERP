using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper
{
    public static class RoleHelpers
    {
        public static Guid INBUILT_ADMIN_ID() => Guid.Parse("a1b6b6b0-0825-4975-a93d-df3dc86f8cc7");
        public const string INBUILT_ADMIN = nameof(INBUILT_ADMIN);
        public static Guid WALURE_ADMIN_ID() => Guid.Parse("3134ff36-f284-4634-a2d1-31f6ddaf2668");
        public const string WALURE_ADMIN = nameof(WALURE_ADMIN);
        public static Guid WALURE_BASIC_USER_ID() => Guid.Parse("3134fd36-f284-4634-a2d1-31f6ddef2668");
        public const string WALURE_BASIC_USER = nameof(WALURE_BASIC_USER);

    }
}
