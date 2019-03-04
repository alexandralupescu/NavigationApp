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

            /* service registration is necessary to support constructor injection in consuming classes. */
            services.AddScoped<IDbService<Cities>, DbService<Cities>>();
            services.AddScoped<IDbService<Distances>, DbService<Distances>>();

            services.AddTransient(typeof(ICitiesLogic), typeof(CitiesLogic));
            services.AddTransient(typeof(IDistancesLogic), typeof(DistancesLogic));


            /* Swagger option configuration: setting name and version */
            /* Swagger allows you to describe the structure of APIs so that machines can read them */
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            /* expose the generated data */
            /* enable middleware to serve generated Swagger as a JSON endpoint. */
            app.UseSwagger();

            /* expose the UI that comes with Swashbuckle */
            /* Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
             specifying the Swagger JSON endpoint. */
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(builder => builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowCredentials()
            );


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
