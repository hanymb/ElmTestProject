using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Application.Abstraction.Reposatory;
using Application.Abstraction.Services;
using Application.Implementation;

namespace Application.Dependancy
{
    public static class ServicesContainer
    {
        public static void AddApplictionServices( this IServiceCollection services,IConfiguration configuration  )
        {
            services.AddAutoMapper(typeof(Application.MappingProfile.MappingProfile));
            services.AddTransient<IBookService, BookService>();
        }
    }
}
