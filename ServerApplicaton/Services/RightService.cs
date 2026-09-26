using System;
using System.Collections.Generic;
using System.Linq;   
using Model;
using Repositories.Interfaces;

namespace Services
{

    public interface IRightService
    {
        IEnumerable<Right> GetRightByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<Right> GetAllRight();
        Response Create(Right entry);
        Right GetRightById(int id);
        Response Update(Right entry);
        Response Delete(int id, int currentUserId);
        List<Right> GetActiveRight();
        List<Right> GetRightByPortal(int portalid);
    }
    public class RightService : IRightService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public RightService(ICommonRepository respository)
        {
            _respository = respository;
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<Right> GetAllRight()
        {
            return _respository.GetListByStore<Right>("[acc].[Proc_SelectAll_Right]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<Right> GetRightByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("PageSize", pageSize),
                new KeyValuePair<string, object>("PageIndex", pageIndex),
                new KeyValuePair<string, object>("OrderByExpression", "[Order]")
            };
            var rows = 0;
            var list = _respository.GetListByStore<Right>("[acc].[Proc_SelectPaged_Right]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Right GetRightById(int id)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("RightId", id),
            };
            return _respository.GetObjectByStore<Right>("[acc].[Proc_SelectByID_Right]", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Right entry)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("ParentId", entry.ParentId),
                new KeyValuePair<string, object>("RightName", entry.RightName),
                new KeyValuePair<string, object>("ShowInMenu", entry.ShowInMenu),
                new KeyValuePair<string, object>("Icon", entry.Icon),
                new KeyValuePair<string, object>("Order", entry.Order),
                new KeyValuePair<string, object>("NameInMenu", entry.NameInMenu),
                new KeyValuePair<string, object>("PortalId", entry.PortalId),
                new KeyValuePair<string, object>("Url", entry.Url),
                new KeyValuePair<string, object>("Status", entry.Status),
            };
            var id = _respository.GetValueByKey("[acc].[Proc_Insert_Right]", "RightId", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = ""
            };
        }

        /// <summary>
        /// Update the exist row
        /// </summary>
        /// <author>Louis</author>
        public Response Update(Right entry)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("RightId", entry.RightId),
                new KeyValuePair<string, object>("ParentId", entry.ParentId),
                new KeyValuePair<string, object>("RightName", entry.RightName),
                new KeyValuePair<string, object>("ShowInMenu", entry.ShowInMenu),
                new KeyValuePair<string, object>("Icon", entry.Icon),
                new KeyValuePair<string, object>("Order", entry.Order),
                new KeyValuePair<string, object>("NameInMenu", entry.NameInMenu),
                new KeyValuePair<string, object>("PortalId", entry.PortalId),
                new KeyValuePair<string, object>("Url", entry.Url),
                new KeyValuePair<string, object>("Status", entry.Status),
            };
            var obj = _respository.ExcuteStore("[acc].[Proc_Update_Right]", arg);
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
        public Response Delete(int id, int currentUserId)
        {
            var arg = new[]
            {
                new KeyValuePair<string, object>("RightId", id),
                new KeyValuePair<string, object>("CurrentUserId", currentUserId),
            };
            return _respository.ExcuteStore("[acc].[Proc_DeleteByID_Right]", arg);
        }

        public List<Right> GetActiveRight()
        {
            var sql = "Select * from [acc].[Right] Where Status =1 and ShowInMenu =1  Order by [Order]";
            return _respository.GetListBySqlQuery<Right>(sql);
        }

        public List<Right> GetRightByPortal(int portalid)
        {
            return _respository.GetListBySqlQuery<Right>(@"Select * from acc.[right] where portalid = " + portalid);
        }
    }
}

