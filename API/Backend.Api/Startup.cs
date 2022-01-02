using Backend.Api.DTOs;
using Backend.Api.DTOs.Request;
using Backend.Api.DTOs.Response;
using Backend.Api.Mappers;
using Backend.Api.Security;
using Backend.Api.Validation;
using Backend.Core.Interfaces;
using Backend.Core.Services;
using Backend.DAL;
using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Backend.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Api
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
            // Registration our interfaces to concrete implementation, i.e. dependency injection
            // Repositories
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IThreadRepository, ThreadRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Services
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IThreadService, ThreadService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            // Mappers
            services.AddScoped<IMapper<ThreadEntity, ThreadResponseDTO>, ThreadMapper>();
            services.AddScoped<IMapper<CommentEntity, CommentResponseDTO>, CommentMapper>();
            services.AddScoped<IMapper<MovieEntity, MovieDTO>, MovieMapper>();

            // Utilities
            services.AddSingleton<JwtHandler>();

            // Database context
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(Configuration["Connection"], b => b.MigrationsAssembly("Backend.DAL")));

            // Add in built health check
            services.AddHealthChecks();

            // Adding our filter which validates incoming DTOs, based on the IValidatableObject.Validate method
            services.Configure<MvcOptions>(x => x.Conventions.Add(new ModelStateValidatorConvention()));

            // Mapping configuration to strongly typed type
            services.Configure<JwtSettings>(Configuration.GetSection("JWTSettings"));
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JWTSettings:SecretKey").Value)),
                    ValidIssuer = Configuration.GetSection("JWTSettings:Issuer").Value,
                    ValidAudience = Configuration.GetSection("JWTSettings:Audience").Value,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options => options.AddPolicy("User", policy => policy.RequireRole("User")));
            services.AddAuthorization(options => options.AddPolicy("Admin", policy => policy.RequireRole("Admin")));

            // Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Authorization",
                                In = ParameterLocation.Header,
                            },
                        new List<string>()
                    }
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHealthChecks("/api/health");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
