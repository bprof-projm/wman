using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wman.Data;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Logic.Services;
using Wman.Repository.Classes;
using Wman.Repository.Interfaces;
using Wman.WebAPI.Helpers;
//using System.Data.Entity.Database;

namespace Wman.WebAPI
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
            string signingKey = Configuration.GetValue<string>("SigningKey");
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddControllers(x => x.Filters.Add(new ApiExceptionFilter()));
            services.AddTransient<IAuthLogic, AuthLogic>();
            services.AddTransient<ICalendarEventLogic, CalendarEventLogic>();
            services.AddTransient<IEventLogic, EventLogic>();
            services.AddTransient<IUserLogic, UserLogic>();
            services.AddTransient<DBSeed, DBSeed>();
            services.AddTransient<ILabelLogic, LabelLogic>();
            services.AddTransient<IAllInWorkEventLogic, AllInWorkEventLogic>();
            services.AddTransient<IPhotoLogic, PhotoLogic>();
            services.AddControllers().AddJsonOptions(options =>
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            //services.AddSingleton(Configuration);
#if DEBUG
            services.AddSingleton<IAuthorizationHandler, AllowAnonymous>(); //Uncommenting this will disable auth, for debugging purposes.
#endif


            services.AddTransient<IWorkEventRepo, WorkEventRepo>();
            services.AddTransient<IPicturesRepo, PicturesRepo>();
            services.AddTransient<ILabelRepo, LabelRepo>();
            services.AddTransient<IAddressRepo, AddressRepo>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddSwaggerGen(c =>
            {
                //c.DescribeAllEnumsAsStrings();
                // configure SwaggerDoc and others
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wman.WebAPI", Version = "v1" });
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.IncludeXmlComments(XmlCommentsFilePath);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {securityScheme, new string[] { }}
                });
            });
            string appsettingsConnectionString = Configuration.GetConnectionString("wmandb");

            services.AddDbContext<wmanDb>(options => options
#if DEBUG
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#else

#endif
            .UseSqlServer(appsettingsConnectionString, b => b.MigrationsAssembly("Wman.WebAPI")));

            services.AddIdentityCore<WmanUser>(
                     option =>
                     {
                         option.Password.RequireDigit = false;
                         option.Password.RequiredLength = 6;
                         option.Password.RequireNonAlphanumeric = false;
                         option.Password.RequireUppercase = false;
                         option.Password.RequireLowercase = false;
                     }
                 ).AddRoles<WmanRole>()
                 .AddRoleManager<RoleManager<WmanRole>>()
                 .AddSignInManager<SignInManager<WmanUser>>()
                 .AddRoleValidator<RoleValidator<WmanRole>>()
                 .AddEntityFrameworkStores<wmanDb>()
                 .AddDefaultTokenProviders();


            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://www.security.org",
                    ValidIssuer = "http://www.security.org",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                };
            });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                  });
            });

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);


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

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wman.WebAPI v1");

            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
