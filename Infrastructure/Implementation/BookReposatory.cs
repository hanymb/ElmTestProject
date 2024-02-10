using Application.Abstraction.Reposatory;
using Domain.Entities;
using Infrastructure.Implementation.Base;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IQueryable<Book>> Searchtitle(string title)
        {
          var query=_db.Books.Where(b=>EF.Functions.Like(b.BookInfo,$"%{title}%"));
            return query;
        }
    }
}
