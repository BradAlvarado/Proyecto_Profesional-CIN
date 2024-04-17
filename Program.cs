using Microsoft.EntityFrameworkCore;
using Sistema_CIN.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sistema_CIN.Controllers;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Variable del String de la conexion de la BD
var connectionString = builder.Configuration.GetConnectionString("connection_db");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<CIN_pruebaContext>(options =>
        options.UseSqlServer(connectionString));

CIN_pruebaContext _context;
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin",
         policy => policy.RequireRole("Administrador"));
});

// Inicializamos _context en el scope
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    _context = scope.ServiceProvider.GetRequiredService<CIN_pruebaContext>();
}

//builder.Services.AddAuthorization(options =>
//{

//    options.AddPolicy("PermitidosPorRoles", policy =>
//    {
//        policy.RequireAssertion(context =>
//        {
//            // Obtener los roles permitidos de tu método MiAccion()
//            var rolesPermitidos = ObtenerRolesPermitidos(context, _context);

//            // Verificar si el usuario actual tiene al menos uno de los roles permitidos
//            return rolesPermitidos.Any(role => context.User.IsInRole(role));
//        });
//    });

//});

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Login}/{id?}");

app.MapRazorPages();

app.Run();
