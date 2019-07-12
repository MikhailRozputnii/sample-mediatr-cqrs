using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class PageInfo
    {
        public int PageNumber { get; private set; } 
        public int TotalPages { get; private set; }
    
        public PageInfo(int count, int pageSize, int pageNumber){
            PageNumber = pageNumber==0?1:pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize); 
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}
