Create a CRUD facade:

- Create Dto and mapper for Dto and entity
- In controller, change inherited class "ControllerBase" for CrudController<some Dto, int>
- Also, in controller add to constructor "IEntityDtoFacade< "some Dto", int> facade" (example IEntityDtoFacade<UserDto, int> facade)
- Create CRUD Facade (example - UserCrudFacade)
- In Ajka.BL class Bootstrapper add facade to DI (example - services.AddTransient<IEntityDtoFacade<UserDto, int>, UserCrudFacade>())
- Enjoy ...
