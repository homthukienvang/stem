using System;
using System.Reflection;
using System.Web;
using log4net;
using Repositories.Implementations;

namespace Security
{
    public static class Logger
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogError(Exception e, string pageName)
        {
            _log.Error(pageName, e);
            var sql = @"INSERT INTO [dbo].[Log]
           ( [ErrorMessage]
           ,[ErrorDateTime]
           ,[ObjectError]
           ,[InnerException]
           ,[ErrorStackTrace]
           ,[ErrorCode])
     VALUES
           ( @ErrorMessage
           ,GetDate()
           ,@ObjectError
           ,@InnerException
           ,@ErrorStackTrace
           ,@ErrorCode)";
            try
            {
                var common = new CommonRepository(new Database());
                common.ExcuteSqlQuery(sql,
                    new
                    {
                        ErrorMessage = e.Message,
                        ObjectError = pageName,
                        InnerException = e.InnerException != null ? e.InnerException.ToString() : "",
                        ErrorStackTrace = e.StackTrace,
                        ErrorCode = "ErrorCode"
                    });
            }
            catch (Exception ex)
            {
            }
        }
    }
}