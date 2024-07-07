using Application.SalesFolder.Handlers;
using Application.SalesFolder.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
        services.AddTransient<IGetSalesByDateQueryHandler, GetSalesByDateQueryHandler>();
        services.AddTransient<Application.Interfaces.ISaleRepository, Infrastructure.Repositories.SaleRepository>();


        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });
        var connectionString = Configuration.GetConnectionString("PostgresConnection");

        services.AddDbContext<MyDbContext>(options =>
            options.UseNpgsql(connectionString,
                npgsqlOptions => npgsqlOptions.UseNetTopologySuite()));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}