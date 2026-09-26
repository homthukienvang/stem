using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Interfaces;
namespace Services
{

    public interface IClientLessonService
    {
        IEnumerable<ClientLesson> GetClientLessonByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<ClientLesson> GetAllClientLesson();
        Response Create(ClientLesson entry);
        ClientLesson GetClientLessonById(int id);
        Response Update(ClientLesson entry);
        Response Delete(int id);
    }
    public class ClientLessonService : IClientLessonService
	{
		/// <summary>
		/// Declare resposity
		/// </summary>
		/// <param name="psqlConn"></param>
		/// <author>louis</author>	
		private readonly ICommonRepository _respository;

        public ClientLessonService(ICommonRepository respository)
        {
            _respository = respository;
        }
		
        /// <summary>
		/// Get all values
		/// </summary>
		/// <returns>List values</returns>
		/// <author>Louis</author>
        public IEnumerable<ClientLesson> GetAllClientLesson()
        {
            return _respository.GetListByStore<ClientLesson>("[dbo].[Proc_SelectAll_ClientLesson]");
        }
        
        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public IEnumerable<ClientLesson> GetClientLessonByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex =  pageIndex 
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<ClientLesson>("[dbo].[Proc_SelectPaged_ClientLesson]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
        
        /// <summary>
		/// Get values by Id
		/// </summary>
		/// <returns>value by Id</returns>
		/// <author>Louis</author>
        public ClientLesson GetClientLessonById(int id)
        {
            var arg = new
            {
                ClientLessonId=id
            };
            return _respository.GetObjectByStoreV2<ClientLesson>("[dbo].[Proc_SelectByID_ClientLesson]", arg);
        }
        
        /// <summary>
		/// Insert a new row and return the identity
		/// </summary>
		/// <returns>new identity</returns>
		/// <author>Louis</author>
        public Response Create(ClientLesson entry)
        {
            var arg = new
            {
                LessonId = entry.LessonId,
                ClientId = entry.ClientId,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsLocked = entry.IsLocked,
                IsDeleted = entry.IsDeleted,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_ClientLesson]", arg);
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
        public Response Update(ClientLesson entry)
        {
            var arg = new
            {
                ClientLessonId = entry.ClientLessonId,
                LessonId = entry.LessonId,
                ClientId = entry.ClientId,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsLocked = entry.IsLocked,
                IsDeleted = entry.IsDeleted,
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_ClientLesson]", arg);
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
                ClientLessonId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_ClientLesson]", arg);
        }
	}
}

