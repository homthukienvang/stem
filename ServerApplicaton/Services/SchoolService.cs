using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public interface ISchoolService
    {
        School GetById(int id);
        School GetByCode(string code);
        Response Update(School entry);
        Response Create(School entry);
        Response Delete(int id);
        IEnumerable<School> GetByPage(int pageIndex, int pageSize, string schoolCode, string citycode, string districtcode, string usercitycode, string userdistrictcode, string deploymentmethod, out int totalRow);
        IEnumerable<School> GetAll();
        IEnumerable<School> GetAllCode();
    }

    public class SchoolService : ISchoolService
    {
        private readonly ICommonRepository _respository;

        public SchoolService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public SchoolService()
            : this(new CommonRepository(new Database()))
        {
        }
        public School GetById(int id)
        {
            var arg = new
            {
                SchoolId = id
            };
            return _respository.GetObjectByStoreV2<School>("[dbo].[Proc_SelectByID_School]", arg);
        }

        public School GetByCode(string code)
        {
            var arg = new
            {
                Code = code
            };
            return _respository.GetObjectByStoreV2<School>("[dbo].[Proc_SelectBycode_School]", arg);
        }

        public Response Update(School entry)
        {
            var arg = new
            {
                entry.SchoolId,
                entry.Code,
                entry.Name,
                entry.CityCode,
                entry.DistrictCode,
                entry.DeploymentMethod,
                entry.NumberOfStudent,
                entry.PrintDocument,
                entry.Certificate
            };
            var obj = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Update_School]", arg);
            return new Response
            {
                Success = Convert.ToInt32(obj) > 0,
                Message = Convert.ToInt32(obj) == -1 ? "Mã trường đã tồn tại" : ""
            };
        }

        public Response Create(School entry)
        {
            var arg = new
            {
                entry.Code,
                entry.Name,
                entry.CityCode,
                entry.DistrictCode,
                entry.DeploymentMethod,
                entry.NumberOfStudent,
                entry.PrintDocument,
                entry.Certificate
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_School]", arg);

            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = Convert.ToInt32(id) == -1 ? "Mã trường đã tồn tại" : ""
            };
        }

        public Response Delete(int id)
        {
            var arg = new
            {
                SchoolId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_School]", arg);
        }

        public IEnumerable<School> GetAll()
        {
            return _respository.GetListByStore<School>("[dbo].[Proc_SelectAll_School]");
        }

        public IEnumerable<School> GetAllCode()
        {
            return _respository.GetListByStore<School>("[dbo].[Proc_SelectAllCode_School]");
        }

        public IEnumerable<School> GetByPage(int pageIndex, int pageSize, string schoolCode, string citycode, string districtcode, string usercitycode, string userdistrictcode, string deploymentmethod, out int totalRow)
        {
            var arg = new
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Code = schoolCode,
                CityCode = citycode,
                DistrictCode = districtcode,
                UserCityCode = usercitycode,
                UserDistrictCode = userdistrictcode,
                deploymentmethod = deploymentmethod
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<School>("[dbo].[Proc_SelectPaged_School]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
    }
}
