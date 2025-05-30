using Microsoft.EntityFrameworkCore;
using GYM_ITM.Models;
using GYM_ITM.Controllers.Observer;  // Reemplaza con el espacio de nombres correcto


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Observer Interfaces and Services
ObserverConfiguration.Configuration(builder.Services);

builder.Services.AddDbContext<DbgymContext>(options => 
    options.UseSqlServer(   builder.Configuration.GetConnectionString("conexion")));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
