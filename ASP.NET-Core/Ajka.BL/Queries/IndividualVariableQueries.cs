using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ajka.BL.Models.Base;

namespace Ajka.BL.Queries
{
    public class IndividualVariableQueries : IIndividualVariableQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public IndividualVariableQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IndividualVariableDto> GetIndividualVariableAsync(IAjkaShopDbContext ajkaShopDbContext, string keyName, CancellationToken cancellationToken)
        {
            var variable = await ajkaShopDbContext.IndividualVariable.AsNoTracking()
                .Where(x => x.KeyName == keyName)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IndividualVariableDto>(variable);
        }
    }
}
