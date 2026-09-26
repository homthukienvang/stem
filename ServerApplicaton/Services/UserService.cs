using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Extensions;
using Model;
using Repositories.Interfaces;

namespace Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUserByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<User> GetAllUser();
        Response Create(User entry);
        User GetUserById(int id);
        Response Update(User entry);
        Response Delete(int id);
        User Login(User model);
        List<Right> GetRight(int userId);
        Response ResetPassword(User model);
    }

    public class UserService : IUserService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public UserService()
        {
        }

        public UserService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public Response ResetPassword(User model)
        {
            string sql = String.Format(@"UPDATE acc.[User] SET Password = N'{1}' WHERE UserId = {0}",
                model.UserId, model.Password.MD5Hash());

            return _respository.ExcuteSql(sql);
        }
        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<User> GetAllUser()
        {
            return _respository.GetListByStore<User>("[acc].[Proc_SelectAll_User]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<User> GetUserByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("PageSize", pageSize),
                new KeyValuePair<string, object>("PageIndex", pageIndex),
                new KeyValuePair<string, object>("OrderByExpression", null)
            };
            var rows = 0;
            var list = _respository.GetListByStore<User>("[acc].[Proc_SelectPaged_User]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public User GetUserById(int id)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("UserId", id),
            };
            var user = _respository.GetObjectByStore<User>("[acc].[Proc_SelectByID_User]", arg);
            var sql =
                string.Format(@"select r.*,(case when u.UserId IS NULL then Cast(0 as bit) else  Cast(1 as bit) end )AS Selected 
 from acc.Role r left join acc.UserRole  u on r.RoleId = u.RoleId and  u.UserId  = {0}
 ", id)
                ;
            user.Roles = _respository.GetListBySqlQuery<Role>(sql);
            return user;
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(User entry)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("UserActivated", entry.UserActivated),
                new KeyValuePair<string, object>("CreatedDate", entry.CreatedDate),
                new KeyValuePair<string, object>("LastUpdatePassword", entry.LastUpdatePassword),
                new KeyValuePair<string, object>("UserName", entry.UserName),
                new KeyValuePair<string, object>("Password", entry.Password.MD5Hash()),
                new KeyValuePair<string, object>("DisplayName", entry.DisplayName),
                new KeyValuePair<string, object>("CityCode", entry.CityCode),
                new KeyValuePair<string, object>("DistrictCode", entry.DistrictCode)
            };
            var res = _respository.GetObjectByStore<Response>("[acc].[Proc_CreateNewUser]", arg);
            var userid = Convert.ToInt32(res.Id);
            if (userid > 0 && entry.Roles != null)
            {
                string sql = "";

                foreach (var r in entry.Roles)
                {
                    sql += String.Format(@"INSERT INTO  [acc].[UserRole]
                                       ([RoleId]
                                       ,[UserId]
                                       ,[Activated]
                                       ,[StartDate]
                                       ,[EndDate])
                                 VALUES
                                       ({0}
                                       ,{1}
                                       ,1
                                       ,GETDATE()
                                       ,GETDATE());", r.RoleId, userid);
                }
                _respository.ExcuteSql(sql);
            }

            return res;
        }

        /// <summary>
        /// Update the exist row
        /// </summary>
        /// <author>Louis</author>
        public Response Update(User entry)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("UserId", entry.UserId),
                new KeyValuePair<string, object>("UserActivated", entry.UserActivated),
                new KeyValuePair<string, object>("CreatedDate", entry.CreatedDate),
                new KeyValuePair<string, object>("LastUpdatePassword", entry.LastUpdatePassword),
                new KeyValuePair<string, object>("UserName", entry.UserName),
                new KeyValuePair<string, object>("Password", entry.Password.MD5Hash()),
                new KeyValuePair<string, object>("DisplayName", entry.DisplayName),
                new KeyValuePair<string, object>("CityCode", entry.CityCode),
                new KeyValuePair<string, object>("DistrictCode", entry.DistrictCode)
            };
            var obj = _respository.ExcuteStore("[acc].[Proc_Update_User]", arg);

            if (entry.UserName != "admin")
            {
                //xử lý liên kết cho tài khoản khác admin (admin ko cần)
                string sql = string.Format(@"DELETE [acc].[UserRole] where  UserId ={0};", entry.UserId);
                if (entry.Roles != null)
                {
                    foreach (var r in entry.Roles)
                    {
                        sql += String.Format(@" INSERT INTO  [acc].[UserRole]
                                       ([RoleId]
                                       ,[UserId]
                                       ,[Activated]
                                       ,[StartDate]
                                       ,[EndDate])
                                 VALUES
                                       ({0}
                                       ,{1}
                                       ,1
                                       ,GETDATE()
                                       ,GETDATE());", r.RoleId, entry.UserId);
                    }
                }
                _respository.ExcuteSql(sql);
            }

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
            var arg = new[]
            {
                new KeyValuePair<string, object>("UserId", id),
            };
            return _respository.ExcuteStore("[acc].[Proc_DeleteByID_User]", arg);
        }

        public User Login(User model)
        {
            var arg = new[]
               {
                new KeyValuePair<string, object>("UserName", model.UserName) ,
                new KeyValuePair<string, object>("Password", model.Password.MD5Hash())
            };
            return _respository.GetObjectByStore<User>("[dbo].[Proc_User_Login]", arg);
        }

        public List<Right> GetRight(int userId)
        {
            var arg = new[]
                 {
                new KeyValuePair<string, object>("UserId", userId) ,
            };
            return _respository.GetListByStore<Right>("[dbo].[Proc_User_GetRight]", arg);
        }
    }
}