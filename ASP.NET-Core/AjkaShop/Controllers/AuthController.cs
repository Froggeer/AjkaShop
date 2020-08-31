using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Base;
using Ajka.BL.Services.Base.Interfaces;
using AjkaShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Authorization for users.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ControllerName("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Returns an access token from OAuth using a login name and password.
        /// </summary>
        /// <param name="loginModel">Username and password</param>
        /// <param name="cancellationToken"></param>
        /// <returns>access token</returns>
        [AllowAnonymous]
        [HttpPost, Route(nameof(Login))]
        public async Task<AuthResponseDto> Login([FromBody] AuthLoginModel loginModel, CancellationToken cancellationToken = default)
        {
            return await authService.Authenticate(loginModel.Username, loginModel.Password, cancellationToken).ConfigureAwait(false);
        }
    }
}
