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
        List<Room> GetRoomByPage(int pageIndex, int pageSize, out int totalRow);
        List<Room> GetAllRoom();
        List<Room> GetAll();
        Response Create(Room entry);
        Room GetRoomById(int id);
        Response Update(Room entry);
        Response Delete(int id);
        List<Room> GetRoomBySubjectId(int id);
        List<Room> GetRoomByClient(int id);
        List<Room> GetRoomByLessonId(int id);
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
        public List<Room> GetAllRoom()
        {
            return _respository.GetListBySqlQuery<Room>("SELECT *	FROM [Room] WHERE IsLocked = 0 order by Name");
        }

        public List<Room> GetAll()
        {
            return _respository.GetListBySqlQuery<Room>("SELECT *	FROM [Room] ");
        }

        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public List<Room> GetRoomByPage(int pageIndex, int pageSize, out int totalRow)
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
            return _respository.GetObjectBySqlQueryV2<Room>("Select * from Room Where RoomId =  @RoomId", arg);
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
                entry.RoomId,
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
            var id = _respository.ExcuteSqlQuery(@"
                    INSERT INTO [Room] (
RoomId,
	                    [SubjectId],
	                    [Code],
	                    [Name],
	                    [Order],
	                    [NormalImage],
	                    [HoverImage],
	                    [SelectedImage],
	                    [Description],
	                    [IsLocked],
	                    [UpdateUserId],
	                    [UpdateDate],
	                    [CreatedUserId],
	                    [CreatedDate],
	                    [IsDeleted]
                    ) VALUES (
@RoomId,
	                    @SubjectId,
	                    @Code,
	                    @Name,
	                    @Order,
	                    @NormalImage,
	                    @HoverImage,
	                    @SelectedImage,
	                    @Description,
	                    @IsLocked,
	                    @UpdateUserId,
	                    GETDATE(),
	                    @CreatedUserId,
	                    GETDATE(),
	                    @IsDeleted
                    )", arg);
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
            var obj = _respository.ExcuteSqlQuery(@"UPDATE  [Room] SET
	[SubjectId] = @SubjectId,
	[Code] = @Code,
	[Name] = @Name,
	[Order] = @Order,
	[NormalImage] = @NormalImage,
	[HoverImage] = @HoverImage,
	[SelectedImage] = @SelectedImage,
	[Description] = @Description,
	[IsLocked] = @IsLocked 
WHERE
	[RoomId] = @RoomId", arg);
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
                RoomId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Room]", arg);
        }

        public List<Room> GetRoomBySubjectId(int id)
        {
            return _respository.GetListBySqlQuery<Room>(string.Format("select roomId,code, name from Room where SubjectId = {0} and Isdeleted = 0 Order by code, name", id));
        }

        public List<Room> GetRoomByLessonId(int id)
        {
            return _respository.GetListBySqlQuery<Room>(string.Format("select * from Room where Roomid in (select roomid from lesson where lessonid = {0})", id));
        }

        public List<Room> GetRoomByClient(int id)
        {
            return _respository.GetListBySqlQuery<Room>(string.Format("select * from Room where RoomId in (select Lesson.RoomId from ClientLesson inner join Lesson on ClientLesson.LessonId = Lesson.LessonId where ClientLesson.ClientId = {0} ) Order by code, name", id));
        }
        
	}
}

