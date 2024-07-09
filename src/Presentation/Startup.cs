using System.Reflection;
using Application.Interfaces;
using Application.Requests.Handlers;
using Application.Requests.Interfaces;
using Application.Sales.Handlers;
using Application.Services.Async;
using Application.Services.Sync;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ProJect;

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
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost"; // Replace with your Redis server's IP or hostname if it's not running on the same machine
            options.InstanceName = "instance"; // Replace with a unique name for your cache instance
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = "830744935664-ck2vtk52s4tbthqfn6m50u12n4ueqq67.apps.googleusercontent.com";
                options.ClientSecret = "<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";
            });
        services.AddTransient<IGetSalesByDateQueryHandler, GetSalesByDateQueryHandler>();
        services.AddTransient<ISaleRepository, Infrastructure.Repositories.SaleRepository>();
        services.AddTransient<IGetSalesByGoodsQueryHandler, GetSalesByGoodsQueryHandler>();
        services.AddTransient<IGetSalesByClientsQueryHandler, GetSalesByClientsQueryHandler>();
        services.AddTransient<ICreateOrderQueryHandler, CreateOrderQueryHandler>();
        services.AddTransient<IDeleteOrderQueryHandler, DeleteOrderQueryHandler>();
        //services.AddTransient<IInventoryServiceSync, InventoryServiceSync>();
        //services.AddTransient<IInventoryServiceAsync, InventoryServiceAsync>();




        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesParser", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth"),
                        TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID" },
                            { "profile", "Profile" },
                            { "email", "Email" }
                        }
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new string[] { }
                }
            });
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

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesParser V1");
            c.OAuthClientId("830744935664-ck2vtk52s4tbthqfn6m50u12n4ueqq67.apps.googleusercontent.com");
            c.OAuthAppName("SalesParser");
        });


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}