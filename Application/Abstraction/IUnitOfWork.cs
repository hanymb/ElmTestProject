using Application.Abstraction.Reposatory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        public IBookReposatory BookReposatory { get; }
    }
}
