using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public interface ISchoolLogService
    {
        //SchoolLog GetById(int id);
        List<SchoolLog> GetBySchoolId(int schoolid);
        //Response Update(SchoolLog entry);
        Response Create(SchoolLog entry);
        //Response Delete(int id);
        //IEnumerable<SchoolLog> GetByPage(int pageIndex, int pageSize, string schoolCode, string citycode, string districtcode, string usercitycode, string userdistrictcode, out int totalRow);
        //IEnumerable<SchoolLog> GetAll();
    }

    public class SchoolLogService : ISchoolLogService
    {
        private readonly ICommonRepository _respository;

        public SchoolLogService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public SchoolLogService()
            : this(new CommonRepository(new Database()))
        {
        }
        //public SchoolLog GetById(int id)
        //{
        //    var arg = new
        //    {
        //        Id = id
        //    };
        //    return _respository.GetObjectByStoreV2<SchoolLog>("[dbo].[Proc_SelectByID_SchoolLog]", arg);
        //}

        public List<SchoolLog> GetBySchoolId(int schoolId)
        {
            var arg = new
            {
                SchoolId = schoolId
            };
            return _respository.GetListByStoreV2<SchoolLog>("[dbo].[Proc_SelectBySchool_SchoolLog]", arg);
        }

        //public Response Update(SchoolLog entry)
        //{
        //    var arg = new
        //    {
        //        entry.Id,
        //        entry.SchoolId,
        //        entry.UserId,
        //        entry.CreatedDate,
        //        entry.Description
        //    };
        //    var obj = _respository.ExcuteStoreGetValueV2("[dbo].[]", arg);
        //    return new Response
        //    {
        //        Success = Convert.ToInt32(obj) > 0,
        //        Message = Convert.ToInt32(obj) == -1 ? "Mã trường đã tồn tại" : ""
        //    };
        //}

        public Response Create(SchoolLog entry)
        {
            var arg = new
            {
                entry.SchoolId,
                entry.UserId,
                entry.CreatedDate,
                entry.Description
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_SchoolLog]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = ""
            };
        }

        //public Response Delete(int id)
        //{
        //    var arg = new
        //    {
        //        SchoolId = id,
        //    };
        //    return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_School]", arg);
        //}

        //public IEnumerable<SchoolLog> GetAll()
        //{
        //    return _respository.GetListByStore<SchoolLog>("[dbo].[Proc_SelectAll_School]");
        //}

        //public IEnumerable<SchoolLog> GetAllCode()
        //{
        //    return _respository.GetListByStore<SchoolLog>("[dbo].[Proc_SelectAllCode_School]");
        //}

        //public IEnumerable<SchoolLog> GetByPage(int pageIndex, int pageSize, string schoolCode, string citycode, string districtcode, string usercitycode, string userdistrictcode, out int totalRow)
        //{
        //    var arg = new
        //    {
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //        Code = schoolCode,
        //        CityCode = citycode,
        //        DistrictCode = districtcode,
        //        UserCityCode = usercitycode,
        //        UserDistrictCode = userdistrictcode
        //    };
        //    var rows = 0;
        //    var list = _respository.GetListByStoreV2<SchoolLog>("[dbo].[Proc_SelectPaged_School]", arg);
        //    if (list.Any()) rows = list.First().TotalRowCount;
        //    totalRow = rows;
        //    return list;
        //}
    }
}
