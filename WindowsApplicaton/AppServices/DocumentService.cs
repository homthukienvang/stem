using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
namespace Services
{

    public interface IDocumentService
    {
        List<Document> GetDocumentByPage(int pageIndex, int pageSize, out int totalRow);
        List<Document> GetAll();
        List<Document> GetAllDocument();
        Response Create(Document entry);
        Document GetDocumentById(int id);
        Response Update(Document entry);
        Response Delete(int id);
        Response DeleteByLeson(int id);
        List<Document> GetAllDocumentByLesson(int lessonId);
        void ResetDownloadStatus();
    }
    public class DocumentService : IDocumentService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public DocumentService(ICommonRepository respository)
        {
            _respository = respository;
        }


        public DocumentService()
            : this(new CommonRepository(new Database()))
        {
        }

        public List<Document> GetAll()
        {
            return _respository.GetListBySqlQuery<Document>("SELECT * FROM [Document]");
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public List<Document> GetAllDocument()
        {
            return _respository.GetListBySqlQuery<Document>("select * from document");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public List<Document> GetDocumentByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<Document>("[dbo].[Proc_SelectPaged_Document]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Document GetDocumentById(int id)
        {
            var arg = new
            {
                DocumentId = id
            };
            return _respository.GetObjectBySqlQueryV2<Document>("Select * from Document where DocumentId = @DocumentId", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Document entry)
        {
            var arg = new
            {
                entry.DocumentId,
                MediaTypeId = 1,
                Description = entry.Description,
                Order = entry.Order,
                NormalImage = entry.NormalImage,
                IsFree = entry.IsFree,
                Price = entry.Price,
                FileName = entry.FileName,
                DocumentName = entry.DocumentName,
                ImageUrl = entry.ImageUrl,
                IsDeleted = entry.IsDeleted,
                entry.LessonId
            };
            var id = _respository.ExcuteSqlQuery(@"
                    INSERT INTO  [Document] ( 
                    DocumentId,
                    [Order], 
                    [FileName],
                    [DocumentName] ,
                    IsDeleted,
                    LessonId,
                    IsDownloaded
                    ) VALUES ( 
                    @DocumentId,
                    @Order, 
                    @FileName,
                    @DocumentName, 
                    0,
                    @LessonId,
                    0
                    ) ", arg);
            return new Response()
            {
                Success = id,
                Message = ""
            };
        }

        /// <summary>
        /// Update the exist row
        /// </summary>
        /// <author>Louis</author>
        public Response Update(Document entry)
        {
            var arg = new
            {
                DocumentId = entry.DocumentId,
                Order = entry.Order,
                FileName = entry.FileName,
                DocumentName = entry.DocumentName,
                IsDownloaded = entry.IsDownloaded
            };
            var obj = _respository.ExcuteSqlQuery(@"UPDATE [Document] SET 
                [Order] = @Order, 
                [FileName] = @FileName,
                [DocumentName] = @DocumentName,
                [IsDownloaded] = @IsDownloaded
                WHERE
                [DocumentId] = @DocumentId", arg);
            return new Response
            {
                Success = obj
            };
        }

        /// <summary>
        /// Delete the exist row
        /// </summary>
        /// <author>Louis</author>
        public Response Delete(int id)
        {
            var arg = new
            {
                DocumentId = id,
            };
            var obj = _respository.ExcuteSqlQuery(@"DELETE [Document] WHERE [DocumentId] = @DocumentId", arg);
            return new Response
            {
                Success = obj
            };
        }

        public Response DeleteByLeson(int id)
        {
            var arg = new
            {
                LessonId = id,
            };
            var obj = _respository.ExcuteSqlQuery(@"DELETE [Document] WHERE [LessonId] = @LessonId", arg);
            return new Response
            {
                Success = obj
            };
        }

        public List<Document> GetAllDocumentByLesson(int lessonId)
        {
            string sql = @"SELECT d.*
	                        FROM [Document] d 
	                        WHERE  d.LessonId = @LessonId";
            return _respository.GetListBySqlQueryV2<Document>(sql, new
            {
                LessonId = lessonId,
            });
        }

        public void ResetDownloadStatus()
        {
            _respository.ExcuteSql("Update [Document] set IsDownloaded = 0");
        }
    }
}

