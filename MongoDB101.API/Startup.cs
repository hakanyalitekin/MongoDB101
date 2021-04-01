using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB101.API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB101.API
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

            /* appsettings.json konfig�rasyon dosyam�z�n i�erisinde ki, MongoDbSettings isminde ki section'�n al�naca��n� 
             ve bunun MongoDbSettings s�n�f�n�n property'leriyle set edilece�ini belirtiyoruz. */
            services.Configure<MongoDbSettings>(Configuration.GetSection(nameof(MongoDbSettings)));

            /* DI i�in gerekli olan, Controller'lar�n contractor'lar�nda IDbSettings interface'inin ta��yabilece�i instance'lar�n,
                ��z�mlenerek enjekte edilebilece�ini belirtiyoruz. */
            services.AddSingleton<IDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MongoDB101.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB101.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
