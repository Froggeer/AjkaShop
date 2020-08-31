using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajka.BL.Queries
{
    public class ItemCardImageQueries : IItemCardImageQueries, IAjkaShopService
    {
        private readonly IMapper _mapper;

        public ItemCardImageQueries(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemCardImage>> GetEntitiesAsync(IAjkaShopDbContext ajkaShopDbContext, int itemCardId, CancellationToken cancellationToken)
        {
            return await ajkaShopDbContext.ItemCardImage.AsNoTracking()
                .Where(x => x.ItemCardId == itemCardId)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ItemCardImageDto>> GetItemCardImagesAsync(IAjkaShopDbContext ajkaShopDbContext, int itemCardId, CancellationToken cancellationToken)
        {
            var itemCardImages = await ajkaShopDbContext.ItemCardImage.AsNoTracking()
                .Where(x => x.ItemCardId == itemCardId)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ItemCardImageDto>>(itemCardImages);
        }
    }
}
