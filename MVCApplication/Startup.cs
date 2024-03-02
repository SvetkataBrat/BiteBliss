using DataLayer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCApplication.Managers;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ServiceLayer;

namespace MVCApplication
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
            services.AddControllersWithViews();
            services.AddRazorPages();
            
            services.AddScoped<RecipeContext, RecipeContext>();
            services.AddScoped<IdentityManager, IdentityManager>();
            services.AddScoped<IdentityContext, IdentityContext>();
            services.AddScoped<IEmailSender, EmailSenderManager>();

            services.AddDbContext<BiteBlissDBContext>(op =>
            {
                op.UseSqlServer("Server=TIMI-PC;Database=BiteBlissDB;Trusted_Connection=True;Encrypt=False");
            }, ServiceLifetime.Scoped);

            services.AddIdentity<User, IdentityRole>(iop =>
            {
                iop.Password.RequiredLength = 5;
                iop.Password.RequireNonAlphanumeric = false;
                iop.Password.RequiredUniqueChars = 0;
                iop.Password.RequireUppercase = false;
                iop.Password.RequireLowercase = false;
                iop.Password.RequireDigit = false;

                iop.User.RequireUniqueEmail = true;

                iop.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<BiteBlissDBContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.SlidingExpiration = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}