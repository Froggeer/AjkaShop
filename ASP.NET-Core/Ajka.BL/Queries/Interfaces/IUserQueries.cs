using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.User;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Queries.Interfaces
{
    public interface IUserQueries
    {
        Task<IEnumerable<UserDto>> GetUsersValidAsync(IAjkaShopDbContext ajkaShopDbContext, CancellationToken cancellationToken);
        Task<UserDto> GetUserAsync(IAjkaShopDbContext ajkaShopDbContext, string email, CancellationToken cancellationToken);
    }
}