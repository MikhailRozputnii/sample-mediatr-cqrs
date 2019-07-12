using Microsoft.AspNetCore.Mvc.Razor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class IndexViewModel
    {
        public int PageSize = 3;
        public string Message { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
        public PageInfo PageInfo { get; set; }
        public string Search { get; set; }
        public string SortOrder { get; set; }
        private bool isAscending;
        public bool IsAscending {
            set {
                if (value)
                {
                    value = false;
                }
                else value = true;

                isAscending = value;
            }
            get
            {
                return isAscending;
            }
        }
        public int CurrentPage { get; set; }
    }
}
