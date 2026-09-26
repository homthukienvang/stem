using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public interface IClientService
    {
        IEnumerable<Client> GetClientByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<Client> GetAllClient();
        Client GetDefaultClient();
        Response Create(Client entry);
        Client GetClientById(int id);
        Response Update(Client entry);
        Response Delete(int id);
        Client LoginToApp(string userName, string pass, bool @checked);
        void UpdatePass(int clientId, string md5Hash);
        Response IncreeClientLessonView(int clientId, int lessonId);
        Response IncreeClientLessonDownload(int clientId, int lessonId);
        ClientLessonView GetClientLessonView(int clientId, int lessonId);
    }

    public class ClientService : IClientService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public ClientService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public ClientService()
        {
            _respository = new CommonRepository();
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<Client> GetAllClient()
        {
            return _respository.GetListBySqlQuery<Client>("SELECT * FROM [Client]");
        }

        public Client GetDefaultClient()
        {
            return _respository.GetObjectBySqlQuery<Client>("SELECT top 1 * FROM [Client]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<Client> GetClientByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<Client>("[dbo].[Proc_SelectPaged_Client]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Client GetClientById(int id)
        {
            var arg = new
            {
                ClientId = id
            };
            return _respository.GetObjectBySqlQueryV2<Client>("SELECT * FROM [Client] where  ClientId = @ClientId", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Client entry)
        {
            var arg = new
            {
                entry.ClientId,
                entry.FullName,
                entry.UserName,
                entry.Phone,
                entry.Email,
                entry.ImageUrl,
                entry.IsActived,
                entry.IsLock,
                entry.IsDeleted,
                entry.PassWork,
                entry.MacIp,
                entry.EndDate,
                entry.FullLesson
            };
            var id = _respository.ExcuteSqlQuery(@"
                        INSERT INTO  [Client] (ClientId,
	                        [FullName],
	                        [UserName],
	                        [Phone],
	                        [Email],
	                        [ImageUrl],
	                        [IsActived],
	                        [IsLock],
	                        [IsDeleted],
	                        [PassWork] ,
                            [MacIp],
                            [EndDate],
                            [FullLesson]
                        ) VALUES (@ClientId,
	                        @FullName,
	                        @UserName,
	                        @Phone,
	                        @Email,
	                        @ImageUrl,
	                        @IsActived,
	                        @IsLock,
	                        @IsDeleted,
	                        @PassWork ,
                            @MacIp,
                            @EndDate,    
                            @FullLesson
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
        public Response Update(Client entry)
        {
            var arg = new
            {
                entry.ClientId,
                entry.FullName,
                entry.UserName,
                entry.Phone,
                entry.Email,
                entry.ImageUrl,
                entry.IsActived,
                entry.IsLock,
                entry.IsDeleted,
                entry.EndDate,
                entry.FullLesson
            };
            var obj = _respository.ExcuteSqlQuery(@"
                    UPDATE  [Client] SET
	                    [FullName] = @FullName,
	                    [UserName] = @UserName,
	                    [Phone] = @Phone,
	                    [Email] = @Email,
	                    [ImageUrl] = @ImageUrl,
	                    [IsActived] = @IsActived,
	                    [IsLock] = @IsLock,
	                    [IsDeleted] = @IsDeleted,
                        EndDate =@EndDate,
                        FullLesson=@FullLesson                        
                    WHERE
	                    [ClientId] = @ClientId", arg);
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
                ClientId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Client]", arg);
        }

        public Client LoginToApp(string userName, string pass, bool savePass)
        {
            string sql = @" SELECT * from [Client] where LOWER(UserName)=@UserName ANd PassWork=@Password";
            var client = _respository.GetObjectBySqlQueryV2<Client>(sql,
                new { SavePass = savePass, UserName = userName.ToLower(), Password = pass });
            if (!(client == null || client.ClientId <= 0))
            {
                _respository.ExcuteSql(" Update [Client]  set SavePass  = " + (savePass ? "1" : "0"));
            }
            return client;
        }

        public void UpdatePass(int clientId, string md5Hash)
        {
            string sql = "Update [Client]  set  PassWork =@Password where ClientId= @ClientId";
            _respository.ExcuteSqlQuery(sql, new { Password = md5Hash, ClientId = clientId });
        }

        public Response IncreeClientLessonView(int clientId, int lessonId)
        {
            string sql = @"SELECT [ClientId]
                            ,[LessonId]
                            ,[OpenCount]
                            ,[DownloadCount]
                            ,[Updated]
                            FROM [ClientLessonView]
                            WHERE   ClientId = @ClientId
                            AND LessonId = @LessonId";
            var obj = _respository.GetObjectBySqlQueryV2<ClientLessonView>(sql,
                new { ClientId = clientId, LessonId = lessonId });
            if (obj != null && obj.ClientId > 0)
            {
                return _respository.ExcuteSql(string.Format(@"UPDATE [ClientLessonView]
	                        SET [OpenCount] =[OpenCount]+1
		                        ,[Updated] = 0
                        WHERE   ClientId ={0}
                        AND LessonId = {1}
                ", clientId, lessonId));
            }
            return _respository.ExcuteSql(string.Format(@"INSERT INTO [ClientLessonView]
		                ([ClientId]
		                ,[LessonId]
		                ,[OpenCount]
		                ,[DownloadCount]
		                ,[Updated])
	                VALUES
		                ({0}
		                ,{1}
		                ,1
		                ,1
		                ,0)
                ", clientId, lessonId));
        }

        public Response IncreeClientLessonDownload(int clientId, int lessonId)
        {

            string sql = @"SELECT [ClientId]
                            ,[LessonId]
                            ,[OpenCount]
                            ,[DownloadCount]
                            ,[Updated]
                            FROM [ClientLessonView]
                            WHERE   ClientId = @ClientId
                            AND LessonId = @LessonId";
            var obj = _respository.GetObjectBySqlQueryV2<ClientLessonView>(sql,
                new { ClientId = clientId, LessonId = lessonId });
            if (obj != null && obj.ClientId > 0)
            {
                return _respository.ExcuteSql(string.Format(@"UPDATE [ClientLessonView]
	                        SET  [DownloadCount] = [DownloadCount] +1
		                        ,[Updated] = 0
                        WHERE   ClientId ={0}
                        AND LessonId = {1}
                ", clientId, lessonId));
            }
            return _respository.ExcuteSql(string.Format(@"INSERT INTO [ClientLessonView]
		                ([ClientId]
		                ,[LessonId]
		                ,[OpenCount]
		                ,[DownloadCount]
		                ,[Updated])
	                VALUES
		                ({0}
		                ,{1}
		                ,1
		                ,1
		                ,0)
                ", clientId, lessonId));
        }

        public ClientLessonView GetClientLessonView(int clientId, int lessonId)
        {

            string sql = string.Format(@"UPDATE [ClientLessonView]
	                        SET  [Updated] = 1
                        WHERE   ClientId ={0}
                        AND LessonId ={1}
                        ", clientId, lessonId);
            _respository.ExcuteSql(sql);
            sql = @"  SELECT [ClientId]
                            ,[LessonId]
                            ,[OpenCount]
                            ,[DownloadCount]
                            ,[Updated]
                            FROM [ClientLessonView]
                            WHERE   ClientId = @ClientId
                            AND LessonId = @LessonId";
            return _respository.GetObjectBySqlQueryV2<ClientLessonView>(sql,
                  new { ClientId = clientId, LessonId = lessonId });
        }

        public Response UpdateClientOpen(int clientId, int lessonId)
        {
            return _respository.ExcuteSql(string.Format(@"INSERT INTO [ClientLessonUse]
		                ([ClientId]
		                ,[LessonId]
		                ,[OpenCount]
		                ,[DownloadCount]
		                ,[LastOpen])
	                VALUES
		                ({0}
		                ,{1}
		                ,1
		                ,0
		                ,GETDATE())
                ", clientId, lessonId));
        }

        public string GetClientUse(int clientId, int lessonId)
        {
            string result = "";
            string sql = @"  SELECT top 8 *
                            FROM [ClientLessonUse]
                            WHERE   ClientId = @ClientId
                            AND LessonId = @LessonId order by LastOpen desc";
            List<ClientLessonView> list = _respository.GetListBySqlQueryV2<ClientLessonView>(sql, new { ClientId = clientId, LessonId = lessonId });
            if (list.Any())
            {
                foreach (var item in list)
                {
                    result += item.LastOpen.ToString("dd/MM/yyyy HH:mm:ss") + ";";
                }
            }

            return result;
        }
    }
}