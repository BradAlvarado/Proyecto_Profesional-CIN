using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sistema_CIN.Services;


var builder = WebApplication.CreateBuilder(args);

// Variable del String de la conexion de la BD
var connectionString = builder.Configuration.GetConnectionString("connection_db");



builder.Services.AddDbContext<SistemaCIN_dbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(); // Habilitar el registro de datos sensibles
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddDbContext<SistemaCIN_dbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin",
         policy => policy.RequireRole("Administrador"));
});

builder.Services.AddScoped<FiltrosPermisos>();



builder.Services 
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
      
        options.LoginPath = "/Cuenta/Login";
        options.AccessDeniedPath = "/Cuenta/AccessDenied";
        options.LogoutPath = "/Home/Logout";
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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Login}/{id?}");

app.MapRazorPages();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        System.IO.Path.Combine(app.Environment.ContentRootPath, "wwwroot/images/imageUser")),
    RequestPath = "/fotos" // Ruta para acceder a los archivos cargados
});

IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa/Windows");

app.Run();
