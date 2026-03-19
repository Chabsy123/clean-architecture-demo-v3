using Application.DTOs;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistance.IdentityModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Persistance.SharedServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        public async Task<ApiResponse<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"User not registered with this {request.Email}");
            }
            //if (!user.EmailConfirmed)
            //{
            //    throw new ApiException($"Email not confirmed, pls confirm your email to login");
            //}

            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new ApiException($"Your account is locked, please try again later");
            }

            var succeeded = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!succeeded)
            {
                await _userManager.AccessFailedAsync(user);
                throw new ApiException($"Email or password is incorrect");
            }

            var jwtSecurity = await GenerateTokenAsync(user);
            var authenticationResponse = new AuthenticationResponse();

            authenticationResponse.Id = user.Id;
            authenticationResponse.Username = user.UserName;
            authenticationResponse.Email = user.Email;
            authenticationResponse.IsVerified = user.EmailConfirmed;

            var roles = await _userManager.GetRolesAsync(user);
            authenticationResponse.Roles = roles.ToList();

            authenticationResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurity);

            return new ApiResponse<AuthenticationResponse>(authenticationResponse, "User authenticated successfully");
        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {
            var dbClaim = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }
            //string ipAddress = "192.33";
            var claims = new[]
            { 
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString())
            }
            .Union(dbClaim)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;

        }

        public async Task<ApiResponse<Guid>> RegisterUser(RegisterRequest registerRequest)
        {
            var user = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (user != null)
            {
                throw new ApiException($"User already taken {registerRequest.Email}");
            }

            var userModel = new ApplicationUser();

            userModel.UserName = registerRequest.Username;
            userModel.Email = registerRequest.Email;
            userModel.FirstName = registerRequest.FirstName;
            userModel.LastName = registerRequest.LastName;
            userModel.Gender = registerRequest.Gender;
            userModel.PhoneNumberConfirmed = true;

            var result = await _userManager.CreateAsync(userModel, registerRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userModel, Roles.Basic.ToString());
                return new ApiResponse<Guid>(userModel.Id, "User Registered successfully");
            }
            else
            {
                throw new ApiException(result.Errors.ToString());
            }
        }
    }
}
