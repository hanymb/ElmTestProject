using Application.Abstraction.Reposatory.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Reposatory
{
    public interface IBookReposatory :IRepository<Book>
    {
        Task<IQueryable<Book>> Searchtitle(string title);
    }
}
