using ColtSmart.Data;
using System.Collections.Generic;

namespace ColtSmart.Service
{
    public class PagedResult<T> : BaseResult<IEnumerable<T>>
    {
        /// <summary>
        /// Current Page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Page Size Requested
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total count of matching records
        /// </summary>
        public int TotalCount { get; set; }
    }

    public static class PagedResultExtensions
    {
        public static PagedResult<T> ToPagedResult<T>(this IPagedEnumerable<T> enumerable)
        {
            return new PagedResult<T>()
            {
                Result = enumerable,
                CurrentPage = enumerable.CurrentPage,
                PageSize = enumerable.PageSize,
                TotalCount = enumerable.TotalCount
            };
        }
    }
}
