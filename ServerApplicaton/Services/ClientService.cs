using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using Extensions;

namespace Services
{
    public interface IClientService
    {
        IEnumerable<Client> GetClientByPage(int pageIndex, string tclass, string schoolCode, string cityCode,
            string districtCode, string fullName, string deploymethod, int approve, string usercitycode, string userdistrictcode, int pageSize, out int totalRow);

        IEnumerable<Client> GetAllClient();
        Response Create(Client entry);
        Client GetClientById(int id);
        Response Update(Client entry);
        Response Delete(int id);
        Client LoginToApp(string userName, string passWord, string macid);
        Client LoginToAppV2(string userName, string passWord, string macid);
        IEnumerable<ClientPayment> GetClientPayment(int clientId);
        Response CreateClientPayment(Client model);
        IEnumerable<Lesson> GetClientLessons(int id);
        Response CreateClientLesson(Client model);
        IEnumerable<ClientLesson> GetClientLessonsByClientId(int clientId);
        IEnumerable<ClientLesson> GetClientLessonsByClientIdV2(int clientId);
        Client ChangePass(string userName, string passWord);
        News GetNews();
        IEnumerable<Lesson> GetListLessons();
        Response CreateListClientLesson(Client entry);
        Response ApproveListClient(Client model);
        Response LockListClient(Client model);
        Response DeleteListClient(Client model);
        IEnumerable<Lesson> GetClientLessonViewDetail(int id);
        Response UpdateLessonViewCount(ClientLessonView lesson);
        Response ResetPassword(Client model);
        Response PostUpdateInfo(string clientId, string email, string phone);
        Response ReactiveListClient(Client model);
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
            : this(new CommonRepository(new Database()))
        {
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<Client> GetAllClient()
        {
            return _respository.GetListByStore<Client>("[dbo].[Proc_SelectAll_Client]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<Client> GetClientByPage(int pageIndex, string tclass, string schoolCode, string cityCode,
            string districtCode, string fullName, string deploymethod, int approve, string usercitycode, string userdistrictcode, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                SchoolCode = schoolCode,
                Email = tclass,
                CityCode = cityCode,
                DistrictCode = districtCode,
                FullName = fullName,
                Approve = approve,
                UserCityCode = usercitycode,
                UserDistrictCode = userdistrictcode,
                DeploymentMethod = deploymethod
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
            return _respository.GetObjectByStoreV2<Client>("[dbo].[Proc_SelectByID_Client]", arg);
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
                entry.FullName,
                entry.UserName,
                entry.Phone,
                entry.Email,
                entry.ImageUrl,
                entry.IsActived,
                entry.IsLock,
                entry.IsDeleted,
                entry.PassWork,
                entry.SchoolCode,
                entry.CityCode,
                entry.DistrictCode,
                entry.Address,
                entry.FullLesson
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_Client]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = Convert.ToInt32(id) == -1 ? "Tên đăng nhập đã tồn tại" : ""
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
                entry.IsLock,
                entry.SchoolCode,
                entry.CityCode,
                entry.DistrictCode,
                entry.Address,
                entry.MacIp,
                entry.FullLesson
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_Client]", arg);
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
                ClientId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Client]", arg);
        }

        public Response ResetPassword(Client model)
        {
            string sql = String.Format(@"UPDATE dbo.[Client] SET PassWork = N'{1}' WHERE ClientId = {0}", model.ClientId, model.PassWork.MD5Hash());

            return _respository.ExcuteSql(sql);
        }

        public Response PostUpdateInfo(string clientId, string email, string phone)
        {
            clientId = clientId.Replace("'", "").Replace(";", "");
            email = email.Replace("'", "").Replace(";", "");
            phone = phone.Replace("'", "").Replace(";", "");

            string sql = String.Format(@"UPDATE dbo.[Client] SET [Address] = N'{1}', Phone = N'{2}' WHERE ClientId = {0}", clientId, email, phone);

            return _respository.ExcuteSql(sql);
        }

        public Client LoginToApp(string userName, string passWord, string macid)
        {
            string sql = string.Format("SELECT * from [Client] where (LOWER(UserName)  = N'{0}' )ANd PassWork =N'{1}'",
                userName, passWord);
            var client = _respository.GetObjectBySqlQuery<Client>(sql);
            if (client != null)
            {
                if (string.IsNullOrEmpty(client.MacIp))
                {
                    _respository.ExcuteSql(
                        string.Format("UPDATE [dbo].[Client]    SET   [MacIp] = '{0}'  WHERE ClientId = {1}", macid,
                            client.ClientId));
                    client.MacIp = macid;
                }
                else
                {
                    if (client.MacIp != macid)
                    {
                        return null;
                    }
                }


                var payement = _respository.GetObjectBySqlQuery<ClientPayment>(
                    "SELECT top  1  * from [dbo].[ClientPayment] where clientId = " + client.ClientId +
                    "  order by EndDate desc");
                if (payement != null && payement.ClientId > 0)
                {
                    client.EndDate = payement.EndDate;
                }
                client.MacIp = macid;
                if (client.MacIp != macid)
                {
                    return null;
                }
            }

            return client;
        }

