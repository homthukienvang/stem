using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repositories.Implementations;
using Repositories.Interfaces;
namespace Services
{

    public interface ISubjectService
    {
        IEnumerable<Subject> GetSubjectByPage(int pageIndex, int pageSize, out int totalRow);
        IEnumerable<Subject> GetAllSubject();
        Response Create(Subject entry);
        Subject GetSubjectById(int id);
        Response Update(Subject entry);
        Response Delete(int id);
        IEnumerable<Subject> GetUpdateSubjects(int version);
        IEnumerable<Subject> GetUpdateSubjectsV2(int version);
    }
    public class SubjectService : ISubjectService
	{
		/// <summary>
		/// Declare resposity
		/// </summary>
		/// <param name="psqlConn"></param>
		/// <author>louis</author>	
		private readonly ICommonRepository _respository;

        public SubjectService(ICommonRepository respository)
        {
            _respository = respository;
        }
        public SubjectService()
            : this(new CommonRepository(new Database()))
        {
        }
		
        /// <summary>
		/// Get all values
		/// </summary>
		/// <returns>List values</returns>
		/// <author>Louis</author>
        public IEnumerable<Subject> GetAllSubject()
        {
            return _respository.GetListByStore<Subject>("[dbo].[Proc_SelectAll_Subject]");
        }
        
        /// <summary>
		/// Get all values by page
		/// </summary>
		/// <returns>List values by page</returns>
		/// <author>Louis</author>
        public IEnumerable<Subject> GetSubjectByPage(int pageIndex, int pageSize, out int totalRow)
        {
            var arg = new
            {
                PageSize = pageSize,
                PageIndex =  pageIndex 
            };
            var rows = 0;
            var list = _respository.GetListByStoreV2<Subject>("[dbo].[Proc_SelectPaged_Subject]", arg);
            if (list.Any()) rows = list.First().TotalRowCount;
            totalRow = rows;
            return list;
        }
        
        /// <summary>
		/// Get values by Id
		/// </summary>
		/// <returns>value by Id</returns>
		/// <author>Louis</author>
        public Subject GetSubjectById(int id)
        {
            var arg = new
            {
                SubjectId=id
            };
            return _respository.GetObjectByStoreV2<Subject>("[dbo].[Proc_SelectByID_Subject]", arg);
        }
        
        /// <summary>
		/// Insert a new row and return the identity
		/// </summary>
		/// <returns>new identity</returns>
		/// <author>Louis</author>
        public Response Create(Subject entry)
        {
            var arg = new
            {
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                Description = entry.Description,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                IsLocked = entry.IsLocked,
                CreatedDate = entry.CreatedDate,
                IsDeleted = entry.IsDeleted,
            };
            var id = _respository.ExcuteStoreGetValueV2("[dbo].[Proc_Insert_Subject]", arg);
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
        public Response Update(Subject entry)
        {
            var arg = new
            {
                SubjectId = entry.SubjectId,
                Code = entry.Code,
                Name = entry.Name,
                Order = entry.Order,
                Description = entry.Description,
                NormalImage = entry.NormalImage,
                HoverImage = entry.HoverImage,
                SelectedImage = entry.SelectedImage,
                IsLocked = entry.IsLocked,   
                IsDeleted = entry.IsDeleted,
            };
            var obj = _respository.ExcuteStoreV2("[dbo].[Proc_Update_Subject]", arg);
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
                SubjectId = id,
            };
            return _respository.ExcuteStoreV2("[dbo].[Proc_DeleteByID_Subject]", arg);
        }

        public IEnumerable<Subject> GetUpdateSubjects(int version)
        {
            var arg = new
            {
                Version = version,
            };
            return _respository.GetListByStoreV2<Subject>("[dbo].[Proc_GetUpdateSubjects]", arg);
        }

        public IEnumerable<Subject> GetUpdateSubjectsV2(int version)
        {
            var arg = new
            {
                Version = version,
            };
            return _respository.GetListByStoreV2<Subject>("[dbo].[Proc_GetUpdateSubjectsV2]", arg);
        }
	}
}

