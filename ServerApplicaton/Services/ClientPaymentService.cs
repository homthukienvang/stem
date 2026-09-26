using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Interfaces;
namespace Services
{

    public interface IClientPaymentService
    {
        IEnumerable<ClientPayment> GetClientPaymentByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<ClientPayment> GetAllClientPayment();
        Response Create(ClientPayment entry);
        ClientPayment GetClientPaymentById(int id);
        Response Update(ClientPayment entry);
        Response Delete(int id);
    }
    public class ClientPaymentService : IClientPaymentService
	{
		/// <summary>
		/// Declare resposity
		/// </summary>
		/// <param name="psqlConn"></param>
		/// <author>louis</author>	
		private readonly ICommonRepository _respository;

        public ClientPaymentService(ICommonRepository respository)
        {
            _respository = respository;
        }
		
        /// <summary>
		/// Get all values
		/// </summary>
		/// <returns>List values</returns>
		/// <author>Louis</author>
        public IEnumerable<ClientPayment> GetAllClientPayment()
        {
            return _respository.GetListByStore<ClientPayment>("[dbo].[Proc_SelectAll_ClientPayment]");
        }
        
        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public IEnumerable<ClientPayment> GetClientPaymentByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex =  pageIndex 
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<ClientPayment>("[dbo].[Proc_SelectPaged_ClientPayment]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
        
        /// <summary>
		/// Get values by Id
		/// </summary>
		/// <returns>value by Id</returns>
		/// <author>Louis</author>
        public ClientPayment GetClientPaymentById(int id)
        {
            var arg = new
            {
                ClientPaymentId=id
            };
            return _respository.GetObjectByStoreV2<ClientPayment>("[dbo].[Proc_SelectByID_ClientPayment]", arg);
        }
        
        /// <summary>
		/// Insert a new row and return the identity
		/// </summary>
		/// <returns>new identity</returns>
		/// <author>Louis</author>
        public Response Create(ClientPayment entry)
        {
            var arg = new
            {
                ClientId = entry.ClientId,
                Amount = entry.Amount,
                PaymentDate = entry.PaymentDate,
                IsDeleted = entry.IsDeleted,
                CreatedDate = entry.CreatedDate,
                PaymentCode = entry.PaymentCode,
                Description = entry.Description,
                BeginDate = entry.BeginDate,
                EndDate = entry.EndDate,
                CreatedUserId = entry.CreatedUserId,
                CreatedUserName = entry.CreatedUserName,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_ClientPayment]", arg);
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
        public Response Update(ClientPayment entry)
        {
            var arg = new
            {
                ClientPaymentId = entry.ClientPaymentId,
                ClientId = entry.ClientId,
                Amount = entry.Amount,
                PaymentDate = entry.PaymentDate,
                IsDeleted = entry.IsDeleted,
                CreatedDate = entry.CreatedDate,
                PaymentCode = entry.PaymentCode,
                Description = entry.Description,
                BeginDate = entry.BeginDate,
                EndDate = entry.EndDate,
                CreatedUserId = entry.CreatedUserId,
                CreatedUserName = entry.CreatedUserName,
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_ClientPayment]", arg);
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
                ClientPaymentId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_ClientPayment]", arg);
        }
	}
}

