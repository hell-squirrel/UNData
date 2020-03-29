using System.Collections.Generic;
using AppService.Interfaces;
using Domain.Model;

namespace AppService.Queries
{
    public class SearchLocationDescriptionQuery: IQuery<IEnumerable<Location>>
    {
        public string Query { get;}

        public int Page { get;}

        public int PageSize { get;}

        public SearchLocationDescriptionQuery(string query, int page, int pageSize)
        {
            Query = query;
            Page = page;
            PageSize = pageSize;
        }
    }
}