using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Interfaces;

namespace Services
{

    public interface IRoleService
    {
        IEnumerable<Role> GetRoleByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<Role> GetAllRole();
        Response Create(Role entry);
        Role GetRoleById(int id);
        Response Update(Role entry);
        Response Delete(int id);
    }
    public class RoleService : IRoleService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public RoleService(ICommonRepository respository)
        {
            _respository = respository;
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<Role> GetAllRole()
        {
            return _respository.GetListByStore<Role>("[acc].[Proc_SelectAll_Role]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<Role> GetRoleByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("PageSize", pageSize),
                new KeyValuePair<string, object>("PageIndex", pageIndex),
                new KeyValuePair<string, object>("OrderByExpression", null)
            };
            var rows = 0;
            var list = _respository.GetListByStore<Role>("[acc].[Proc_SelectPaged_Role]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Role GetRoleById(int id)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("RoleId", id),
            };
            var role = _respository.GetObjectByStore<Role>("[acc].[Proc_SelectByID_Role]", arg);
            string sql = string.Format(@"select r.RightId,r.ParentId,r.RightName , 
(case when rr.RoleId IS NULL then Cast(0 as bit) else  Cast(1 as bit) end )AS Selected from [acc].[Right] r left join acc.RoleRight rr on r.RightId = rr.RightId  and  rr.RoleId ={0}
 ", id);
            role.Rights = _respository.GetListBySqlQuery<Right>(sql);
            return role;
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Role entry)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("RoleActivated", entry.RoleActivated),
                new KeyValuePair<string, object>("RoleName", entry.RoleName),
                new KeyValuePair<string, object>("RoleDesc", entry.RoleDesc),
            };
            var id = _respository.GetValueByKey("[acc].[Proc_Insert_Role]", "RoleId", arg);
            var roleid = Convert.ToInt32(id);
            if (roleid > 0 && entry.Rights != null)
            {
                string sql = "";
                foreach (var right in entry.Rights)
                {
                    sql += String.Format(@"INSERT INTO  [acc].[RoleRight]
                               ([RoleId]
                               ,[RightId]
                               ,[Activated])
                         VALUES
                               ({0}
                               ,{1}
                               ,1);", roleid, right.RightId);
                }
                _respository.ExcuteSql(sql);
            }
            return new Response()
            {
                Success = roleid > 0,
                Message = ""
            };
        }

        /// <summary>
        /// Update the exist row
        /// </summary>
        /// <author>Louis</author>
        public Response Update(Role entry)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("RoleId", entry.RoleId),
                new KeyValuePair<string, object>("RoleActivated", entry.RoleActivated),
                new KeyValuePair<string, object>("RoleName", entry.RoleName),
                new KeyValuePair<string, object>("RoleDesc", entry.RoleDesc),
            };
            var obj = _respository.ExcuteStore("[acc].[Proc_Update_Role]", arg);
            string sql = string.Format(@"DELETE [acc].[RoleRight] where  RoleId ={0};", entry.RoleId);
            if (entry.Rights != null)
            {
                foreach (var right in entry.Rights)
                {
                    sql += string.Format(@"INSERT INTO  [acc].[RoleRight]
                               ([RoleId]
                               ,[RightId]
                               ,[Activated])
                         VALUES
                               ({0}
                               ,{1}
                               ,1);", entry.RoleId, right.RightId);
                }
            }
            _respository.ExcuteSql(sql);
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
                new KeyValuePair<string, object>("RoleId", id),
            };
            return _respository.ExcuteStore("[acc].[Proc_DeleteByID_Role]", arg);
        }
    }
}

