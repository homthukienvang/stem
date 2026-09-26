using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppServices
{
    public interface ICommentAnswerService
    {
        IEnumerable<CommentAnswer> GetAll();
        Response Create(CommentAnswer entry);
        CommentAnswer GetById(int id);
        Response Update(CommentAnswer entry);
        Response Delete(int id);
        int GetNewMessage();
    }

    public class CommentAnswerService : ICommentAnswerService
    {
        private readonly ICommonRepository _respository;

        public CommentAnswerService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public CommentAnswerService()
            : this(new CommonRepository(new Database()))
        {
        }

        public IEnumerable<CommentAnswer> GetAll()
        {
            return _respository.GetListBySqlQuery<CommentAnswer>("select * from CommentAnswer");
        }
        public Response Create(CommentAnswer entry)
        {
            var arg = new
            {
                entry.CommentId,
                entry.ClientId,
                entry.ClientName,
                entry.UserId,
                entry.UserName,
                entry.Answer,
                entry.AnswerTime,
                entry.Status
            };
            var id = _respository.ExcuteSqlQuery(@"
                        INSERT INTO  [CommentAnswer] (CommentId,
	                        [ClientId],
	                        [ClientName],
	                        [UserId],
	                        [UserName],
	                        [Answer],
	                        [AnswerTime],
	                        [Status]
                        ) VALUES (@CommentId,
	                        @ClientId,
	                        @ClientName,
	                        @UserId,
	                        @UserName,
	                        @Answer,
	                        @AnswerTime,
	                        @Status
                        )", arg);
            return new Response()
            {
                Success = id,
                Message = ""
            };
        }
        public CommentAnswer GetById(int id)
        {
            var arg = new
            {
                Id = id
            };
            return _respository.GetObjectBySqlQueryV2<CommentAnswer>("select * from CommentAnswer where Id = @Id", arg);
        }

        public int GetNewMessage()
        {
            List<CommentAnswer> list = _respository.GetListBySqlQuery<CommentAnswer>("select * from CommentAnswer where [Status] = 0");
            return list.Count;
        }

        public Response Update(CommentAnswer entry)
        {
            var arg = new
            {
                entry.Id,
                entry.CommentId,
                entry.ClientId,
                entry.ClientName,
                entry.UserId,
                entry.UserName,
                entry.Answer,
                entry.AnswerTime,
                entry.Status
            };
            var obj = _respository.ExcuteSqlQuery(@"
                    UPDATE  CommentAnswer SET
	                    CommentId = @CommentId,
	                    ClientId = @ClientId,
	                    ClientName = @ClientName,
	                    UserId = @UserId,
	                    UserName = @UserName,
	                    Answer = @Answer,
	                    AnswerTime = @AnswerTime,
	                    Status = @Status
                    WHERE
	                    [Id] = @Id", arg);
            return new Response
            {
                Success = obj
            };
        }
        public Response Delete(int id)
        {
            return new Response
            {
                Success = true
            };
        }
    }
}
