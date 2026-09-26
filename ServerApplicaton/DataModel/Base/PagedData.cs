using System.Collections.Generic;

namespace DataModel.Base
{
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}