        public Client LoginToAppV2(string userName, string passWord, string macid)
        {
            string sql = string.Format("SELECT * from [Client] where Approved = 1 and [IsLock] = 0 and (LOWER(UserName)  = N'{0}' )ANd PassWork =N'{1}'",
                userName, passWord);
            var client = _respository.GetObjectBySqlQuery<Client>(sql);
            if (client != null)
            {
                if (string.IsNullOrEmpty(client.MacIp))
                {
                    _respository.ExcuteSql(
                        string.Format("UPDATE [dbo].[Client]    SET   [MacIp] = '{0}'  WHERE ClientId = {1}", macid,
                            client.ClientId));
                    client.MacIp = macid;
                }
                else
                {
                    //if (!client.UserName.ToUpper().Equals("DEMO"))
                        if (!client.MacIp.Contains(macid) && !macid.Contains(client.MacIp))
                        {
                            return null;
                        }
                }

                var payement = _respository.GetObjectBySqlQuery<ClientPayment>(
                    "SELECT top  1  * from [dbo].[ClientPayment] where clientId = " + client.ClientId +
                    "  order by EndDate desc");
                if (payement != null && payement.ClientId > 0)
                {
                    client.EndDate = payement.EndDate;
                    client.ServerTime = DateTime.Now;
                    //if (payement.EndDate > DateTime.Now)
                    if (payement.EndDate < DateTime.Now)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
                client.MacIp = macid;
                if (client.MacIp != macid)
                {
                    return null;
                }
            }

            return client;
        }

        public IEnumerable<ClientPayment> GetClientPayment(int clientId)
        {
            string sql =
                string.Format(
                    " SELECT *  FROM [dbo].[ClientPayment] WHERE [ClientId] = {0} ORDER BY [PaymentDate] DESC", clientId);
            return _respository.GetListBySqlQuery<ClientPayment>(sql);
        }

        public Response CreateClientPayment(Client entry)
        {
            var arg = new
            {
                entry.ClientId,
                entry.Amount,
                entry.PaymentDate,
                entry.PaymentCode,
                entry.Description,
                entry.BeginDate,
                entry.EndDate,
                entry.CreatedUserName,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_ClientPayment]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = ""
            };
        }

