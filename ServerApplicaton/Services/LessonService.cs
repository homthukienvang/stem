using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;

namespace Services
{
    public interface ILessonService
    {
        IEnumerable<Lesson> GetLessonByPage(int pageIndex,int subjectId, int roomId, string keyword, int pageSize, out int totalRow);
        IEnumerable<Lesson> GetAllLesson();
        Response Create(Lesson entry);
        Lesson GetLessonById(int id);
        Response Update(Lesson entry);
        Response Delete(int id);
        IEnumerable<Lesson> GetUpdateLessons(int version);
        IEnumerable<Lesson> GetUpdateLessonsV2(int clientId,int version);
        Response UpdateGuideFile(Lesson model);
        int GetLastVersion();
        Response Lock(int id, bool islock);
        Response DeleteLessons(Lesson model);
        Response LockLessons(Lesson model);
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
        public IEnumerable<Lesson> GetAllLesson()
        {
            return _respository.GetListByStore<Lesson>("[dbo].[Proc_SelectAll_Lesson]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<Lesson> GetLessonByPage(int pageIndex, int subjectId, int roomId, string keyword, int pageSize, out int totalRow)
        {
            var arg = new
            {
                SubjectId = subjectId,
                RoomId = roomId,
                Keyword = keyword,
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
            return _respository.GetObjectByStoreV2<Lesson>("[dbo].[Proc_SelectByID_Lesson]", arg);
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
                entry.RoomId,
                entry.Code,
                entry.Name,
                entry.Order,
                entry.NormalImage,
                entry.HoverImage,
                entry.SelectedImage,
                entry.IsFree,
                entry.Price,
                entry.Description,
                entry.Status,
                entry.UpdateUserId,
                entry.UpdateDate,
                entry.CreatedUserId,
                entry.CreatedDate,
                entry.IsLocked,
                entry.IsDeleted,
                entry.GuideFile
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_Lesson]", arg);
            return new Response
            {
                Success = Convert.ToInt32(id) > 0,
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
                entry.LessonId,
                entry.RoomId,
                entry.Code,
                entry.Name,
                entry.Order,
                entry.NormalImage,
                entry.HoverImage,
                entry.SelectedImage,
                entry.IsFree,
                entry.Price,
                entry.Description,
                entry.IsLocked,
                entry.GuideFile
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_Lesson]", arg);
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
                LessonId = id
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Lesson]", arg);
        }

        public IEnumerable<Lesson> GetUpdateLessons(int version)
        {
            var arg = new
            {
                Version = version
            };
            return _respository.GetListByStoreV2<Lesson>("[dbo].[Proc_GetUpdateLessons]", arg);
        }
        public IEnumerable<Lesson> GetUpdateLessonsV2(int clientId, int version)
        {
            var arg = new
            {
                ClientId= clientId,
                Version = version
            };
            return _respository.GetListByStoreV2<Lesson>("[dbo].[Proc_GetUpdateLessonsV2]", arg);
        }

        public Response UpdateGuideFile(Lesson model)
        {
             _respository.ExcuteSqlQuery(@"UPDATE  dbo.Lesson SET Reversion = ISNULL(Reversion,0)  + 1  WHERE LessonId = @LessonId", new
            {
                model.LessonId
            });
            var result = _respository.ExcuteSqlQuery(@"UPDATE [dbo].[Lesson] SET 
	        GuideFile=@GuideFile
        WHERE
	        [LessonId] = @LessonId", new
            {
                model.GuideFile,
                model.LessonId
            });
            return new Response {Success = result};
        }

        public int GetLastVersion()
        {
            //return Convert.ToInt32(_respository.ExcuteSqlQueryGetValueV2("select Cast( CHANGE_TRACKING_CURRENT_VERSION()as int ) Version",new{}));
            return Convert.ToInt32(_respository.ExcuteSqlQueryGetValueV2("select Cast( ISNULL(CHANGE_TRACKING_CURRENT_VERSION(),0) as int ) Version", new { }));            
        }

        public Response Lock(int id, bool islock)
        {
            var result = _respository.ExcuteSqlQuery(@"UPDATE [dbo].[Lesson] SET 
	        IsLocked=@islock
        WHERE
	        [LessonId] = @id", new
            {
                islock,
                id
            });
            return new Response { Success = result };
        }

        public Response DeleteLessons(Lesson model)
        {
           _respository.WithTransaction(() =>
            {
                foreach (var id in model.ListId)
                {
                    var arg = new
                    {
                        LessonId = id
                    };
                    _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Lesson]", arg);
                }
                return new Response();
            });
            return new Response()
            {
                Success = true,
                Message = ""
            };
        }

        public Response LockLessons(Lesson model)
        {
            string sql = string.Format(@"UPDATE [dbo].[Lesson] SET IsLocked ={1}
                                        WHERE  LessonId in ({0})
                                       ", string.Join(",", model.ListId), model.IsLocked == true ? "1" : "0");
            _respository.WithTransaction(() =>
            {
                var id = _respository.ExcuteSql(sql);
                return new Response();
            });
            return new Response()
            {
                Success = true,
                Message = ""
            };
        }
    }
}