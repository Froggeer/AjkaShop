using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Facades.Base;
using Ajka.BL.Models.User;
using Ajka.BL.Queries.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.Common.Helpers;
using Ajka.DAL.Model.Interfaces;
using AjkaShop.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace AjkaShop.Controllers
{
    /// <summary>
    /// Managing users.
    /// </summary>
    [Authorize(Roles = RoleConstants.AdministratorRole)]
    [ApiController]
    [Route("[controller]")]
    [ControllerName("users")]
    public class UserController : CrudController<UserDto, int>
    {
        private readonly IAjkaShopDbContext _ajkaShopDbContext;
        private readonly IUserQueries _userQueries;
        private readonly AppSettings _appSettings;

        public UserController(IEntityDtoFacade<UserDto, int> facade,
            IAjkaShopDbContext ajkaShopDbContext,
            IUserQueries userQueries,
            IOptions<AppSettings> appSettings) : base(facade)
        {
            _ajkaShopDbContext = ajkaShopDbContext;
            _userQueries = userQueries;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Creates a new record.
        /// </summary>
        /// <returns>Id of new record</returns>
        public override async Task<int> Create([FromBody] UserDto entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                entity.Password = PasswordSecurityHelper.HashPassword(entity.Password, _appSettings.PasswordSalt);
            }
            return await base.Create(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Update a record.
        /// </summary>
        /// <param name="entity">Entity with valid Id</param>
        /// <returns>true=success</returns>
        public override async Task<bool> Update([FromBody] UserDto entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                entity.Password = PasswordSecurityHelper.HashPassword(entity.Password, _appSettings.PasswordSalt);
            }
            else
            {
                var previousEntity = await Get(entity.Id).ConfigureAwait(false);
                if (previousEntity != null)
                {
                    entity.Password = previousEntity.Password;
                }
            }
            return await base.Update(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns only valid users record.
        /// </summary>
        [HttpGet, Route("valid"), SwaggerOperation(nameof(GetUsersValid))]
        public async Task<IEnumerable<UserDto>> GetUsersValid(CancellationToken cancellationToken = default)
        {
            return await _userQueries.GetUsersValidAsync(_ajkaShopDbContext, cancellationToken).ConfigureAwait(false);
        }
    }
}
