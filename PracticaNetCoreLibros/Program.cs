using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PracticaNetCoreLibros.Data;
using PracticaNetCoreLibros.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddAuthentication(options =>

{

    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;

    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnetion");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<LibrosRepository>();

builder.Services
    .AddControllersWithViews(options => options.EnableEndpointRouting = false);
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

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//SI TENEMOS SEGURIDAD PERSONALIZADA DEBEMOS UTILIZAR 
//app.UseMvc 

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}"
        );
});

//app.MapControllerRoute( 
//    name: "default", 
//    pattern: "{controller=Home}/{action=Index}/{id?}" 
//    ); 

app.Run();