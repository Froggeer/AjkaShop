using System;
using System.Threading.Tasks;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.Common.Constants.Base;
using Ajka.Common.Helpers;
using Ajka.DAL;
using Ajka.DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ajka.BL.Services.Base
{
    public class ErrorLogService : IErrorLogService, IAjkaShopService
    {
        private readonly IConfiguration _configuration;

        public ErrorLogService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task LogExceptionAsync(Exception exception)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var errorRecord = new ErrorLog
                    {
                        CreateDate = DateTime.Now,
                        Message = exception.Message,
                        FullDescription = ExceptionToStringHelper.Transform(exception)
                    };
                    var optionsBuilder = new DbContextOptionsBuilder<AjkaShopDbContext>();
                    optionsBuilder.UseSqlServer(_configuration.GetConnectionString(BaseConstants.DbDefaultConnectionIdentifier));
                    await using var context = new AjkaShopDbContext(optionsBuilder.Options);
                    context.Add(errorRecord);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }).ConfigureAwait(false);
        }
    }
}
