using DataAccess.Dto;
using DataAccess.IServices;
using DataAccess.Model.Identity;
using Iposweb.Core.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Share.Constants;
using Share.Controler;
using Share.Enums;
using Share.Validation;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
#nullable disable

namespace Onboarding.Controllers
{
    /// <summary>
    /// Api for Account and User registration
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IAccountServices _accountServices;
        private readonly UserManager<WalureUser> _userManager;
        private readonly SignInManager<WalureUser> _signInManager;

        /// <summary>
        /// 
        /// </summary>
        public AccountController(IAccountServices accountServices,UserManager<WalureUser> userManager, SignInManager<WalureUser> signInManager, IOptions<IdentityOptions> identityOptions)
        {
            _accountServices = accountServices;
            _userManager = userManager;
            _signInManager = signInManager;
            _identityOptions = identityOptions;
        }

        /// <summary>
        /// Endpoint for User Registration
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            
            if (model == null)
            {
                return ApiResponse(null, " Model can not be null", ApiResponseCodes.FAIL);
            }

            var result = await _accountServices.RegisterUser(model);
            if(_accountServices.HasError)
            {
                return ApiResponse(result, _accountServices.Errors, codes: ApiResponseCodes.FAIL);
            }
            return ApiResponse(result, Constant.Sucessfull, codes: ApiResponseCodes.OK);
        }
        /// <summary>
        /// Login EndPoint
        /// </summary>
        [AllowAnonymous]
        [HttpPost("~/api/auth/token"), Produces("application/json")]
        public async Task<IActionResult> Token()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {
                var result = await PasswordSignIn(request);
                return result;
            }
            else if (request.IsRefreshTokenGrantType())
            {
                var result = await RefreshToken(request);
                return result;
            }

            return Forbid(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                   properties: new AuthenticationProperties(new Dictionary<string, string>
                   {
                       [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                       [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The specified grant type is not supported."
                   }));
        }

        private async Task<IActionResult> RefreshToken(OpenIddictRequest request)
        {
            // Retrieve the claims principal stored in the refresh token.
            var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Retrieve the user profile corresponding to the refresh token.
            // Note: if you want to automatically invalidate the refresh token
            // when the user password/roles change, use the following line instead:
            // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
            var user = await _userManager.GetUserAsync(info.Principal);

            if (user == null)
            {
                return Forbid(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                   properties: new AuthenticationProperties(new Dictionary<string, string>
                   {
                       [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                       [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid."
                   }));
            }

            // Ensure the user is still allowed to sign in.
            if (!await _signInManager.CanSignInAsync(user))
            {
                return Forbid(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                   properties: new AuthenticationProperties(new Dictionary<string, string>
                   {
                       [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                       [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                   }));
            }

            // Create a new authentication ticket, but reuse the properties stored
            // in the refresh token, including the scopes originally granted.
            var principal = await CreateTicketAsync(request, user, info.Properties);
            var signInResult = SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return signInResult;
        }

        private async Task<IActionResult> PasswordSignIn(OpenIddictRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Username)
                    ?? await _userManager.FindByNameAsync(request.Username);

            if (user == null || user.IsDeleted)
            {
                return Forbid(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                     properties: new AuthenticationProperties(new Dictionary<string, string>
                     {
                         [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                         [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Email or password is incorrect."
                     }));
            }

            // Ensure the user is allowed to sign in.
            if (!await _signInManager.CanSignInAsync(user))
            {
                return Forbid(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                   properties: new AuthenticationProperties(new Dictionary<string, string>
                   {
                       [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                       [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "You are not allowed to sign in."
                   }));
            }

          
            if (!String.IsNullOrEmpty(request.Password))
            {
                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Forbid(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                  properties: new AuthenticationProperties(new Dictionary<string, string>
                  {
                      [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                      [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Username or password is incorrect."
                  }));
                }
            }
            // Create a new authentication ticket.
            var principal = await CreateTicketAsync(request, user);

            var signInResult = SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
          
            return signInResult;
        }

        private async Task<ClaimsPrincipal> CreateTicketAsync(OpenIddictRequest oidcRequest, WalureUser user,
          AuthenticationProperties properties = null)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            await _accountServices.AddUserClaims(user, identity);

            // Create a new authentication ticket holding the user identity.

            if (!oidcRequest.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                principal.SetScopes(new[]
                {
                   Scopes.OpenId,
                   Scopes.Email,
                   Scopes.Profile,
                   Scopes.Phone,
                   Scopes.Roles,
                }.Intersect(oidcRequest.GetScopes()));
            }

            principal.SetResources("resource_server");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            var destinations = new List<string>
            {
                Destinations.AccessToken
            };

            foreach (var claim in principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == Claims.Name && principal.HasScope(Scopes.Profile)) ||
                    (claim.Type == Claims.Email && principal.HasScope(Scopes.Email)) ||
                    (claim.Type == Claims.Role && principal.HasScope(Scopes.Roles)) ||
                    (claim.Type == Claims.Audience && principal.HasScope(Claims.Audience)))
                {
                    destinations.Add(Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                var name = new Claim(Claims.GivenName, user.FirstName ?? "[NA]");
                name.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
                identity.AddClaim(name);
            }

           
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();

            identity.AddClaim(new Claim(Scopes.Roles, userRole));
            return principal;
        }


    }
}
