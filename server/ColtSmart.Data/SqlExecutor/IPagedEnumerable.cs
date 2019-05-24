using System.Collections.Generic;

namespace ColtSmart.Data
{
    public interface IPagedEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Current Page Requested
        /// </summary>
        int CurrentPage { get; set; }

        /// <summary>
        /// Size of the Page Requested
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Total Matching records
        /// </summary>
        int TotalCount { get; set; }
    }
}
