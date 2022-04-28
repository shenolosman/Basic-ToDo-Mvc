using Microsoft.AspNetCore.Identity;
using So.ToDo.DataAccessLayer.Concrete.EntityFrameworkCore.Contexts;
using So.ToDo.DataAccessLayer.Concrete.EntityFrameworkCore.Repositories;
using So.ToDo.DataAccessLayer.Interfaces;
using SO.ToDo.BusinessLayer.Concrete;
using SO.ToDo.BusinessLayer.Interfaces;
using SO.ToDo.Entities.Concrete;
using SO.ToDo.WebAPP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IMyTaskService, MyTaskManager>();
builder.Services.AddScoped<IRapportService, RapportManager>();
builder.Services.AddScoped<IStateOfUrgentService, StateOfUrgentManager>();

builder.Services.AddScoped<IMyTaskDAL, EfMyTaskRepository>();
builder.Services.AddScoped<IStateOfUrgentDal, EfStateOfUrgentRepository>();
builder.Services.AddScoped<IRapportDal, EfRapportRepository>();

builder.Services.AddDbContext<ToDoContext>();

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ToDoContext>();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.Cookie.Name = "TodoAppCookie";
    o.Cookie.SameSite = SameSiteMode.Strict;
    o.Cookie.HttpOnly = true;
    o.ExpireTimeSpan = TimeSpan.FromDays(30);
    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    o.LoginPath = "/Home/Index";
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<AppRole>>();
    IdentityInitializer.SeedData(userManager, roleManager).Wait();
}

app.Run();
