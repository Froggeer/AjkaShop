using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Exceptions;
using Ajka.Common.Constants.Base;
using Ajka.Common.Models.Base;
using Ajka.DAL;
using Ajka.DAL.Model.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajka.BL.Facades.Base
{
    public class RepositoryCrudFacade<TEntityDto, TEntity, TKey> where TEntity : class, IEntity<TKey> where TEntityDto : IEntity<TKey>
    {
        private readonly AjkaShopDbContext ajkaShopDbContext;
        private readonly IMapper _mapper;

        protected RepositoryCrudFacade(AjkaShopDbContext ajkaShopDbContext,
                                   IMapper mapper)
        {
            this.ajkaShopDbContext = ajkaShopDbContext;
            _mapper = mapper;
        }

        public async Task<TEntityDto> GetDetailAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await ajkaShopDbContext.Set<TEntity>().AsQueryable()
                                                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken)
                                                .ConfigureAwait(false);
            return _mapper.Map<TEntityDto>(entity);
        }

        public async Task<IEnumerable<TEntityDto>> GetFilteredAsync(CrudFilterModel filterData, CancellationToken cancellationToken)
        {
            IEnumerable<TEntity> entities;
            if (filterData.ObjectsPerPage == 0 || filterData.PageNumber == 0)
            {
                entities = await ajkaShopDbContext.Set<TEntity>().AsQueryable()
                    .ToListAsync().ConfigureAwait(false);
            }
            else
            {
                entities = await ajkaShopDbContext.Set<TEntity>().AsQueryable()
                    .Skip(filterData.ObjectsPerPage * (filterData.PageNumber - 1))
                    .Take(filterData.ObjectsPerPage)
                    .ToListAsync().ConfigureAwait(false);
            }
            if(!string.IsNullOrWhiteSpace(filterData.OrderColumn))
            {
                Func<TEntity, Object> orderByFunc = sort => sort.GetType().GetProperty(filterData.OrderColumn)?.GetValue(sort);
                entities = filterData.IsDescendingOrder
                    ? entities.OrderByDescending(orderByFunc)
                    : entities.OrderBy(orderByFunc);
            }
            return entities != null ? _mapper.Map<IEnumerable<TEntityDto>>(entities) : null;
        }

        public async Task<bool> UpdateAsync(TEntityDto entityDto, CancellationToken cancellationToken)
        {
            await using (var context = ajkaShopDbContext)
            {
                await using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var entity = await context.Set<TEntity>()
                                                  .AsQueryable()
                                                  .SingleOrDefaultAsync(x => x.Id.Equals(entityDto.Id), cancellationToken);
                        if (entity == null)
                        {
                            throw new KeyNotFoundException(string.Format(AjkaExceptions.E0002, entityDto.Id));
                        }
                        _mapper.Map(entityDto, entity);
                        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                        await transaction.CommitAsync(cancellationToken);
                        return true;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        throw;
                    }
                }
            }
        }

        public async Task<int> CreateAsync(TEntityDto entityDto, CancellationToken cancellationToken)
        {
            if (entityDto?.Id > 0)
            {
                throw new AjkaBusinessException(string.Format(AjkaExceptions.E0001, entityDto.Id));
            }
            await using (var context = ajkaShopDbContext)
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var entity = _mapper.Map<TEntity>(entityDto);
                    context.Add(entity);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    await transaction.CommitAsync(cancellationToken);
                    return entity.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await using (var context = ajkaShopDbContext)
            {
                await using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var entity = await context.Set<TEntity>()
                                                  .AsQueryable()
                                                  .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
                        if (entity == null)
                        {
                            throw new KeyNotFoundException(string.Format(AjkaExceptions.E0002, id));
                        }
                        context.Remove(entity);
                        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                        await transaction.CommitAsync(cancellationToken);
                        return true;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        throw;
                    }
                }
            }
        }
    }
}
