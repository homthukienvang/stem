using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
namespace Services
{

    public interface ISubjectService
    {
        List<Subject> GetSubjectByPage(int pageIndex, int pageSize, out int totalRow);
        List<Subject> GetAllSubject();
        List<Subject> GetAll();
        Response Create(Subject entry);
        Subject GetSubjectById(int id);
        Subject GetSubjectByRoomId(int id);
        Response Update(Subject entry);
        Response Delete(int id);
    }
    public class SubjectService : ISubjectService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public SubjectService(ICommonRepository respository)
        {
            _respository = respository;
        }
        public SubjectService()
            : this(new CommonRepository(new Database()))
        {
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public List<Subject> GetAllSubject()
        {
            return _respository.GetListBySqlQuery<Subject>("Select * from Subject where IsLocked = 0 order by Name");
        }

        public List<Subject> GetAll()
        {
            return _respository.GetListBySqlQuery<Subject>("Select * from Subject");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public List<Subject> GetSubjectByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<Subject>("[dbo].[Proc_SelectPaged_Subject]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Subject GetSubjectById(int id)
        {
            var arg = new
            {
                SubjectId = id
            };
            return _respository.GetObjectBySqlQueryV2<Subject>("Select * from Subject Where SubjectId =  @SubjectId", arg);
        }

        public Subject GetSubjectByRoomId(int id)
        {
            var arg = new
            {
                LessonId = id
            };
            return _respository.GetObjectBySqlQueryV2<Subject>("select subject.* from room inner join subject on room.subjectid = subject.subjectid where room.roomid =  @LessonId", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Subject entry)
        {
            string sql = @"
                INSERT INTO  [Subject] (
                    [SubjectId],
	                [Code],
	                [Name],
	                [Order],
	                [Description],
	                [NormalImage],
	                [HoverImage],
	                [SelectedImage],
	                [IsLocked],
	                [CreatedDate],
	                [IsDeleted]
                ) VALUES (
                    @SubjectId,
	                @Code,
	                @Name,
	                @Order,
	                @Description,
	                @NormalImage,
	                @HoverImage,
	                @SelectedImage,
	                @IsLocked,
	                GETDATE(),
	                @IsDeleted
                )";
            var arg = new
            {entry.SubjectId,
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                Description = entry.Description,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                IsLocked = entry.IsLocked,
                CreatedDate = entry.CreatedDate,
                IsDeleted = entry.IsDeleted,
            };
            var id = _respository.ExcuteSqlQuery(sql, arg);
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
        public Response Update(Subject entry)
        {
            var arg = new
            {
                SubjectId = entry.SubjectId,
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                Description = entry.Description,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                IsLocked = entry.IsLocked,
                IsDeleted = entry.IsDeleted,
            };
            var obj = _respository.ExcuteSqlQuery(@"
                UPDATE  [Subject] SET
	                [Code] = @Code,
	                [Name] = @Name,
	                [Order] = @Order,
	                [Description] = @Description,
	                [NormalImage] = @NormalImage,
	                [HoverImage] = @HoverImage,
	                [SelectedImage] = @SelectedImage,
	                [IsLocked] = @IsLocked
                WHERE
	                [SubjectId] = @SubjectId", arg);
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
                SubjectId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Subject]", arg);
        }
    }
}

                                                            