using System;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Services
{

    public interface IAdvertisementService
    {
        Advertisement GetAdvById(int id);
        Response Update(Advertisement entry);
        IEnumerable<Advertisement> GetAllAdvertisment();
        Response UpdateCount(int clientid, int advid);
        IEnumerable<AdvReport> GetAdvByPage(int pageIndex, int reporttype, string searchvalue, string fromdate, string todate, int advId, int pageSize, out int totalRow);
    }
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ICommonRepository _respository;

        public AdvertisementService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public AdvertisementService()
            : this(new CommonRepository(new Database()))
        {
        }

        public IEnumerable<Advertisement> GetAllAdvertisment()
        {
            return _respository.GetListByStore<Advertisement>("[dbo].[Proc_SelectAll_Advertisement]");
        }

        public Advertisement GetAdvById(int id)
        {
            var arg = new
            {
                Id = id
            };
            return _respository.GetObjectByStoreV2<Advertisement>("[dbo].[Proc_SelectByID_Advertisement]", arg);
        }

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
                entry.Flag
            };

            var obj = _respository.ExcuteStoreV2(@"[dbo].[Proc_Update_Advertisement]", arg);

            return new Response
            {
                Success = obj.Success,
                Message = obj.Message
            };
        }

        public Response UpdateCount(int clientid, int advid)
        {
            var arg = new
            {
                ClientId = clientid,
                AdvId = advid
            };

            var obj = _respository.ExcuteStoreV2(@"[dbo].[Proc_Update_AdvertisementCount]", arg);

            return new Response
            {
                Success = obj.Success,
                Message = obj.Message
            };
        }

        private static DateTime? ParseDate(string input)
        {
            DateTime result;
            if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            return null;
        }

        public IEnumerable<AdvReport> GetAdvByPage(int pageIndex, int reporttype, string searchvalue, string fromdate,string todate, int advId, int pageSize, out int totalRow)
        {
            DateTime? from = ParseDate(fromdate);
            DateTime? to = ParseDate(todate);

            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                ReportType = reporttype,
                SearchValue = searchvalue,
                AdvId = advId,
                DateFrom = from,
                DateTo = to
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<AdvReport>("[dbo].[Proc_SelectPaged_AdvertismentReport]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
    }
}
