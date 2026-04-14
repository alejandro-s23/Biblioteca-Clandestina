using Library.Data;
using Library.Data.Interfaces;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo para o "lacre" expirar
    options.Cookie.HttpOnly = true; // Segurança: impede acesso via JavaScript
    options.Cookie.IsEssential = true; // Necessário para o site funcionar
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Access/Index"; // Caminho para o portal de acesso
        options.AccessDeniedPath = "/Access/Index"; // Caminho caso não seja admin
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Tempo do selo de validade
    });
//Declarando os REPOSITORY
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRentRepository, BookRentRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

//Declarando os SERVICES
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IRequestService, RequestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Access}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();