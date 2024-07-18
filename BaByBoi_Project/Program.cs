using BaByBoi.Domain.Repositories.Interface;
using BaByBoi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service;
using BaByBoi.DataAccess.Service.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using BaByBoi.DataAccess.DTOs;
using BaByBoi.DataAccess.Service.VNpayService;
using FUMiniHotelManagement.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BaByBoiContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IProductSizeRepository, ProductSizeRepository>();

//service
builder.Services.AddScoped(typeof(UserService));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVnpayService, VnpayService>();

builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IOrderService, OrderSerivce>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSignalR();

// config send email
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IEmailService, EmailService>();

// config google
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    googleOptions.ClientId = googleAuthNSection["ClientId"];
    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
    googleOptions.CallbackPath = "/dang-nhap-google";
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<SignalrServer>("/signalrServer");

app.MapRazorPages();
app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/CustomerViewPage/CusViewProduct");
    return Task.CompletedTask;
});

app.Run();
