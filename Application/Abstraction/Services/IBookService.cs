using Application.Request;
using Application.Response;
using Domain.Models;
using Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IBookService
    {
        Task<ResultOfAction<PagedListResult<BookserachResponse>>> SearchBookAsync(searchBookRequest request);
    }
}
