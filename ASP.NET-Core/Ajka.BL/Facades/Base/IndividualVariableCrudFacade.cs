using Ajka.BL.Models.Base;
using Ajka.DAL;
using Ajka.DAL.Model;
using AutoMapper;

namespace Ajka.BL.Facades.Base
{
    public class IndividualVariableCrudFacade : RepositoryCrudFacade<IndividualVariableDto, IndividualVariable, int>, IEntityDtoFacade<IndividualVariableDto, int>
    {
        public IndividualVariableCrudFacade(AjkaShopDbContext ajkaShopDbContext, IMapper mapper) : base(ajkaShopDbContext, mapper)
        {
        }
    }
}
