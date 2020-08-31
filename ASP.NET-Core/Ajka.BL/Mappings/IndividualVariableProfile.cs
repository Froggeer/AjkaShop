using Ajka.BL.Models.Base;
using Ajka.DAL.Model;
using AutoMapper;

namespace Ajka.BL.Mappings
{
    public class IndividualVariableProfile : Profile
    {
        public IndividualVariableProfile()
        {
            CreateMap<IndividualVariable, IndividualVariableDto>();
            CreateMap<IndividualVariableDto, IndividualVariable>();
        }
    }
}
