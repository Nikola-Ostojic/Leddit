using Backend.Api.Controllers;
using Backend.Api.DTOs;
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
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Backend.Api.Tests.Api
{
    public class TestStartup
    {
        public IConfiguration Configuration { get; }

        public TestStartup(IConfiguration configuration)
        {
            
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            // To escape Debug/Release bin folder
            var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Name;

            var basePath = GetProjectPath(root, assembly);
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

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
            services.AddScoped<IMapper<CommentEntity, CommentResponseDTO>, CommentMapper>();
            services.AddScoped<IMapper<ThreadEntity, ThreadResponseDTO>, ThreadMapper>();
            services.AddScoped<IMapper<MovieEntity, MovieDTO>, MovieMapper>();

            // Utilities
            services.AddSingleton<JwtHandler>();

            // Database context
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("MemoryDb"));

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

            services.AddControllers().AddApplicationPart(typeof(MoviesController).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSet seedData)
        {
            // Seeding our in memory database with the test data
            var usersRepo = app.ApplicationServices.GetService<IUserRepository>();
            seedData.Users?.ForEach(u => usersRepo.Create(u));

            var moviesRepo = app.ApplicationServices.GetService<IMovieRepository>();
            seedData.Movies?.ForEach(m => moviesRepo.Create(m));

            var commentsRepo = app.ApplicationServices.GetService<ICommentRepository>();
            seedData.Comments?.ForEach(c => commentsRepo.Create(c));

            //var dbContext = app.ApplicationServices.GetService<ApplicationDbContext>();
            //dbContext.SaveChanges();
            var threadsRepo = app.ApplicationServices.GetService<IThreadRepository>();
            seedData.Threads?.ForEach(t => threadsRepo.Create(t));

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo?.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo?.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }



    }
}
