using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model.Identity
{
    public  partial class Defaults
    {
        public const string SysName = "Admin";
        public const string SysUserEmail = "Admin@walurecapital.com";
        public static readonly Guid SysUserId = Guid.Parse("9BF9C4ED-96DD-40B2-A63D-B0AED45F2848");
       
    }
}