using Application.Abstraction;
using Application.Abstraction.Reposatory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookDbContext _db;
        private readonly IBookReposatory _bookReposatory;
        public UnitOfWork(BookDbContext db, IBookReposatory bookReposatory)
        {
            _db = db;
            _bookReposatory = bookReposatory;
        }
        public IBookReposatory BookReposatory { get => _bookReposatory; }

        async Task<int> IUnitOfWork.SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
