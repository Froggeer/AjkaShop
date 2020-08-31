using System;
using System.Collections;
using System.Text;

namespace Ajka.Common.Helpers
{
    public static class ExceptionToStringHelper
    {
        private static StringBuilder builder;

        public static string Transform(Exception exception)
        {
            builder = new StringBuilder();
            WriteExceptionDetails(exception, 0);
            return builder.ToString();
        }

        private static void WriteExceptionDetails(Exception exception, int level)
        {
            var indent = new string(' ', level);

            if (level > 0)
            {
                builder.AppendLine($"{indent}=== INNER EXCEPTION ===");
            }

            void Append(string prop)
            {
                var propertyValue = exception.GetType().GetProperty(prop)?.GetValue(exception);

                if (propertyValue != null)
                {
                    builder.AppendFormat("{0}{1}: {2}{3}", indent, prop, propertyValue, Environment.NewLine);
                }
            }

            Append("Message");
            Append("HResult");
            Append("HelpLink");
            Append("Source");
            Append("StackTrace");
            Append("TargetSite");

            foreach (DictionaryEntry dataException in exception.Data)
            {
                builder.AppendFormat("{0} {1} = {2}{3}", indent, dataException.Key, dataException.Value, Environment.NewLine);
            }

            if (exception.InnerException != null && level < 10)
            {
                WriteExceptionDetails(exception.InnerException, ++level);
            }
        }
    }
}
