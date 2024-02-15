using Application.Abstraction.Reposatory;
using Domain.Entities;
using Infrastructure.Implementation.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class BookReposatory : Repository<Book>, IBookReposatory
    {
        public BookReposatory(BookDbContext db) : base(db)
        {
        }

        public async Task<IQueryable<Book>> Searchtitle(IQueryable<Book> query, string title)
        {
            query.Where(q => JsonConvert.DeserializeObject<BookInfoDto>(q.BookInfo).BookTitle .Contains(title));
            return query;
          //var query=_db.Books.Where(b=>EF.Functions.Like(b.BookInfo,$"%{title}%"));
           
          //  return query;
        }
    }
}
