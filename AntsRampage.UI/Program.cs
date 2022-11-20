using AntsRampage.Application.Services;
using AntsRampage.UI.Consumers;
using AntsRampage.UI.Data;
using MassTransit;
using MassTransit.MessageData;
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

var tempPath = Path.Combine(Path.GetTempPath(), "AntsRampage", Guid.NewGuid().ToString());
Console.WriteLine($"Temp Path: {tempPath}");
var messageDataRepository = new FileSystemMessageDataRepository(new DirectoryInfo(tempPath));

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("", true));
  

    x.AddConsumer<AntConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseMessageData(messageDataRepository);
        cfg.Host("*", "/", h =>
        {
            h.Username("*");
            h.Password("*");
        });
        cfg.ConfigureEndpoints(context);
    });
});

var sslOptions = new SslClientAuthenticationOptions { RemoteCertificateValidationCallback = delegate { return true; } };
SocketsHttpHandler _httpClientHandler = new SocketsHttpHandler()
{
    SslOptions = sslOptions,
    PooledConnectionLifetime = TimeSpan.FromMinutes(1)
};

builder.Services.AddScoped(sp => new RequestService(_httpClientHandler).UseDistributed(sp.GetRequiredService<IPublishEndpoint>()).UseMessageDataRepository(messageDataRepository));

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
