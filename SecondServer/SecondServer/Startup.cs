using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecondServer.Middleware;
using SecondServer.Models;
using SecondServer.Abstractions;
namespace SecondServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

         public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string conString = Configuration["ConnectionStrings:DataAccessPostgreSqlProvider"];
            services.AddDbContext<ToDoContext>(options => options.UseNpgsql(conString));
            services.AddTransient<IItemsRepository, DataRepository>();
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        //   app.UseMiddleware<AuthCheck>();
            app.UseHttpsRedirection();
           
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
  }