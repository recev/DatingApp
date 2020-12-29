using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
namespace DatingApi.Data.DataTransferObjects
{
    public class PaginatedUserList
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalUserCount { get; set; }
        public int TotalPageCount {
            get
            {
                var doubleTotalPageCount = TotalUserCount /  (double)PageSize;
                var intTotalPageCount = (int)Math.Ceiling(doubleTotalPageCount);
                return intTotalPageCount;
            }
        }

        public IList<CompactUser> Users { get; set; }
    }
}