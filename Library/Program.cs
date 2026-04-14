using Library.Data;
using Library.Data.Interfaces;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Configurando logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

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
try
{
    Log.Information("Starting up");
    var app = builder.Build();
    
    app.UseSerilogRequestLogging();

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
    
    // --- BLOCO DE MIGRAÇÃO AUTOMÁTICA ---
    // Executa apenas se não estiver em modo de Design (geração de scripts)
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<LibraryContext>();
        
        // Aplica migrações pendentes no banco de produção
        // Isso resolve o seu problema de criar as tabelas no MonsterASP
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    // -------------------------------------

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during startup");
}
finally
{
    Log.CloseAndFlush();
}