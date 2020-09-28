using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Something.Application;
using Something.Domain;
using Something.Persistence;
using Something.Security;
using System.Text;
using JwtConstants = Something.Security.JwtConstants;

namespace SomethingUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string DevSomething3AllowSpecificOrigins = "_devSomething3AllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: DevSomething3AllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:44380")
                                      .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                                  });
            });
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(JwtConstants.Secret);
                    var key = new SymmetricSecurityKey(secretBytes);
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = JwtConstants.Issuer,
                        ValidAudience = JwtConstants.Audience,
                        IssuerSigningKey = key
                    };
                });
            services.AddAuthorization();
            services.AddDbContext<AppDbContext>(
                options => options.UseInMemoryDatabase(nameof(Something.API))
                );
            services.AddSingleton<ISomethingFactory, SomethingFactory>();
            services.AddScoped<ISomethingCreateInteractor, SomethingCreateInteractor>();
            services.AddScoped<ISomethingReadInteractor, SomethingReadInteractor>();
            services.AddScoped<ISomethingPersistence, SomethingPersistence>();
            services.AddSingleton<ISomethingUserManager, SomethingUserManager>(); 
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseCors(DevSomething3AllowSpecificOrigins);
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
