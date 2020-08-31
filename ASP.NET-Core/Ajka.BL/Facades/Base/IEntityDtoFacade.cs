using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.Common.Models.Base;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Facades.Base
{
    public interface IEntityDtoFacade<TEntityDto, in TKey> where TEntityDto : IEntity<TKey>
    {
        Task<TEntityDto> GetDetailAsync(TKey id, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(TEntityDto entity, CancellationToken cancellationToken);

        Task<int> CreateAsync(TEntityDto entity, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken);

        Task<IEnumerable<TEntityDto>> GetFilteredAsync(CrudFilterModel filterData, CancellationToken cancellationToken);
    }
}
