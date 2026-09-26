using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Interfaces;
namespace Services
{

    public interface IMediaTypeService
    {
        IEnumerable<MediaType> GetMediaTypeByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<MediaType> GetAllMediaType();
        Response Create(MediaType entry);
        MediaType GetMediaTypeById(int id);
        Response Update(MediaType entry);
        Response Delete(int id);
    }
    public class MediaTypeService : IMediaTypeService
	{
		/// <summary>
		/// Declare resposity
		/// </summary>
		/// <param name="psqlConn"></param>
		/// <author>louis</author>	
		private readonly ICommonRepository _respository;

        public MediaTypeService(ICommonRepository respository)
        {
            _respository = respository;
        }
		
        /// <summary>
		/// Get all values
		/// </summary>
		/// <returns>List values</returns>
		/// <author>Louis</author>
        public IEnumerable<MediaType> GetAllMediaType()
        {
            return _respository.GetListByStore<MediaType>("[dbo].[Proc_SelectAll_MediaType]");
        }
        
        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public IEnumerable<MediaType> GetMediaTypeByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex =  pageIndex 
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<MediaType>("[dbo].[Proc_SelectPaged_MediaType]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
        
        /// <summary>
		/// Get values by Id
		/// </summary>
		/// <returns>value by Id</returns>
		/// <author>Louis</author>
        public MediaType GetMediaTypeById(int id)
        {
            var arg = new
            {
                MediaTypeId=id
            };
            return _respository.GetObjectByStoreV2<MediaType>("[dbo].[Proc_SelectByID_MediaType]", arg);
        }
        
        /// <summary>
		/// Insert a new row and return the identity
		/// </summary>
		/// <returns>new identity</returns>
		/// <author>Louis</author>
        public Response Create(MediaType entry)
        {
            var arg = new
            {
                Code = entry.Code,
                Name = entry.Name,
                Description = entry.Description,
                IsLocked = entry.IsLocked,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_MediaType]", arg);
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
        public Response Update(MediaType entry)
        {
            var arg = new
            {
                MediaTypeId = entry.MediaTypeId,
                Code = entry.Code,
                Name = entry.Name,
                Description = entry.Description,
                IsLocked = entry.IsLocked,
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_MediaType]", arg);
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
                MediaTypeId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_MediaType]", arg);
        }
	}
}

