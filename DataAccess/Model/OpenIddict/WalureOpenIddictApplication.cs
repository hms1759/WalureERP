using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model.OpenIddict
{
    public class WalureOpenIddictApplication : OpenIddictEntityFrameworkCoreApplication<Guid, WalureOpenIddictAuthorization, WalureOpenIddictToken>
    {
        public WalureOpenIddictApplication()
        {
            Id = Guid.NewGuid();
        }
        public string? AppId { get; set; }
        public string? Language { get; set; }
    }

    public class WalureOpenIddictAuthorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, WalureOpenIddictApplication, WalureOpenIddictToken> { }
    public class WalureOpenIddictScope : OpenIddictEntityFrameworkCoreScope<Guid> { }
    public class WalureOpenIddictToken : OpenIddictEntityFrameworkCoreToken<Guid, WalureOpenIddictApplication, WalureOpenIddictAuthorization> { }
}
