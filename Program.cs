using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebMVC.Data;
using WebMVC.Models;

namespace WebMVC
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<WebMVCContext>(options =>
			    options.UseSqlServer(builder.Configuration.GetConnectionString("WebMVCContext") ?? throw new InvalidOperationException("Connection string 'WebMVCContext' not found.")));

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<WebMVCContext>().AddDefaultTokenProviders();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Auth/Login";
				options.LogoutPath = "/Auth/Logout";
				options.ExpireTimeSpan = TimeSpan.FromDays(7);
			});

			var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                SeedData.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Movies/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Movies}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
