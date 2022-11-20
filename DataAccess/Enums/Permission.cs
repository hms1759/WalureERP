
using DataAccess.Helper;
using System.ComponentModel;

namespace DataAccess.Enums
{
    public enum Permission
    {// Superior Admin
        [Category(RoleHelpers.INBUILT_ADMIN), Description(@"Access All Modules")]
        FULL_CONTROL = 10,
//Admin
        [Category(RoleHelpers.WALURE_ADMIN), Description(@"Access His Department")]
        FULL_DEPARTMENT_CONTROL = 20,
//Basic Staffs
        [Category(RoleHelpers.WALURE_BASIC_USER), Description(@"Access To His Office")]
        FULL_OFFICE_CONTROL = 30
    }
}
