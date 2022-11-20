using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class AuthSettings
    {
        public string PrivateKey { get; set; }
        public string SecretKey { get; set; }
        public string Authority { get; set; }
        public bool RequireHttps { get; set; }
        public int TokenExpiry { get; set; }
        public string Issuer { get; set; }
    }
}
