using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Request;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Types;
using EntityFrameworkCore.SqlServer.JsonExtention;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
                
        }

        public async Task<ResultOfAction<PagedListResult<BookserachResponse>>> SearchBookAsync(searchBookRequest request)
        {

            var query = _unitOfWork.BookReposatory.Searchtitle(request);       
            var pagedListinfo = new PagedListInfo(request.PageNumber, request.PageSize);
            var paginated = _unitOfWork.BookReposatory.GetPagedAsync(pagedListinfo, query);
            var queryCount = Task.FromResult(query.Count());
            await Task.WhenAll(paginated, queryCount);
            var result = new PagedListResult<BookserachResponse>()
            {
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                Result = _mapper.Map<IEnumerable<Book>, IEnumerable<BookserachResponse>>(paginated.Result),
                TotalRows = queryCount.Result
            };
            return new ResultOfAction<PagedListResult<BookserachResponse>>(HttpStatusCode.OK, null, result);

        }
    }
}
