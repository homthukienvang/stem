using Model;
using Model.Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Collections.Generic;
namespace Services
{

    public interface ICommentService
    {
        Comment GetByLessonId(int lessonId);
        Comment GetById(int id);
        Response Create(Comment entry);
        Response CreateFB(Comment entry);
        Response CreateFBA(CommentAnswer entry);
        void UpdateFBAStatus(string caid, string status);
        Response Update(Comment entry);
        Response Delete(int id);
        Response SendComment(Comment entry);
        IEnumerable<Message> GetAllMessages();

        void UpdateTableComment();
        void CreateTableCA();
        bool CheckTableCA();
    }
    public class CommentService : ICommentService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public CommentService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public CommentService()
            : this(new CommonRepository(new Database()))
        {
        }

        public IEnumerable<Message> GetAllMessages()
        {
            string SQL = "";
            SQL = "select CM.CreatedDate, convert(nvarchar, CM.CreatedDate,102) as ShortCreatedDate, substring(CM.Content,0,100) as ShortConent, CM.Content,CM.Status, CM.FeedbackId,CA.UserName,CA.Answer,CA.AnswerTime,CA.Status as CAStatus, CA.Id as CAId" +
                    " from Comment CM left join CommentAnswer CA on CM.FeedbackId = CA.FeedbackId" +
                    " WHERE CM.FeedbackId > 0" +
                    " ORDER BY CA.AnswerTime desc, CM.CreatedDate desc";
            return _respository.GetListBySqlQuery<Message>(SQL);
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Comment GetByLessonId(int id)
        {
            var arg = new
            {
                LessonId = id
            };
            return _respository.GetObjectBySqlQueryV2<Comment>("SELECT * FROM [Comment] where LessonId =@LessonId ", arg);
        }

        public Comment GetById(int id)
        {
            var arg = new
            {
                CommentId = id
            };
            return _respository.GetObjectBySqlQueryV2<Comment>("SELECT * FROM [Comment] where CommentId =@CommentId ", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Comment entry)
        {
            var arg = new
            {
                ClientId = entry.ClientId
                ,
                CommentType = entry.CommentType
                ,
                Content = entry.Content
                ,
                LessonId = entry.LessonId
            };
            var id = _respository.ExcuteSqlQuery(@"
            INSERT INTO  [Comment] (
	            [ClientId]
	            ,[CommentType]
	            ,[Content]
	            ,[LessonId]
	            ,[CreatedDate]
            ) VALUES (
	            @ClientId
	            ,@CommentType
	            ,@Content
	            ,@LessonId
	            ,getdate()
            ) ", arg);
            return new Response()
            {
                Success = id,
                Message = ""
            };
        }

        public Response CreateFB(Comment entry)
        {
            var arg = new
            {
                FeedbackId = entry.FeedbackId
            };
            var id = _respository.ExcuteSqlQuery(@"
            INSERT INTO  Comment (
	            ClientId
	            ,CommentType
	            ,Content
	            ,LessonId
	            ,CreatedDate,
                FeedbackId,
                Status
            ) SELECT " +
                entry.ClientId + "," +
                entry.CommentType + ",N'" +
                entry.Content + "'," +
                entry.LessonId + ",'" +
                entry.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                entry.FeedbackId + "," +
                entry.Status +
             " where not exists (select CommentId from comment where feedbackid = @FeedbackId)", arg);
            return new Response()
            {
                Success = id,
                Message = ""
            };
        }

        public Response CreateFBA(CommentAnswer entry)
        {
            var arg = new
            {
                FBAnswerId = entry.FBAnswerId
            };
            var id = _respository.ExcuteSqlQuery(@"
            INSERT INTO  CommentAnswer (
	            CommentId
	            ,ClientId
	            ,ClientName
	            ,UserId
	            ,UserName,
                Answer,
                AnswerTime,
                Status,
                FeedbackId,
                FBAnswerId
            ) SELECT " +
                entry.CommentId + "," +
                entry.ClientId + ",N'" +
                entry.ClientName + "'," +
                entry.UserId + ",N'" +
                entry.UserName + "',N'" +
                entry.Answer + "','" +
                entry.AnswerTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                entry.Status + "," +
                entry.FeedbackId + "," +
                entry.FBAnswerId +
            " where not exists (select Id from CommentAnswer where FBAnswerId = @FBAnswerId)", arg);
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
        public Response Update(Comment entry)
        {
            var arg = new
            {
                ClientId = entry.ClientId
                ,
                CommentType = entry.CommentType
                ,
                Content = entry.Content
                ,
                LessonId = entry.LessonId
                ,
                CreatedDate = entry.CreatedDate
                ,
                CommentId = entry.CommentId
            };
            var obj = _respository.ExcuteSqlQuery(@"
                UPDATE [Comment] SET
                [ClientId] = @ClientId
	            ,[CommentType] = @CommentType
	            ,[Content] = @Content
	            ,[LessonId] = @LessonId
	            ,[CreatedDate] = @CreatedDate
                WHERE
	            [CommentId] = @CommentId", arg);
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
                CommentId = id,
            };
            return _respository.ExcuteStoreV2("delete from Comment where CommentId = @CommentId", arg);
        }

        public void UpdateFBAStatus(string caid, string status)
        {
            _respository.ExcuteSql("update CommentAnswer set [Status] = " + status + " Where Id = " + caid);
        }


        public Response SendComment(Comment entry)
        {
            //            var arg = new
            //            {
            //                ClientId = entry.ClientId
            //                ,
            //                CommentType = entry.CommentType
            //                ,
            //                Content = entry.Content
            //                ,
            //                LessonId = entry.LessonId
            //                ,
            //                CreatedDate = entry.CreatedDate
            //                ,
            //                CommentId = entry.CommentId
            //            };
            //            var obj = _respository.ExcuteSqlQuery(@"
            //                UPDATE [Comment] SET
            //                ,[ClientId] = @ClientId
            //	            ,[CommentType] = @CommentType
            //	            ,[Content] = @Content
            //	            ,[LessonId] = @LessonId
            //	            ,[CreatedDate] = @CreatedDate
            //                WHERE
            //	            [CommentId] = @CommentId", arg);
            return new Response
            {
                Success = true
            };
        }

        public void UpdateTableComment()
        {
            _respository.ExcuteSql("alter table Comment add column FeedbackId int");
        }

        public void CreateTableCA()
        {
            _respository.ExcuteSql("CREATE TABLE [CommentAnswer] (Id int NOT NULL IDENTITY(1,1) PRIMARY KEY, CommentId int, ClientId int, ClientName nvarchar(255), UserId int, UserName nvarchar(255), Answer nvarchar(1000), AnswerTime datetime, Status int, FeedbackId int, FBAnswerId int)");
            _respository.ExcuteSql("CREATE TABLE [ClientLessonUse] ([Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY, [ClientId] int, [LessonId] int, [OpenCount] int, [DownloadCount] int, [LastOpen] datetime)");
        }

        public bool CheckTableCA()
        {
            List<InfoSchema> result = null;
            result = _respository.GetListBySqlQuery<InfoSchema>("SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CommentAnswer'");
            return result == null || result.Count <= 0;
        }
    }
}

