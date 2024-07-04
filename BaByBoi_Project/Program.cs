using BaByBoi.Domain.Repositories.Interface;
using BaByBoi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service;
using BaByBoi.DataAccess.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BaByBoiContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
//service
builder.Services.AddScoped(typeof(UserService));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/CustomerViewPage/CusViewProduct");
    return Task.CompletedTask;
});

app.Run();
