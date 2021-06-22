using Messanger.Bll.Generators;
using Messanger.Bll.Generators.Impl;
using Messanger.Bll.Helpers;
using Messanger.Bll.Hub;
using Messanger.Bll.Services;
using Messanger.Bll.Services.Impl;
using Messanger.Dal.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Owin;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatApi
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
            //services.AddDbContext<DatabaseContext>(
            //    options => options.UseNpgsql(Configuration.GetConnectionString("DatabaseContext"),
            //        x => x.MigrationsAssembly("Messanger.Dal")));

            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=password",
                    x => x.MigrationsAssembly("Messanger.Dal")));

            ConfigurationServices.ConfigureServices(services, Configuration);

            services.AddTransient<IHashGenerator, HashGenerator>(provider =>
                new HashGenerator("sdfsdf232i8iduvsnh2j1oscakfc249dsv1"));
            services.AddTransient<IJwtGenerator, JwtGenerator>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IRoomService, RoomService>();

            services.AddControllers();

            services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });

            var documenation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            var documentationPath = Path.Combine(AppContext.BaseDirectory, documenation);

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                },
                            },
                            new string[0]
                        }
                    });

                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",
                        Name = "Authorization",
                        Description = "Bearer + JWT token",
                        BearerFormat = "JWT"
                    });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Messanger", Version = "v1" });

                if (File.Exists(documentationPath))
                    c.IncludeXmlComments(documenation, includeControllerXmlComments: true);
            });

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                }));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration.GetSection("AuthKey").GetValue<string>("Secret"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true

                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chathub")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApi v1"));
            }

            app.UseMiddleware<LoggerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessangerHub>("/chathub");
            });
        }
    }
}
