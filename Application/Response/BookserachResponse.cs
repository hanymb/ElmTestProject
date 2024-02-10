using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Application.Response
{
    public class BookserachResponse
    {
        public Int64 BookId { get; set; }       
        

        public string BookTitle {  get; set; }
        public string BookDescription {  get; set; }
        public string Auther {  get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastModified { get; set; }
      //  public string BookInfo { get; set; }

    }
}
