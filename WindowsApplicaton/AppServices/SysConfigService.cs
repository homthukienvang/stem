using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;

namespace Services
{
    public interface ISysConfigService
    {
        IEnumerable<SysConfig> GetSysConfigByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<SysConfig> GetAllSysConfig();
        Response Create(SysConfig entry);
        SysConfig GetSysConfigById(int id);
        Response Update(SysConfig entry);
        Response Delete(int id);
        SysConfig GetSysConfigByKey(string dataversion);
    }

    public class SysConfigService : ISysConfigService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public SysConfigService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public SysConfigService()
        {
            _respository = new CommonRepository();
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<SysConfig> GetAllSysConfig()
        {
            return _respository.GetListByStore<SysConfig>("[dbo].[Proc_SelectAll_SysConfig]");
        }

        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<SysConfig> GetSysConfigByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<SysConfig>("[dbo].[Proc_SelectPaged_SysConfig]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public SysConfig GetSysConfigById(int id)
        {
            var arg = new
            {
                SysConfigId = id
            };
            return _respository.GetObjectByStoreV2<SysConfig>("[dbo].[Proc_SelectByID_SysConfig]", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(SysConfig entry)
        {
            var arg = new
            {
                Code = entry.Code,
                Name = entry.Name,
                Value = entry.Value,
            };
            var id = _respository.ExcuteSqlQueryGetValueV2(@"
                        INSERT INTO  [SysConfig] (
	                        [Code],
	                        [Name],
	                        [Value]
                        ) VALUES (
	                        @Code,
	                        @Name,
	                        @Value
                        )
                        ", arg);
            return new Response()
            {
                Success = true
            };
        }

        /// <summary>
        /// Update the exist row
        /// </summary>
        /// <author>Louis</author>
        public Response Update(SysConfig entry)
        {
            var arg = new
            {
                SysConfigId = entry.SysConfigId,
                Code = entry.Code,
                Name = entry.Name,
                Value = entry.Value,
            };
            var obj = _respository.ExcuteSqlQueryGetValueV2(@"
                UPDATE  [SysConfig] SET
	                [Code] = @Code,
	                [Name] = @Name,
	                [Value] = @Value
                WHERE
	                [SysConfigId] = @SysConfigId", arg);
            return new Response
            {
                Success = true
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
                SysConfigId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_SysConfig]", arg);
        }

        public SysConfig GetSysConfigByKey(string code)
        {
            return _respository.GetObjectBySqlQueryV2<SysConfig>("SELECT * 	FROM [SysConfig] where Code =@Code",
                new {Code = code});
        }
    }
}