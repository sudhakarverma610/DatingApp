using DatingApp.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DatingApp.Api.Helpers;
using AutoMapper;

namespace DatingApp.Api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            var migrationsAssembly = typeof(DataContext).GetTypeInfo().Assembly.GetName();
            var migrationsAssemblyName = migrationsAssembly.Name;

            var dbConnString = Configuration.GetConnectionString("DefaultConnection");

            void DbContextOptionsBuilder(DbContextOptionsBuilder builder)
            { 
                 builder.UseSqlServer(dbConnString, sql => sql.MigrationsAssembly(migrationsAssemblyName));
                
            }
            services.AddDbContext<DataContext>(DbContextOptionsBuilder);
             
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                builder =>
                                {
                                    builder.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod(); ;
                                });
            });

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddTransient<SeedData>();
            var assesmbly=typeof(DatingRespository).Assembly;
            services.Configure<CloudinarySetting>(Configuration.GetSection("CloudinarySetting"));
            services.AddAutoMapper(assesmbly);

            services.AddScoped<IAuthRepository,AuthRepository>();

            services.AddScoped<IDatingRespository, DatingRespository>();
            var PrivateKey ="this is a key which is used by athentication to convert a Token";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options=>{
                options.TokenValidationParameters=new TokenValidationParameters(){
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(PrivateKey)),
                    ValidateIssuer=false,
                    ValidateAudience=false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder=>{
                    builder.Run(async context=>{
                        context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                        var error=context.Features.Get<IExceptionHandlerFeature>();
                        if(error==null){
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            //  app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // app.UseMvc();
        }
    }
}
