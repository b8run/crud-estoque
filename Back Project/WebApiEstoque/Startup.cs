using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApiEstoque.Models;
using System;
using Microsoft.Extensions.Logging;

namespace WebApiEstoque
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
            // Passando a vers?o do MySql
            var serverVersion = new MySqlServerVersion(new System.Version(8,0,27));
            // Pegando a string de conex?o do banco
            var connection = Configuration["ConnectionMySQL:MySqlConnectionString"];

            services.AddControllers();
            services.AddDbContext<ProdutoContext>(
                opt => opt
                .UseMySql(connection, serverVersion)
                 .UseMySql(connection, serverVersion)
                // Configura??es de log(mensagens de erro em sequ?ncia)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableDetailedErrors()
                );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiEstoque", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiEstoque v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
