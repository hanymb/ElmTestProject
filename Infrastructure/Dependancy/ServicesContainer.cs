using Application.Abstraction;
using Application.Abstraction.Reposatory;
using Application.Abstraction.Services;
using Application.Implementation;
using Infrastructure.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependancy
{
    public static class ServicesContainer
    {
        public static void AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<IBookReposatory,BookReposatory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<BookDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
