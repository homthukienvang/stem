using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
namespace Services
{

    public interface IFeedbackService
    {
        IEnumerable<Feedback> GetFeedbackByPage(int pageIndex, int pageSize, int id, string fromdate, string todate, string fullName,string tClass, string tSchoolCode, string tLessonCode, int viewed , out int totalRow);
        IEnumerable<Feedback> GetAllFeedback();
        Response Create(Feedback entry);
        Feedback GetFeedbackById(int id);
        Response Update(Feedback entry);
        Response Delete(int id);
        Response FinishFeedback(Feedback model);

        IEnumerable<FeedbackAnswer> GetFeedbackAnswer(int feedbackid);
        Response CreateFeedbackAnswer(Feedback entry);
        IEnumerable<Feedback> GetFeedbackByClient(int clientId);
        IEnumerable<FeedbackAnswer> GetFeedbackAnswerByClient(int clientId);

        IEnumerable<FeedBackFile> GetFeedbackFile(int feedbackid);
    }
    public class FeedbackService : IFeedbackService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>louis</author>	
        private readonly ICommonRepository _respository;

        public FeedbackService(ICommonRepository respository)
        {
            _respository = respository;
        }
        public FeedbackService()
            : this(new CommonRepository(new Database()))
        {
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>Louis</author>
        public IEnumerable<Feedback> GetAllFeedback()
        {
            return _respository.GetListByStore<Feedback>("[dbo].[Proc_SelectAll_Feedback]");
        }

        public IEnumerable<Feedback> GetFeedbackByClient(int clientId)
        {
            var args = new
            {
                ClientId = clientId
            };
            return _respository.GetListByStoreV2<Feedback>("[dbo].[Proc_SelectByClient_Feedback]", args);
        }

        public IEnumerable<FeedbackAnswer> GetFeedbackAnswerByClient(int clientId)
        {
            var args = new
            {
                ClientId = clientId
            };
            return _respository.GetListByStoreV2<FeedbackAnswer>("[dbo].[Proc_SelectByClient_FeedbackAnswer]", args);
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
        /// <summary>
        /// Get all values by page
        /// </summary>
        /// <returns>List values by page</returns>
        /// <author>Louis</author>
        public IEnumerable<Feedback> GetFeedbackByPage(int pageIndex, int pageSize, 
            int clientid, string fromdate, string todate, string fullName,string tClass, string tSchoolCode, string tLessonCode, int viewed , out int totalRow)
        {
            DateTime? from = ParseDate (fromdate);
            DateTime? to = ParseDate (todate);
             
            var arg = new
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                ClientId = clientid,
                FromDate = from,
                ToDate = to,
                FullName = fullName,
                Class = tClass,
                SchoolCode= tSchoolCode,
                LessonCode= tLessonCode,
                Viewed = viewed
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<Feedback>("[dbo].[Proc_SelectPaged_Feedback]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }

        /// <summary>
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>Louis</author>
        public Feedback GetFeedbackById(int id)
        {
            var arg = new
            {
                FeedbackId = id
            };
            return _respository.GetObjectByStoreV2<Feedback>("[dbo].[Proc_SelectByID_Feedback]", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>Louis</author>
        public Response Create(Feedback entry)
        {
            var arg = new
            {
                ClientId = entry.ClientId,
                LessonId = entry.LessonId,
                Name = entry.Name ,
                Phone = entry.Phone ,
                Email = entry.Email ,
                Comment = entry.Comment ,
                FileAttachs = entry.FileAttachs
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_Feedback]", arg);
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
        public Response Update(Feedback entry)
        {
            throw new Exception();
        }

        /// <summary>
        /// Delete the exist row
        /// </summary>
        /// <author>Louis</author>
        public Response Delete(int id)
        {
            var arg = new
            {
                FeedbackId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Feedback]", arg);
        }

        public Response FinishFeedback(Feedback model)
        {
            string sql = string.Format(@"UPDATE [dbo].[Feedback] SET Viewed ={1}
                                        WHERE  FeedbackId in ({0})
                                       ", string.Join(",", model.ListId), model.Viewed == true ? "1" : "0");
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
        
        public IEnumerable<FeedbackAnswer> GetFeedbackAnswer(int feedbackid)
        {
            var arg = new
            {
                @FeedbacId = feedbackid,
            };
            return _respository.GetListByStoreV2<FeedbackAnswer>("[dbo].[Proc_SelectByFeedbackId_FeedbackAnswer]", arg);
        }

        public IEnumerable<FeedBackFile> GetFeedbackFile(int feedbackid)
        {
            var arg = new
            {
                @FeedbacId = feedbackid,
            };
            return _respository.GetListByStoreV2<FeedBackFile>("[dbo].[Proc_SelectByFeedbackId_FeedbackFile]", arg);
        }

        public Response CreateFeedbackAnswer(Feedback entry)
        {
            var arg = new
            {
                @FeedbackId = entry.FeedbackId,
                @UserId = entry.UserId,
                @UserName = entry.UserName,
                @Answer = entry.Answer
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_FeedbackAnswer]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = ""
            };
        }
    }
}

