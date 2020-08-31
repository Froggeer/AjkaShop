using Ajka.BL.Models.Base;
using Ajka.DAL.Model.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Queries.Interfaces
{
    public interface IIndividualVariableQueries
    {
        Task<IndividualVariableDto> GetIndividualVariableAsync(IAjkaShopDbContext ajkaShopDbContext, string keyName, CancellationToken cancellationToken);
    }
}
