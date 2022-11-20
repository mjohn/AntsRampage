using AntsRampage.Application.Services;
using AntsRampage.UI.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Security;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


var sslOptions = new SslClientAuthenticationOptions { RemoteCertificateValidationCallback = delegate { return true; } };
SocketsHttpHandler _httpClientHandler = new SocketsHttpHandler()
{
    SslOptions = sslOptions,
    PooledConnectionLifetime = TimeSpan.FromMinutes(1)
};

builder.Services.AddScoped(sp => new RequestService(_httpClientHandler));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
