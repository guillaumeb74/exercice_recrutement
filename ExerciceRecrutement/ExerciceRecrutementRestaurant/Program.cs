using ExerciceRecrutementRestaurant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

// Set default directory for files to app location and load config
System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
//ConfigManager.LoadConfig();

// Create and setup the service builder for Windows service + Kestrel WEB ASP DOT NET Server
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddNewtonsoftJson(options =>
    {
        //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // Used to specify a default custom json serializer converter - instead we just use JObject then use custom serialization after
        //options.SerializerSettings.Converters.Add(new RequestPostModelConverter()); 
    });
// Windows service config
builder.Host.UseWindowsService(options => {
        options.ServiceName = "D4E-DCS-LoadBalancer-Service";
    })
    .ConfigureServices(services => {
        services.Configure<HostOptions>(options =>
        {
            //Service Behavior in case of exceptions - defautls to StopHost
            //options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            //Host will try to wait 30 seconds before stopping the service. 
            //options.ShutdownTimeout = TimeSpan.FromSeconds(30);
        });
        services.AddHostedService<Worker>();
    });

// Https certificate for web server
/*var certificate = new X509Certificate2(
    Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "cert.pfx"),
    "Odoo@1228"
);*/
// Web server config
builder.WebHost.UseKestrel(options => {
    options.Listen(System.Net.IPAddress.Any, /*ConfigManager.config.loadBalancerServerPort*/8080, listenOptions => {
        /*var connectionOptions = new HttpsConnectionAdapterOptions();
        connectionOptions.ServerCertificate = certificate;

        listenOptions.UseHttps(connectionOptions);*/
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

// A ETUDIER
//app.UseWebSockets();

app.UseExceptionHandler(err => err.UseCustomErrors(app.Environment));

// Configure the HTTP request pipeline.
/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}*/

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();