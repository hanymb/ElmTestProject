using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request
{
    public class searchBookRequest :BaseFilterParameters
    {
        public string BookTitle { get; set; }
        public string BookDescription { get; set; }
        public string Auther {  get; set; }
        public DateTime? PublishDate { get; set; }
    }




    public class BaseFilterParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
       

        public List<OrderByModel> Ordering { get; set; } = new List<OrderByModel>();
    }
}
