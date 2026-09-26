using System.Collections.Generic;

namespace AppModel.Base
{
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}