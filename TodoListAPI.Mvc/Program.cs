using Microsoft.AspNetCore.Identity;
using TodoListAPI.Mvc.Data.Identity;
using TodoListAPI.Mvc.Data.TodoList;
using TodoListAPI.Mvc.Repositories;
using Microsoft.OpenApi.Models;

namespace TodoListAPI.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.AddNpgsqlDbContext<IdentityDbContext>("identity-db");
        builder.AddNpgsqlDbContext<TodoListDbContext>("todo-list-db");

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<IdentityDbContext>();

        builder.Services.AddScoped<ITodoRepository, TodoRepository>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            var filePath = Path.Combine(AppContext.BaseDirectory, "MyApi.xml");
            c.IncludeXmlComments(filePath);
        });

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}
