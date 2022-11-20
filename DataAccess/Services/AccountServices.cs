

#pragma warning disable CS1591 
using DataAccess.Dto;
using DataAccess.Helper;
using DataAccess.IServices;
using DataAccess.Model;
using DataAccess.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using Share.Constants;
using Share.Enums;
using Share.Extensions;
using Share.Validation;
using Shared.Dapper;
using Shared.Dapper.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
#nullable disable

namespace DataAccess.Services
{
    public class AccountServices : Service<Staffs>, IAccountServices
    {

        private readonly UserManager<WalureUser> _userManager;
        private readonly RoleManager<WalureRole> _roleManager;
        public AccountServices(IUnitOfWork unitOfWork, UserManager<WalureUser> userManager, RoleManager<WalureRole> roleManager) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddUserClaims(WalureUser user, ClaimsIdentity identity)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (identity == null)
                throw new ArgumentNullException(nameof(identity));


            if (!string.IsNullOrEmpty(user.FirstName))
                identity.AddClaim(new Claim(ClaimTypeHelpers.FirstName, user.FirstName));

            if (!string.IsNullOrEmpty(user.LastName))
                identity.AddClaim(new Claim(ClaimTypeHelpers.LastName, user.LastName));

            string userType = user.UserType.GetDescription();

            if (!string.IsNullOrWhiteSpace(userType))
                identity.AddClaim(new Claim(ClaimTypeHelpers.UserType, userType));

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                identity.AddClaim(new Claim(ClaimTypeHelpers.PhoneNumber, "True"));

            else
            {
                identity.AddClaim(new Claim(ClaimTypeHelpers.PhoneNumber, "False"));
            }

        }

        public Task<string> Login(string username, string? password, object? token)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterDto> RegisterUser(RegisterDto model)
        {

            WalureRole? role;
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                this.Results.Add(new ValidationResult("Email is already exist"));
                return model;
            }

            //if (model.Email != RegexValidation.EMAIL_REGEX || model.Email != RegexValidation.EMAIL_REGEX2)
            //{
            //    this.Results.Add(new ValidationResult("Input currect email format"));
            //    return model;
            //}

            //if (model.PhoneNumber != RegexValidation.PHONENUMBER_REGEX || model.PhoneNumber != RegexValidation.ALT_PHONENUMBER_REGEX)
            //{
            //    this.Results.Add(new ValidationResult("Input currect Phone Number format"));
            //    return model;
            //}
            var roleExist = false;
            
            if (!string.IsNullOrWhiteSpace(model.RoleName) && await _roleManager.RoleExistsAsync(model.RoleName))
                roleExist = true;
            if (!roleExist)
            {
                base.Results.Add(new ValidationResult($"{model.RoleName} isn't a valid role."));
                return model;
            }

            else if (model.RoleId.HasValue)
            {
                role = await _roleManager.FindByIdAsync(model.RoleId.Value.ToString());
            }
            
            var UuerModel = await _userManager.FindByNameAsync(model.UserName);

            if (model != null)
            {
                base.Results.Add(new ValidationResult($"User already exist."));
                return model;
            }
            var phoneNumberUser = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);

            if (phoneNumberUser != null)
            {
                base.Results.Add(new ValidationResult($"Phone number already exist for another user."));
                return model;
            }

            var emailUser = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);
            if (emailUser != null)
            {
                base.Results.Add(new ValidationResult($"Email already exist for another user."));
                return model;
            }

            WalureUser walureUser = new()
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserType = model.UserType,
                Gender = model.Gender,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                CreatedOn = DateTime.UtcNow,
                LockoutEnabled = false,
                AccessFailedCount = 1,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false

            };

            var result = await _userManager.CreateAsync(walureUser, Defaults.passWord);

            if (!result.Succeeded)
            {
                this.Results.Add(new ValidationResult(_userManager.ErrorDescriber.ToString()));
                return model;
            }
            var roleName = model.RoleName ;
            result = await _userManager.AddToRoleAsync(walureUser, roleName);

            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(walureUser);
                base.Results.AddRange(result.Errors.Select(e => new ValidationResult(e.Description)));
                return model;
            }

            return model;

        }

    }
}
