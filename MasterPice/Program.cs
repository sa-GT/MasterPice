using MasterPice.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterPice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<MyDbContext>(options =>
	        options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));

			builder.Services.AddDistributedMemoryCache();

			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromHours(10);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
			app.UseSession();

			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=GuestFlow}/{action=Home}/{id?}");
            app.Run();
        }
    }
}
