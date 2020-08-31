using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ajka.BL.Services.Base.Interfaces
{
    public interface IErrorLogService
    {
        Task LogExceptionAsync(Exception exception);
    }
}
