using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Collections.Generic;
namespace Services
{

    public interface IDownloadDocumentService
    {
        List<DownloadDocument> GetAllExpired(int ttlDays);
        Response Create(DownloadDocument entry);
        Response Delete(int id);
        bool CheckAndCreateTable();
    }
    public class DownloadDocumentService : IDownloadDocumentService
    {
        private readonly ICommonRepository _respository;

        public DownloadDocumentService(ICommonRepository respository)
        {
            _respository = respository;
        }
        public DownloadDocumentService()
            : this(new CommonRepository(new Database()))
        {
        }

        public List<DownloadDocument> GetAllExpired(int ttlDays)
        {
            return _respository.GetListBySqlQuery<DownloadDocument>("Select * from DownloadDocument where DATEADD(day," + ttlDays + ", CreateDate)<GETDATE()");
        }

        public Response Create(DownloadDocument entry)
        {
            string sql = @"
                INSERT INTO  [DownloadDocument] (
	                [DocumentId],
	                [LessonId],
	                [FileName],
	                [Title],
	                [CreateDate]) 
                VALUES (
	                @DocumentId,
	                @LessonId,
	                @FileName,
	                @Title,
	                GETDATE()
                )";
            var arg = new
            {
                entry.DocumentId,
                entry.LessonId,
                entry.FileName,
                entry.Title
            };
            var id = _respository.ExcuteSqlQuery(sql, arg);
            return new Response()
            {
                Success = id,
                Message = ""
            };
        }

        public Response Delete(int id)
        {
            return _respository.ExcuteSql("DELETE FROM DownloadDocument WHERE Id=" + id);
        }

        public bool CheckAndCreateTable()
        {
            var result = _respository.GetListBySqlQuery<InfoSchema>("SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DownloadDocument'");
            if (result == null || result.Count <= 0)
            {
                _respository.ExcuteSql("CREATE TABLE [DownloadDocument] ([Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,[DocumentId] int,  [LessonId] int, [FileName] NVARCHAR(1000), [Title] NVARCHAR(1000), [CreateDate] datetime)");
                return true;
            }
            return false;
        }
    }
}

