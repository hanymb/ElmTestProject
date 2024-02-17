using Application.Response;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookserachResponse>().AfterMap<LocalizationProcessor<Book, BookserachResponse>>();
        }
    }

    public class LocalizationProcessor<T1, T2> : IMappingAction<T1, T2> where T1 : class where T2 : class
    {
        public void Process(T1 source, T2 destination, ResolutionContext context)
        {
            if(source is Book && destination is BookserachResponse) {
            MapBookToBookSearchResponse(source, destination);
            
            }
        }

        private void MapBookToBookSearchResponse(T1 source, T2 destination)
        {
            var mapedSource = source as Book;

            var mapedDestination = destination as BookserachResponse;
            mapedDestination.Auther=mapedSource.Auther;
            mapedDestination.PublishDate = mapedSource.PublishDate;
            mapedDestination.BookDescription = mapedSource.BookDescription;
            mapedDestination.BookTitle = mapedSource.BookTitle;
            mapedDestination.CoverImage = GetCoverBase64String(mapedSource.Cover);
            mapedDestination.BookId = mapedSource.BookId;
            mapedDestination.LastModified = mapedSource.LastModified;

            
        }

        private string GetCoverBase64String(string coverImage)
        {
            string x = "data:image/jpeg;base64,";
            return coverImage.Remove(0, x.Length);
        }
    }
}

