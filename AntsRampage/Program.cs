﻿// See https://aka.ms/new-console-template for more information
using AntsRampage.Application.Services;
using AntsRampage.Domain.Entities;
using System.Net.Http;
using System.Net.Security;

var sslOptions = new SslClientAuthenticationOptions { RemoteCertificateValidationCallback = delegate { return true; } };
SocketsHttpHandler _httpClientHandler = new SocketsHttpHandler()
{
    SslOptions = sslOptions,
    PooledConnectionLifetime = TimeSpan.FromMinutes(1)
};

Console.WriteLine("Hello, World!");

RequestService service = new RequestService(_httpClientHandler);
var request = new Request(HttpMethod.Get, null, "http://google.com", 1);
await service.Start(request, HttpCompletionOption.ResponseHeadersRead);





//Request r = new Request(_httpClientHandler);