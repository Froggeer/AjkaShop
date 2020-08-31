Before migration:

- Add DbSets to AjkShopDbContext and IAjkShopDbContext
- Build solution
- In Package Manager Console type "add-migration -p Ajka.DAL -s AjkaShop <migration name>"
- Then "Script-Migration -p Ajka.DAL -s AjkaShop -from <previous migration name>" and copy sql to folder SQL with migration name
- For local update DB run "Update-Database"

More on ...

https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx
https://docs.microsoft.com/cs-cz/ef/core/managing-schemas/migrations/?tabs=vs#generate-sql-scripts