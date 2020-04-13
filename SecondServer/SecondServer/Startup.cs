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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IItemsRepository, DataRepository>();
            string conString = Configuration["ConnectionStrings:DataAccessPostgreSqlProvider"];
            string datesConString = Configuration["ConnectionStrings:DatesConnectionString"];
            services.AddDbContext<ToDoContext>(options => options.UseNpgsql(conString));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(routes=>
            {//there routes can be specified
                routes.MapRoute(
                 name: "default",
                 template: "{controller=Home}/{action=Index}/{id?}");
            });
              
            app.UseCors((corsPolicyBuilder) =>
            {//there sites that can get information specified
                corsPolicyBuilder.AllowAnyMethod();
                corsPolicyBuilder.AllowAnyOrigin();
            });
            }
        }
  }

