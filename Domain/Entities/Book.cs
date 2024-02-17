using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Book
    {
        [Key]
        public Int64 BookId { get; set; }
        public string BookInfo { get; set; }
        public DateTime LastModified { get; set; }
        //[NotMapped]
        //public BookInfoDto BookInfoDto { get; set; }

        private JsonDocument _BookInfojsonobject;
        [NotMapped]
        private JsonDocument JsonObject
        {
            get
            {
                if (_BookInfojsonobject == null)
                {
                    _BookInfojsonobject = JsonDocument.Parse(this.BookInfo);
                }
                return _BookInfojsonobject;
            }
        }
        public string BookTitle => JsonObject.RootElement.GetProperty("BookTitle").GetString();
        public string BookDescription => JsonObject.RootElement.GetProperty("BookDescription").GetString();
        public string Auther => JsonObject.RootElement.GetProperty("Author").GetString();
        public DateTime PublishDate => JsonObject.RootElement.GetProperty("PublishDate").GetDateTime();     
        public string Cover => JsonObject.RootElement.GetProperty("CoverBase64").GetString();
    }

    public class BookInfoDto
    {
        public string BookTitle {get; set; }
        public string BookDescription { get; set; }
        public string Auther { get; set; }
        public DateTime PublishDate { get; set; }
        public string Cover { get; set; }


    }
}
