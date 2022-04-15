
using SO.ToDo.Web.Contraints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapControllerRoute(
    "MyRoute",
    "myroute/{language}",
    new { controller = "Home", action = "Index" },
    new { language = new MyRouteConstraint() }
    );


//To call all areas 
app.MapControllerRoute(
    "areas",
    "{area}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();
