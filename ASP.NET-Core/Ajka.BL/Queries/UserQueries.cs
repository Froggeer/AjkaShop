using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.User;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajka.BL.Queries
{
    public class UserQueries : IUserQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public UserQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserAsync(IAjkaShopDbContext ajkaShopDbContext, string email, CancellationToken cancellationToken)
        {
            var userRecord = await ajkaShopDbContext.User.AsNoTracking()
                                                    .Where(x => x.Email == email)
                                                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                                                    .ConfigureAwait(false);
            return _mapper.Map<UserDto>(userRecord);
        }

        public async Task<IEnumerable<UserDto>> GetUsersValidAsync(IAjkaShopDbContext ajkaShopDbContext, CancellationToken cancellationToken)
        {
            var userRecords = await ajkaShopDbContext.User.AsNoTracking()
                                                     .Where(x => x.IsValid)
                                                     .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<UserDto>>(userRecords);
        }
    }
}
