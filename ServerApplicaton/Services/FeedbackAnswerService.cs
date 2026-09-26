using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Repositories.Interfaces;
using Repositories.Implementations;
using System.Globalization;

namespace Services
{

    public interface IFeedbackAnswerService
    {
        IEnumerable<FeedbackAnswer> GetByFeedback(int feedbackid);
        IEnumerable<FeedbackAnswer> GetAll();
        Response Create(FeedbackAnswer entry);
        Response CreateFBA(FeedbackAnswer entry);
        Feedback GetById(int id);
        Response Update(FeedbackAnswer entry);
        Response Delete(int id);
    }

    public class FeedbackAnswerService : IFeedbackAnswerService
    {
        /// <summary>
        /// Declare resposity
        /// </summary>
        /// <param name="psqlConn"></param>
        /// <author>huanpv</author>	
        private readonly ICommonRepository _respository;

        public FeedbackAnswerService(ICommonRepository respository)
        {
            _respository = respository;
        }
        public FeedbackAnswerService()
            : this(new CommonRepository(new Database()))
        {
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>List values</returns>
        /// <author>huanpv</author>
        public IEnumerable<FeedbackAnswer> GetAll()
        {
            return _respository.GetListByStore<FeedbackAnswer>("[dbo].[Proc_SelectAll_FeedbackAnswer]");
        }

        public IEnumerable<FeedbackAnswer> GetByFeedback(int feedbackid)
        {
            var arg = new
            {
                FeedbackId = feedbackid,
            };
            return _respository.GetListByStoreV2<FeedbackAnswer>("[dbo].[Proc_SelectByFeedback_FeedbackAnswer]", arg);
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
        /// Get values by Id
        /// </summary>
        /// <returns>value by Id</returns>
        /// <author>huanpv</author>
        public Feedback GetById(int id)
        {
            var arg = new
            {
                FBAnswerId = id
            };
            return _respository.GetObjectByStoreV2<Feedback>("[dbo].[Proc_SelectByID_FeedbackAnswer]", arg);
        }

        /// <summary>
        /// Insert a new row and return the identity
        /// </summary>
        /// <returns>new identity</returns>
        /// <author>huanpv</author>
        public Response Create(FeedbackAnswer entry)
        {
            var arg = new
            {
                @FeedbackId = entry.FeedbackId,
                @UserId = entry.UserId,
                @UserName = entry.UserName,
                @Answer = entry.Answer,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_FeedbackAnswer]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = ""
            };
        }


        public Response CreateFBA(FeedbackAnswer entry)
        {
            var arg = new
            {
                @FeedbackId = entry.FeedbackId,
                @UserId = entry.UserId,
                @UserName = entry.UserName,
                @Answer = entry.Answer,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_FeedbackAnswer]", arg);
            return new Response()
            {
                Success = Convert.ToInt32(id) > 0,
                Message = ""
            };
        }
        /// <summary>
        /// Update the exist row
        /// </summary>
        /// <author>huanpv</author>
        public Response Update(FeedbackAnswer entry)
        {
            throw new Exception();
        }

        /// <summary>
        /// Delete the exist row
        /// </summary>
        /// <author>huanpv</author>
        public Response Delete(int id)
        {
            var arg = new
            {
                FBAnswerId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_FeedbackAnswer]", arg);
        }
    }
}

