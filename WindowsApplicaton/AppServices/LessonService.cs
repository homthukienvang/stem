using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
namespace Services
{

    public interface ILessonService
    {
        List<Lesson> GetLessonByPage(int pageIndex, int pageSize, out int totalRow);
        List<Lesson> GetAllLesson();
        List<Lesson> GetAll();
        Response Create(Lesson entry);
        Lesson GetLessonById(int id);
        Response Update(Lesson entry);
        Response Delete(int id);
        void UpdateDowloadStatus(Lesson currenLesson);
        List<Lesson> GetLessonByClient();
        List<Lesson> GetLessonByRoom(int roomId, string title, int order);
        void UpdateClientLesson(List<ClientLesson> clientLessons, int clientId);
        void ResetDownloadStatus();
    }
    public class LessonService : ILessonService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public LessonService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public LessonService()
            : this(new CommonRepository(new Database()))
        {
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public List<Lesson> GetAllLesson()
        {
            return _respository.GetListBySqlQuery<Lesson>("SELECT * FROM [Lesson] where IsLocked = 0 order by Name");
        }

        public List<Lesson> GetAll()
        {
            return _respository.GetListBySqlQuery<Lesson>("SELECT * FROM [Lesson]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public List<Lesson> GetLessonByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<Lesson>("[dbo].[Proc_SelectPaged_Lesson]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Lesson GetLessonById(int id)
        {
            var arg = new
            {
                LessonId = id
            };
            return _respository.GetObjectBySqlQueryV2<Lesson>("SELECT * FROM [Lesson] where LessonId =@LessonId ", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Lesson entry)
        {
            var arg = new
            {
                entry.LessonId,
                RoomId = entry.RoomId,
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                IsFree = entry.IsFree,
                Price = entry.Price,
                Description = entry.Description,
                Status = entry.Status,
                UpdateUserId = entry.UpdateUserId,
                UpdateDate = entry.UpdateDate,
                CreatedUserId = entry.CreatedUserId,
                CreatedDate = entry.CreatedDate,
                IsLocked = entry.IsLocked,
                IsDeleted = entry.IsDeleted,
                entry.GuideFile,
                entry.Reversion,
                entry.NeedUpdate
            };
            var id = _respository.ExcuteSqlQuery(@"
INSERT INTO  [Lesson] (
LessonId,
	[RoomId],
	[Code],
	[Name],
	[Order],
	[NormalImage],
	[HoverImage],
	[SelectedImage],
	[IsFree],
	[Price],
	[Description],
	[Status],
	[UpdateUserId],
	[UpdateDate],
	[CreatedUserId],
	[CreatedDate],
	[IsLocked],
	[IsDeleted],
	GuideFile,
IsDownloaded,
Reversion,
NeedUpdate
) VALUES (
@LessonId,
	@RoomId,
	@Code,
	@Name,
	@Order,
	@NormalImage,
	@HoverImage,
	@SelectedImage,
	@IsFree,
	@Price,
	@Description,
	@Status,
	@UpdateUserId,
	@UpdateDate,
	@CreatedUserId,
	@CreatedDate,
	@IsLocked,
	@IsDeleted,
	@GuideFile,
    0,
@Reversion,
@NeedUpdate
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
        public Response Update(Lesson entry)
        {
            var arg = new
            {
                LessonId = entry.LessonId,
                RoomId = entry.RoomId,
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                IsFree = entry.IsFree,
                Price = entry.Price,
                Description = entry.Description,
                IsLocked = entry.IsLocked,
                entry.GuideFile,
                entry.IsDownloaded,
                entry.Reversion,
                entry.NeedUpdate
            };
            var obj = _respository.ExcuteSqlQuery(@"
                UPDATE [Lesson] SET
                [RoomId] = @RoomId,
                [Code] = @Code,
                [Name] = @Name,
                [Order] = @Order,
                [NormalImage] = @NormalImage,
                [HoverImage] = @HoverImage,
                [SelectedImage] = @SelectedImage,
                [IsFree] = @IsFree,
                [Price] = @Price,
                [Description] = @Description, 
                [IsLocked] = @IsLocked,
                GuideFile=@GuideFile,
                IsDownloaded =@IsDownloaded,
Reversion=@Reversion,
NeedUpdate=@NeedUpdate
                WHERE
	            [LessonId] = @LessonId", arg);
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
            string sql = string.Format(@"DELETE FROM ClientLesson WHERE   LessonId = {0}
                                       ", id);
            _respository.ExcuteSql(sql);
            sql = string.Format(@"DELETE FROM [Lesson] WHERE   LessonId = {0}
                                       ", id);
            return _respository.ExcuteSql(sql);
        }

        public void UpdateDowloadStatus(Lesson currenLesson)
        {
            _respository.ExcuteSql("Update Lesson set IsDownloaded = 1,IsDeleted =1 where LessonId = " + currenLesson.LessonId);
        }

        public List<Lesson> GetLessonByClient()
        {
            var client = _respository.GetObjectBySqlQuery<Client>("Select * from Client");
            string sql = "";
            if (client.FullLesson)
            {
                sql = "SELECT * FROM [Lesson] where IsLocked = 0 order by Name";
            }
            else
            {
                sql = @"SELECT l.* FROM Lesson l inner join ClientLesson cl on l.LessonId = cl.LessonId
 where l.IsLocked = 0 and cl.ClientId = " + client.ClientId + " order by Name ";
            }
            return _respository.GetListBySqlQuery<Lesson>(sql);
        }

        public void UpdateClientLesson(List<ClientLesson> clientLessons, int clientId)
        {
            string sql = string.Format(@"DELETE ClientLesson WHERE ClientId = {0}
                                       ", clientId);
            _respository.ExcuteSql(sql);
            foreach (var clientLesson in clientLessons)
            {
                sql = string.Format(@"INSERT ClientLesson
                        ( LessonId ,
                          ClientId ,
                          CreatedUserId ,
                          CreatedDate ,
                          IsLocked ,
                          IsDeleted
                        )
                VALUES  ( {0} ,
                          {1} ,
                          0,
                          GETDATE() ,
                          0,
                          0
                        )
            ", clientLesson.LessonId, clientId);
                _respository.ExcuteSql(sql);
            }
        }

        public List<Lesson> GetLessonByRoom(int roomId, string title, int order)
        {
            string sql = String.Format("select Lesson.* from Lesson inner join ClientLesson on Lesson.LessonId = ClientLesson.LessonId where Lesson.IsLocked = 0 and Lesson.RoomId = {0} and Lesson.Name like N'%{1}%' order by Code " + (order == 0 ? "desc" : "asc") + ",Lesson.name", roomId, title);
            return _respository.GetListBySqlQuery<Lesson>(sql);
        }

        public void ResetDownloadStatus()
        {
            _respository.ExcuteSql("Update Lesson set IsDownloaded = 0,IsDeleted =0");
        }
    }
}

