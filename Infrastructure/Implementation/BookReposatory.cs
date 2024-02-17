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
using EFCoreExtensions;
using EntityFrameworkCore.SqlServer.JsonExtention;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Application.Request;
namespace Infrastructure.Implementation
{
    public class BookReposatory : Repository<Book>, IBookReposatory
    {
        public BookReposatory(BookDbContext db) : base(db)
        {
        }

        public IQueryable<Book> Searchtitle(searchBookRequest request)
        {
            var query = _db.Books.FromSqlRaw(
             "SELECT * FROM Book " +
             "WHERE (JSON_VALUE(BookInfo, '$.BookTitle') LIKE '%' + {0} + '%' OR {0} IS NULL) "+
             "AND (JSON_VALUE(BookInfo, '$.Author') LIKE '%' + {1} + '%' OR {1} IS NULL) " +
             "AND (JSON_VALUE(BookInfo, '$.BookDescription') LIKE '%' + {2} + '%' OR {2} IS NULL) "+
             "AND (PARSE(JSON_VALUE(BookInfo, '$.PublishDate')as date) = {3} OR {3} IS NULL)  ",
             request.BookTitle,request.Auther,request.BookDescription,request.PublishDate);
            return query;
          //var query=_db.Books.Where(b=>EF.Functions.Like(b.BookInfo,$"%{title}%"));           
          //  return query;
        }





    }
   





}
    