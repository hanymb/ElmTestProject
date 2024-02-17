using Application.Abstraction.Reposatory.Base;
using Application.Request;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Reposatory
{
    public interface IBookReposatory :IRepository<Book>
    {

        IQueryable<Book> Searchtitle(searchBookRequest request);


    }
}
