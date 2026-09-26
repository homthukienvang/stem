using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;

namespace Services
{
    public interface IDocumentService
    {
        IEnumerable<Document> GetDocumentByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<Document> GetAllDocument();
        Response Create(Document entry);
        Document GetDocumentById(int id);
        Response Update(Document entry);
        Response Delete(int id);
        Response UpdateListDocument(IEnumerable<Document> documents, int lessionId);
        IEnumerable<Document> GetDocumentByLesson(int id);
        IEnumerable<Document> GetUpdateDocumentsV2(int clientId, int version);
        IEnumerable<Document> GetUpdateDocuments(int version);
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

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<Document> GetAllDocument()
        {
            return _respository.GetListByStore<Document>("[dbo].[Proc_SelectAll_Document]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<Document> GetDocumentByPage(int pageIndex, int pageSize, out int totalRow)
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
            return _respository.GetObjectByStoreV2<Document>("[dbo].[Proc_SelectByID_Document]", arg);
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
                MediaTypeId = 1, //entry.MediaTypeId,
                entry.Description,
                entry.Order,
                entry.NormalImage,
                entry.IsFree,
                entry.Price,
                entry.FileName,
                entry.DocumentName,
                entry.ImageUrl,
                IsDeleted = false,
                entry.LessonId
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_Document]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                ReturnId = Convert.ToInt32(id),
                Message = ""
            };
        }

        public Response CreateDocumentLesson(DocumentLesson entry)
        {
            var arg = new
            {
                entry.LessonId,
                entry.DocumentId,
                entry.UpdateUserId,
                UpdateDate = DateTime.Now,
                entry.CreatedUserId,
                CreatedDate = DateTime.Now,
                entry.IsDeleted,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_DocumentLesson]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
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
                entry.DocumentId,
                MediaTypeId = 1, //entry.MediaTypeId,
                entry.Description,
                entry.Order,
                entry.NormalImage,
                entry.IsFree,
                entry.Price,
                entry.FileName,
                entry.DocumentName,
                entry.ImageUrl,
                IsDeleted = false,
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_Document]", arg);
            return new Response
            {
                Success = obj.Success,
                Message = obj.Message
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
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Document]", arg);
        }

        public Response UpdateListDocument(IEnumerable<Document> documents, int lessionId)
        {
            _respository.WithTransaction(() =>
            {
                var sql = " Delete [dbo].[Document] where LessonId = " + lessionId;
                _respository.ExcuteSql(sql);
                int i = 0;
                foreach (var document in documents)
                {
                    i++;
                    document.Order = i;
                    document.LessonId = lessionId;
                    //if (document.DocumentId == 0)
                    {
                        var resturn = Create(document);
                    }
                    //else
                    //{
                    //    Update(document);
                    //}
                }

                return new Response() {Success = true};
            });
            return new Response() {Success = true};
        }

        public IEnumerable<Document> GetDocumentByLesson(int id)
        {
            return
                _respository.GetListBySqlQuery<Document>(@"SELECT  d.* FROM  [dbo].[Document] d Where d.LessonId = " +
                                                         id);
        }

        public IEnumerable<Document> GetUpdateDocuments(int version)
        {
            var arg = new
            {
                Version = version,
            };
            return _respository.GetListByStoreV2<Document>("[dbo].[Proc_GetUpdateDocuments]", arg);
        }
        public IEnumerable<Document> GetUpdateDocumentsV2(int clientId,int version)
        {
            var arg = new
            {
                ClientId= clientId,
                Version = version,
            };
            return _respository.GetListByStoreV2<Document>("[dbo].[Proc_GetUpdateDocumentsV2]", arg);
        }
    }
}