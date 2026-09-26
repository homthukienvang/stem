using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Interfaces;
namespace Services
{

    public interface IDocumentLessonService
    {
        IEnumerable<DocumentLesson> GetDocumentLessonByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<DocumentLesson> GetAllDocumentLesson();
        Response Create(DocumentLesson entry);
        DocumentLesson GetDocumentLessonById(int id);
        Response Update(DocumentLesson entry);
        Response Delete(int id);
    }
    public class DocumentLessonService : IDocumentLessonService
	{
		/// <summary>
		/// Declare resposity
		/// </summary>
		/// <param name="psqlConn"></param>
		/// <author>louis</author>	
		private readonly ICommonRepository _respository;

        public DocumentLessonService(ICommonRepository respository)
        {
            _respository = respository;
        }
		
        /// <summary>
		/// Get all values
		/// </summary>
		/// <returns>List values</returns>
		/// <author>Louis</author>
        public IEnumerable<DocumentLesson> GetAllDocumentLesson()
        {
            return _respository.GetListByStore<DocumentLesson>("[dbo].[Proc_SelectAll_DocumentLesson]");
        }
        
        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public IEnumerable<DocumentLesson> GetDocumentLessonByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex =  pageIndex 
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<DocumentLesson>("[dbo].[Proc_SelectPaged_DocumentLesson]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
        
        /// <summary>
		/// Get values by Id
		/// </summary>
		/// <returns>value by Id</returns>
		/// <author>Louis</author>
        public DocumentLesson GetDocumentLessonById(int id)
        {
            var arg = new
            {
                DocumentLessonId=id
            };
            return _respository.GetObjectByStoreV2<DocumentLesson>("[dbo].[Proc_SelectByID_DocumentLesson]", arg);
        }
        
        /// <summary>
		/// Insert a new row and return the identity
		/// </summary>
		/// <returns>new identity</returns>
		/// <author>Louis</author>
        public Response Create(DocumentLesson entry)
        {
            var arg = new
            {
                LessonId = entry.LessonId,
                DocumentId = entry.DocumentId,
                UpdateUserId = entry.UpdateUserId,
                UpdateDate = entry.UpdateDate,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsDeleted = entry.IsDeleted,
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
        public Response Update(DocumentLesson entry)
        {
            var arg = new
            {
                DocumentLessonId = entry.DocumentLessonId,
                LessonId = entry.LessonId,
                DocumentId = entry.DocumentId,
                UpdateUserId = entry.UpdateUserId,
                UpdateDate = entry.UpdateDate,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsDeleted = entry.IsDeleted,
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_DocumentLesson]", arg);
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
                DocumentLessonId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_DocumentLesson]", arg);
        }
	}
}

