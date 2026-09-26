using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
namespace Services
{

    public interface IRoomService
    {
        IEnumerable<Room> GetRoomByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<Room> GetAllRoom();
        Response Create(Room entry);
        Room GetRoomById(int id);
        Response Update(Room entry);
        Response Delete(int id);
        IEnumerable<Room> GetRoomBySubjectId(int id);
        IEnumerable<Room> GetUpdateRooms(int version);
        IEnumerable<Room> GetUpdateRoomsV2(int version);
    }

    public class RoomService : IRoomService
	{
		/// <summary>
		/// Declare resposity
		/// </summary>
		/// <param name="psqlConn"></param>
		/// <author>louis</author>	
		private readonly ICommonRepository _respository;

        public RoomService(ICommonRepository respository)
        {
            _respository = respository;
        }
        public RoomService()
            : this(new CommonRepository(new Database()))
        {
        }
		
        /// <summary>
		/// Get all values
		/// </summary>
		/// <returns>List values</returns>
		/// <author>Louis</author>
        public IEnumerable<Room> GetAllRoom()
        {
            return _respository.GetListByStore<Room>("[dbo].[Proc_SelectAll_Room]");
        }
        
        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public IEnumerable<Room> GetRoomByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex =  pageIndex 
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<Room>("[dbo].[Proc_SelectPaged_Room]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
        
        /// <summary>
		/// Get values by Id
		/// </summary>
		/// <returns>value by Id</returns>
		/// <author>Louis</author>
        public Room GetRoomById(int id)
        {
            var arg = new
            {
                RoomId=id
            };
            return _respository.GetObjectByStoreV2<Room>("[dbo].[Proc_SelectByID_Room]", arg);
        }
        
        /// <summary>
		/// Insert a new row and return the identity
		/// </summary>
		/// <returns>new identity</returns>
		/// <author>Louis</author>
        public Response Create(Room entry)
        {
            var arg = new
            {
                SubjectId = entry.SubjectId,
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                Description = entry.Description,
                IsLocked = entry.IsLocked,
                UpdateUserId = entry.UpdateUserId,
                UpdateDate = entry.UpdateDate,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsDeleted = entry.IsDeleted,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_Room]", arg);
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
        public Response Update(Room entry)
        {
            var arg = new
            {
                RoomId = entry.RoomId,
                SubjectId = entry.SubjectId,
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                Description = entry.Description,
                IsLocked = entry.IsLocked
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_Room]", arg);
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
                RoomId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Room]", arg);
        }

        public IEnumerable<Room> GetRoomBySubjectId(int id)
        {
            return _respository.GetListBySqlQuery<Room>(string.Format("select roomId,code, name from Room where SubjectId = {0} and Isdeleted = 0 Order by code, name", id));
        }

        public IEnumerable<Room> GetUpdateRooms(int version)
        {
            var arg = new
            {
                Version = version,
            };
            return _respository.GetListByStoreV2<Room>("[dbo].[Proc_GetUpdateRooms]", arg);
        }
        public IEnumerable<Room> GetUpdateRoomsV2(int version)
        {
            var arg = new
            {
                Version = version,
            };
            return _respository.GetListByStoreV2<Room>("[dbo].[Proc_GetUpdateRoomsV2]", arg);
        }
	}
}

