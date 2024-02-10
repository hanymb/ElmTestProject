using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Request;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var query = _unitOfWork.BookReposatory.Get(trackable: false);

            if (!string.IsNullOrEmpty(request.BookTitle))
                query = query.Where(a => a.BookTitle.Contains(request.BookTitle));
            if (!string.IsNullOrEmpty(request.BookDescription))
                query = query.Where(a => a.BookTitle.Contains(request.BookDescription));
            if (!string.IsNullOrEmpty(request.Auther))
                query = query.Where(a => a.BookTitle.Contains(request.Auther));
            if (request.PublishDate.HasValue)
                query = query.Where(a => a.PublishDate == request.PublishDate);
            try
            {
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
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
