using Model;
using Repositories.Implementations;
using Repositories.Interfaces;

namespace Services
{
    public interface INewsService
    {
        News GetNewsById(int id);
        Response Update(News entry);
    }
    public class NewsService : INewsService
    {
        private readonly ICommonRepository _respository;

        public NewsService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public NewsService()
            : this(new CommonRepository(new Database()))
        {
        }
        public News GetNewsById(int id)
        {
            var arg = new
            {
                NewsId = id
            };
            return _respository.GetObjectByStoreV2<News>("[dbo].[Proc_SelectByID_News]", arg);
        }

        public Response Update(News entry)
        {
            var arg = new
            {
               entry.NewsId,
               entry.Title,
               entry.Description,
               entry.ImageUrl
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_News]", arg);
            return new Response
            {
                Success = obj.Success,
                Message = obj.Message
            };
        }
    }
}