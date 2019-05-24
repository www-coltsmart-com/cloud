﻿using System;
using System.Collections.Generic;

namespace ColtSmart.Data
{
    public class PagedList<T> : List<T>, IPagedEnumerable<T>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public PagedList(IEnumerable<T> source, int currentPage, int pageSize, int totalCount)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", "pageSize must be greater than zero");

            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;

            AddRange(source);
        }

        public PagedList()
        {

        }

    }
}
