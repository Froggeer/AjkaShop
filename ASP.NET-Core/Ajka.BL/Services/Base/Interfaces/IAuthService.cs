using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.Base;

namespace Ajka.BL.Services.Base.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Authenticate(string userName, string password, CancellationToken cancellationToken);
    }
}
