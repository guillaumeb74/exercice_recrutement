using ExerciceRecrutementRestaurant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

// Set default directory for files and app location
System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

// Create and setup the service builder for Windows service + Kestrel WEB ASP DOT NET Server
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {

    });
// Windows service config
builder.Host.UseWindowsService(options => {
        options.ServiceName = "Exercice-Recrutement-Restaurant";
    })
    .ConfigureServices(services => {
        services.AddHostedService<Worker>();
    });

// Web server config
builder.WebHost.UseKestrel(options => {
    options.Listen(System.Net.IPAddress.Any, 8080, listenOptions => {
        });
    })
    .ConfigureLogging(builder =>
    {
        builder.AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("NToastNotify", LogLevel.Warning)
            .AddConsole();
    });

// Building the app and final config
var app = builder.Build();

app.UseExceptionHandler(err => err.UseCustomErrors(app.Environment));

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();