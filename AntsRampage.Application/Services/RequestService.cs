using AntsRampage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsRampage.Application.Services
{
    public class RequestService
    {
        private HttpClient _httpClient;
        private Stopwatch _sw;

        public RequestService(SocketsHttpHandler _httpClientHandler)
        {
            _httpClient = new HttpClient(_httpClientHandler);
            _sw = new Stopwatch();
        }

        public async Task<Request> Start(Request request, HttpCompletionOption httpCompletionOption)
        {
            _sw.Start();
            var requestContent = !String.IsNullOrEmpty(request.Body) ? new StringContent(request.Body) : null;
            var httpRequestMessage = new HttpRequestMessage(request.Method, request.Url) { Content = requestContent };

            await Parallel.ForEachAsync(Enumerable.Range(0, request.TotalCount), async (uri, token) =>
            {
                var ant = new Ant();
                request.Ants.Add(ant);
                await ant.StartWorking(DateTime.UtcNow);
                using (var response = await _httpClient.SendAsync(httpRequestMessage, httpCompletionOption))
                {
                    //var contentStream = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        await ant.EndWorking(true);
                    }
                    else
                    {
                        await ant.EndWorking(false);
                    }
                }
            });

            return request;
        }

        public async Task<Request> Stop(Request request)
        {

            return request;
        }
    }
}
