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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string conString = Configuration["ConnectionStrings:DataAccessPostgreSqlProvider"];
            services.AddDbContext<ToDoContext>(options => options.UseNpgsql(conString));
            services.AddTransient<IItemsRepository, DataRepository>();
          // services.AddTransient<IChangesRepository, HistoryRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

           app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
  }