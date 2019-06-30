/**************************************************************************
 *                                                                        *
 *  File:        Startup.cs                                               *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 *                                                                        *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Navigation.Business.Logic.Implementations;
using Navigation.Business.Logic.Interfaces;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Navigation.api
{
    /// <summary>
    /// ASP.NET Core application must include Startup class. As the name suggests, it is executed first 
    /// when the application starts. Startup class includes two public methods: ConfigureServices and Configure.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor that contains the instance of an IConfiguration object.
        /// </summary>
        /// <param name="configuration">Set of value application configuration properties.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets a configuration value.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            /* Sets the compatibility mode to ASP.NET Core 2.1. */
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            /* Service registration is necessary to support constructor injection in consuming classes. */
            services.AddScoped<IDbService<DataAccess.Collections.Cities>, DbService<DataAccess.Collections.Cities>>();
            services.AddScoped<IDbService<Distances>, DbService<Distances>>();

            services.AddTransient(typeof(ICitiesLogic), typeof(Business.Logic.Implementations.CitiesLogic));
            services.AddTransient(typeof(IDistancesLogic), typeof(DistancesLogic));


            /* Swagger option configuration: setting name and version. */
            /* Swagger allows you to describe the structure of APIs so that machines can read them. */
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        /// <summary>
        /// // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running on.</param>
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

            /* Expose the generated data. */
            /* Enable middleware to serve generated Swagger as a JSON endpoint. */
            app.UseSwagger();

            /* Expose the UI that comes with Swashbuckle. */
            /* Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint. */
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(builder => builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowCredentials()
            .AllowAnyHeader()
            );


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
