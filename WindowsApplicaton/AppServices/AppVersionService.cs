using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Interfaces;
namespace Services
{

    public interface IAppVersionService
    {
        IEnumerable<AppVersion> GetAppVersionByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<AppVersion> GetAllAppVersion();
        Response Create(AppVersion entry);
        AppVersion GetAppVersionById(int id);
        Response Update(AppVersion entry);
        Response Delete(int id);
    }
    public class AppVersionService : IAppVersionService
	{
		/// <summary>
		/// Declare resposity
		/// </summary>
		/// <param name="psqlConn"></param>
		/// <author>louis</author>	
		private readonly ICommonRepository _respository;

        public AppVersionService(ICommonRepository respository)
        {
            _respository = respository;
        }
		
        /// <summary>
		/// Get all values
		/// </summary>
		/// <returns>List values</returns>
		/// <author>Louis</author>
        public IEnumerable<AppVersion> GetAllAppVersion()
        {
            return _respository.GetListByStore<AppVersion>("[dbo].[Proc_SelectAll_AppVersion]");
        }
        
        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public IEnumerable<AppVersion> GetAppVersionByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex =  pageIndex 
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<AppVersion>("[dbo].[Proc_SelectPaged_AppVersion]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
        
        /// <summary>
		/// Get values by Id
		/// </summary>
		/// <returns>value by Id</returns>
		/// <author>Louis</author>
        public AppVersion GetAppVersionById(int id)
        {
            var arg = new
            {
                AppVersionId=id
            };
            return _respository.GetObjectByStoreV2<AppVersion>("[dbo].[Proc_SelectByID_AppVersion]", arg);
        }
        
        /// <summary>
		/// Insert a new row and return the identity
		/// </summary>
		/// <returns>new identity</returns>
		/// <author>Louis</author>
        public Response Create(AppVersion entry)
        {
            var arg = new
            {
                Code = entry.Code,
                PackageName = entry.PackageName,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsLocked = entry.IsLocked,
                IsDeleted = entry.IsDeleted,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_AppVersion]", arg);
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
        public Response Update(AppVersion entry)
        {
            var arg = new
            {
                AppVersionId = entry.AppVersionId,
                Code = entry.Code,
                PackageName = entry.PackageName,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsLocked = entry.IsLocked,
                IsDeleted = entry.IsDeleted,
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_AppVersion]", arg);
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
                AppVersionId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_AppVersion]", arg);
        }
	}
}