        public IEnumerable<Lesson> GetClientLessons(int clientId)
        {
            string sql =
                string.Format(
                    @"SELECT l.*, cl.EndDate , ( CASE WHEN cl.ClientId IS NULL then Cast(0 AS bit) ELSE Cast(1 AS bit)END )    AS IsSelected 
FROM dbo.Lesson l LEFT JOIN dbo.ClientLesson cl ON cl.LessonId = l.LessonId AND cl.ClientId =  {0} ", clientId);
            return _respository.GetListBySqlQuery<Lesson>(sql);
        }

        public IEnumerable<Lesson> GetListLessons()
        {
            string sql =
                string.Format(
                    @"SELECT l.*,   Cast(0 AS bit)      AS IsSelected 
FROM dbo.Lesson l ");
            return _respository.GetListBySqlQuery<Lesson>(sql);
        }

        public Response CreateListClientLesson(Client entry)
        {
            string sql = string.Format(@"UPDATE [dbo].[Client] SET FullLesson ={1}
                                        WHERE  ClientId in ({0})
                                       ", string.Join(",", entry.ListId), entry.FullLesson == true ? "1" : "0");
            if (entry.ResetLesson)
            {
                sql += string.Format(@"DELETE dbo.ClientLesson WHERE ClientId in ({0}) 
                                                   ", string.Join(",", entry.ListId));
            }


            if (entry.ClientLessons != null)
            {
                foreach (var id in entry.ListId)
                {
                    foreach (var clientLesson in entry.ClientLessons)
                    {
                        sql +=
                            string.Format(@"IF NOT EXISTS (SELECT * FROM dbo.ClientLesson WHERE LessonId ={0}  AND ClientId = {1})
                                INSERT dbo.ClientLesson
                                    ( LessonId ,
                                      ClientId ,
                                      CreatedUserId ,
                                      CreatedDate ,
                                      IsLocked ,
                                      IsDeleted,
                                        EndDate
                                    )
                            VALUES  ( {0} ,
                                      {1} ,
                                      0,
                                      GETDATE() ,
                                      0,
                                      0,
                                      {2}
                                    )
                        ", clientLesson.LessonId, id, clientLesson.EndDate.HasValue
                                            ? string.Format("'{0}'", clientLesson.EndDate.Value.ToString("yyyy-MM-dd"))
                                            : "NULL");
                        if (clientLesson.EndDate.HasValue)
                        {
                            sql +=
                                string.Format(@" UPDATE dbo.ClientLesson SET EndDate ='{2}'  WHERE LessonId ={0}  AND ClientId = {1})
                        ", clientLesson.LessonId, id, clientLesson.EndDate.Value.ToString("yyyy-MM-dd"));
                        }
                    }
                }
            }
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

        public Response ApproveListClient(Client model)
        {
            string sql = string.Format(@"UPDATE [dbo].[Client] SET Approved ={1} , ApproverId = {2}
                                        WHERE  ClientId in ({0})
                                       ", string.Join(",", model.ListId), model.Approved == true ? "1" : "0",
                model.ApproverId);
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

        public Response LockListClient(Client model)
        {
            string sql = string.Format(@"UPDATE [dbo].[Client] SET IsLock ={1}
                                        WHERE  ClientId in ({0})
                                       ", string.Join(",", model.ListId), model.IsLock == true ? "1" : "0");
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

        public Response ReactiveListClient(Client model)
        {
            string sql = string.Format(@"UPDATE [dbo].[Client] SET MacIp = ''
                                        WHERE  ClientId in ({0})
                                       ", string.Join(",", model.ListId));
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

        public Response DeleteListClient(Client model)
        {
            string sql = string.Format(@"Delete [dbo].[Client] WHERE  ClientId in ({0})
                                       ", string.Join(",", model.ListId), model.Approved == true ? "1" : "0",
                model.ApproverId);
            _respository.WithTransaction(() =>
            {
                foreach (var id in model.ListId)
                {
                    var arg = new
                    {
                        ClientId = id,
                    };
                    _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Client]", arg);
                }
                return new Response();
            });
            return new Response()
            {
                Success = true,
                Message = ""
            };
        }

        public IEnumerable<Lesson> GetClientLessonViewDetail(int id)
        {
            string sql =
                 string.Format(
                     @"    SELECT  l.LessonId ,
                        l.RoomId ,
                        l.Code ,
                        l.Name ,
                        cl.OpenCount ,
                        cl.DownloadCount ,
                        cl.LastOpen,
                        cl.LastUse
                FROM    dbo.Lesson l
                        JOIN dbo.ClientLessonView cl ON cl.LessonId = l.LessonId
                                                             AND cl.ClientId =  {0} ", id);
            return _respository.GetListBySqlQuery<Lesson>(sql);
        }

        public Response UpdateLessonViewCount(ClientLessonView lesson)
        {
            var arg = new
            {
                ClientId = lesson.ClientId,
                LessonId = lesson.LessonId,
                lesson.DownloadCount,
                lesson.OpenCount,
                lesson.LastUse
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_UpdateClientLessonView]", arg);
        }

        public Response CreateClientLesson(Client entry)
        {
            string sql = string.Format(@"UPDATE [dbo].[Client] SET FullLesson ={1}
                                        WHERE  ClientId = {0}
                                       ", entry.ClientId, entry.FullLesson == true ? "1" : "0");
            sql += string.Format(@"DELETE dbo.ClientLesson WHERE ClientId = {0}
                                       ", entry.ClientId);
            if (entry.ClientLessons != null)
                foreach (var clientLesson in entry.ClientLessons)
                {
                    sql += string.Format(@"INSERT dbo.ClientLesson
                        ( LessonId ,
                          ClientId ,
                          CreatedUserId ,
                          CreatedDate ,
                          IsLocked ,
                          IsDeleted, EndDate
                        )
                VALUES  ( {0} ,
                          {1} ,
                          0,
                          GETDATE() ,
                          0,
                          0,{2}
                        )
            ", clientLesson.LessonId, entry.ClientId, clientLesson.EndDate.HasValue
                                            ? string.Format("'{0}'", clientLesson.EndDate.Value.ToString("yyyy-MM-dd"))
                                            : "NULL");
                }
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

        public IEnumerable<ClientLesson> GetClientLessonsByClientId(int clientId)
        {
            string sql =
                string.Format(
                    @"SELECT cl.* FROM    dbo.ClientLesson cl  WHERE cl.ClientId =  {0} ", clientId);
            return _respository.GetListBySqlQuery<ClientLesson>(sql);
        }

        public IEnumerable<ClientLesson> GetClientLessonsByClientIdV2(int clientId)
        {
            string sql =
                string.Format(
                    @"SELECT cl.* FROM    dbo.ClientLesson cl  WHERE  (  cl.EndDate IS NULL
            OR cl.EndDate >= GETDATE()) and  cl.ClientId =  {0} ", clientId);
            return _respository.GetListBySqlQuery<ClientLesson>(sql);
        }

        public Client ChangePass(string userName, string passWord)
        {
            string sql = string.Format(@"UPDATE [dbo].[Client] SET [PassWork] =N'{1}' WHERE  UserName =N'{0}'
                                       ", userName, passWord);
            sql += string.Format(@"SELECT *   FROM [dbo].[Client] WHERE LOWER(UserName)  = N'{0}'", userName);
            return _respository.GetObjectBySqlQuery<Client>(sql);
        }

        public News GetNews()
        {
            string sql = string.Format(@"select top 1* from news");
            return _respository.GetObjectBySqlQuery<News>(sql);
        }
    }
}