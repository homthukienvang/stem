using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Collections.Generic;

namespace AppServices
{
    public interface IAdvertisementService
    {
        Response Create(Advertisement entry);
        Advertisement GetAdvById(int id);
        Response Update(Advertisement entry);
        Response Delete(int id);
        bool CheckTableAdv();
        void CreateTableAdv();
    }

    public class AdvertisementService : IAdvertisementService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public AdvertisementService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public AdvertisementService()
        {
            _respository = new CommonRepository();
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Advertisement GetAdvById(int id)
        {
            var arg = new
            {
                Id = id
            };
            return _respository.GetObjectBySqlQueryV2<Advertisement>("SELECT * FROM Advertisement where  Id = @Id", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Advertisement entry)
        {
            var arg = new
            {
                entry.Id,
                entry.Title,
                entry.AdvContent,
                entry.AdvImage,
                entry.AdvLink,
                entry.StartDate,
                entry.EndDate,
                entry.ClickPerDay,
                entry.Flag,
                entry.ClickCount,
                entry.ClickLinkCount
            };
            var id = _respository.ExcuteSqlQuery(@"
                        INSERT INTO Advertisement
                           (Id
                           ,Title
                           ,AdvContent
                           ,AdvImage
                           ,AdvLink
                           ,StartDate
                           ,EndDate
                           ,ClickPerDay
                           ,Flag
                           ,ClickCount
                           ,ClickLinkCount)
                        VALUES (@Id
                           ,@Title
                           ,@AdvContent
                           ,@AdvImage
                           ,@AdvLink
                           ,@StartDate
                           ,@EndDate
                           ,@ClickPerDay
                           ,@Flag
                           ,@ClickCount
                           ,@ClickLinkCount)", arg);
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
        public Response Update(Advertisement entry)
        {
            var arg = new
            {
                entry.Id,
                entry.Title,
                entry.AdvContent,
                entry.AdvImage,
                entry.AdvLink,
                entry.StartDate,
                entry.EndDate,
                entry.ClickPerDay,
                entry.Flag,
                entry.ClickCount,
                entry.ClickLinkCount
            };
            var obj = _respository.ExcuteSqlQuery(@"
                    UPDATE  Advertisement SET
	                    Title = @Title,
	                    AdvContent = @AdvContent,
	                    AdvLink = @AdvLink,
                        AdvImage =@AdvImage,
	                    StartDate = @StartDate,
	                    EndDate = @EndDate,
	                    ClickPerDay = @ClickPerDay,
	                    Flag = @Flag,
                        ClickCount = @ClickCount,
                        ClickLinkCount = @ClickLinkCount
                    WHERE
	                    Id = @Id", arg);
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
                Id = id,
            };
            var obj = _respository.ExcuteSqlQuery("delete from Advertisement where Id = @Id", arg);

            return new Response
            {
                Success = obj
            };
        }

        public bool CheckTableAdv()
        {
            List<InfoSchema> result = null;
            result = _respository.GetListBySqlQuery<InfoSchema>("SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Advertisement'");
            return result == null || result.Count <= 0;
        }

        public void CreateTableAdv()
        {
            _respository.ExcuteSql("CREATE TABLE [Advertisement] ([Title] nvarchar(254), [StartDate] datetime, [EndDate] datetime, [ClickPerDay] int, [Flag] int, [AdvImage] nvarchar(254), [ClickCount] int DEFAULT 0, [AdvContent] ntext, [AdvLink] ntext, [ClickLinkCount] int DEFAULT 0,  [Id] int NOT NULL PRIMARY KEY)");
        }
    }

}
