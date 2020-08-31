using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Base;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.Common.Constants.Service;
using Ajka.Common.Helpers;
using Ajka.DAL.Model.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ajka.BL.Services.Base
{
    public class AuthService : IAuthService, IAjkaShopService
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IUserQueries _userQueries;
        private readonly AppSettings _appSettings;

        public AuthService(IAjkaShopDbContext ajkaShopDbContext,
                           IUserQueries userQueries,
                           IOptions<AppSettings> appSettings)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _userQueries = userQueries;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthResponseDto> Authenticate(string email, string password, CancellationToken cancellationToken)
        {
            var userRecord = await _userQueries.GetUserAsync(_ajkaShopDbContext, email, cancellationToken).ConfigureAwait(false);
            if(userRecord == null)
            {
                return new AuthResponseDto
                {
                    ErrorMessage = AuthConstants.errorNameOrPasswordIsInvalid
                };
            }
            if (!userRecord.Password.Equals(PasswordSecurityHelper.HashPassword(password, _appSettings.PasswordSalt)))
            {
                return new AuthResponseDto
                {
                    ErrorMessage = AuthConstants.errorNameOrPasswordIsInvalid
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.ClientSecret);
            var claim = new Claim(ClaimTypes.Role, RoleConstants.LoggedInUserRole);
            if (userRecord.IsAdministrator)
            {
                claim = new Claim(ClaimTypes.Role, RoleConstants.AdministratorRole);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    claim,
                    new Claim(ClaimTypes.Name, userRecord?.Name),
                    new Claim(ClaimTypes.Surname, userRecord?.Surname),
                    new Claim(ClaimTypes.Email, userRecord?.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthResponseDto
            {
                UserId = userRecord.Id,
                AccessToken = tokenHandler.WriteToken(token)
            };
        }
    }
}
