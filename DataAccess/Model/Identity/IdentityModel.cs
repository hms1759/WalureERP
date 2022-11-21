using DataAccess.Enums;
using Microsoft.AspNetCore.Identity;
using Share.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model.Identity
{
    public class WalureUser : IdentityUser<Guid>
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? RefreshToken { get; set; }
        public UserTypes UserType { get; set; }
        public Genders Gender { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class WalureUserClaim : IdentityUserClaim<Guid>
    {
    }

    public class WalureUserLogin : IdentityUserLogin<Guid>
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }

    public class WalureRole : IdentityRole<Guid>
    {
        public bool InBuilt { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }

    public class WalureUserRole : IdentityUserRole<Guid>
    {
    }

    public class WalureRoleClaim : IdentityRoleClaim<Guid>
    {

    }
    public class WalureUserToken : IdentityUserToken<Guid>
    {
    }
}


