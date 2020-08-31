using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Facades.Base;
using Ajka.Common.Models.Base;
using Ajka.DAL.Model.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers.Base
{
    public abstract class CrudController<TEntityDto, TKey> : ControllerBase where TEntityDto : IEntity<TKey>
    {
        private readonly IEntityDtoFacade<TEntityDto, TKey> facade;

        protected CrudController(IEntityDtoFacade<TEntityDto, TKey> facade)
        {
            this.facade = facade;
        }

        /// <summary>
        /// Returns a record by primary ID.
        /// </summary>
        /// <param name="id">ID of record</param>
        [HttpGet, Route("{id}"), SwaggerOperation(nameof(Get))]
        public virtual async Task<TEntityDto> Get([FromRoute] TKey id)
        {
            return await facade.GetDetailAsync(id, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Return collection based on specified input.
        /// </summary>
        /// <param name="filterData">Collection of filter parameters. When ObjectsPerPage or PageNumber is 0,
        /// then all records are returned</param>
        [AllowAnonymous]
        [HttpPost, Route("filter"), SwaggerOperation(nameof(Filter))]
        public virtual async Task<IEnumerable<TEntityDto>> Filter([FromBody] CrudFilterModel filterData)
        {
            return await facade.GetFilteredAsync(filterData, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new record.
        /// </summary>
        /// <returns>ID of new record</returns>
        [HttpPost, SwaggerOperation(nameof(Create))]
        public virtual async Task<int> Create([FromBody] TEntityDto entity)
        {
            return await facade.CreateAsync(entity, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Update a record.
        /// </summary>
        /// <param name="entity">Entity with valid ID</param>
        /// <returns>true=success</returns>
        [HttpPut, SwaggerOperation(nameof(Update))]
        public virtual async Task<bool> Update([FromBody] TEntityDto entity)
        {
            return await facade.UpdateAsync(entity, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="id">ID of a record</param>
        /// <returns>true=success</returns>
        [HttpDelete, Route("{id}"), SwaggerOperation(nameof(Delete))]
        public virtual async Task<bool> Delete([FromRoute] TKey id)
        {
            return await facade.DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
