using Ajka.BL.Exceptions;
using Ajka.Common.Constants.Base;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AjkaShop.Helpers
{
    public static class FilePropertiesCheck
    {
        public static string CheckFileProperties(IFormFile file)
        {
            if (file.Length <= 0)
            {
                throw new HttpResponseException
                {
                    Value = AjkaExceptions.E0004
                };
            }
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension.Equals(".gif"))
            {
                throw new HttpResponseException
                {
                    Value = AjkaExceptions.E0005
                };
            }
            return fileExtension;
        }
    }
}
