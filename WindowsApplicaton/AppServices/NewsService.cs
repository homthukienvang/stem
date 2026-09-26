using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public interface INewsService
    {
        News GetById(int id);
        News GetByStatus(int id);
        IEnumerable<News> GetAll();
        Response Create(News entry);
        Response Update(News entry);
        Response Delete(int id);
    }

    public class NewsService : INewsService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public NewsService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public NewsService()
            : this(new CommonRepository(new Database()))
        {
        }

        public News GetById(int id)
        {
            var arg = new
            {
                NewsId = id
            };
            return _respository.GetObjectBySqlQueryV2<News>("SELECT * FROM [News] where NewsId =@NewsId ", arg);
        }

        public News GetByStatus(int id)
        {
            var arg = new
            {
                Status = id
            };
            return _respository.GetObjectBySqlQueryV2<News>("SELECT * FROM [News] where Status = @Status ", arg);
        }

        public IEnumerable<News> GetAll()
        {
            return _respository.GetListBySqlQuery<News>("SELECT * FROM [News]");
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(News entry)
        {
            var arg = new
            {
                NewsId = entry.NewsId,
                Title = entry.Title,
                Description = entry.Description,
                entry.ImageUrl,
                entry.NewsType,
            };
            var id = _respository.ExcuteSqlQuery(@"
            INSERT INTO  [News] (
                NewsId
	         
	            ,[Title]
                ,Description
	            ,[Status]
                ,ImageUrl
                ,NewsType
            ) VALUES (
                @NewsId
                
	            ,@Title
                ,@Description
	            ,0
                ,@ImageUrl
                ,@NewsType
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
        public Response Update(News entry)
        {
            var arg = new
            {
                Title = entry.Title
                ,
                Description = entry.Description
                ,
                CreatedDate = entry.CreatedDate
                ,
                NewsId = entry.NewsId
                ,
                Status = entry.Status,
                entry.ImageUrl
            };
            var obj = _respository.ExcuteSqlQuery(@"
                UPDATE [News] SET
                [Title] = @Title
	            ,[Description] = @Description	           
                ,[Status] = @Status
                ,[ImageUrl] = @ImageUrl
                WHERE
	            [NewsId] = @NewsId", arg);
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
            return _respository.ExcuteStoreV2("delete from News where NewsId = @NewsId", arg);
        }
    }
}