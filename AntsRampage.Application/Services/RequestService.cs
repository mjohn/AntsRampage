﻿
using AntsRampage.Application.Messages;
using AntsRampage.Domain.Entities;
using MassTransit;
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
        public TimeSpan Elapsed => _sw.Elapsed;

        private bool _useMassTransit;
        private IPublishEndpoint _endpoint;
        private IMessageDataRepository _messageDataRepository;
        public RequestService(SocketsHttpHandler _httpClientHandler)
        {
            _httpClient = new HttpClient(_httpClientHandler);
            _sw = new Stopwatch();
        }

        public RequestService UseDistributed(IPublishEndpoint endpoint)
        {
            _useMassTransit = true;
            _endpoint = endpoint;
            return this;
        }
        public RequestService UseMessageDataRepository(IMessageDataRepository messageDataRepository)
        {
            _messageDataRepository = messageDataRepository;
            return this;
        }

        public async Task<Request> Start(Request request, bool getResponseBody, int maxParallelOperations)
        {
            _sw.Start();

            var httpCompletionOption = getResponseBody ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead;

            var requestContent = !String.IsNullOrEmpty(request.Body) ? new StringContent(request.Body) : null;

            await Parallel.ForEachAsync(Enumerable.Range(0, request.TotalCount), new ParallelOptions() { MaxDegreeOfParallelism = maxParallelOperations }, async (uri, token) =>
            {
                var httpRequestMessage = new HttpRequestMessage(request.Method, request.Url) { Content = requestContent };

                var ant = new Ant();
                request.Ants.Add(ant);
                await ant.StartWorking(DateTime.UtcNow);
                string responseBody = null;
                try
                {
                    using (var response = await _httpClient.SendAsync(httpRequestMessage, httpCompletionOption))
                    {
                        if (getResponseBody)
                        {
                            responseBody = await response.Content.ReadAsStringAsync();
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            await ant.EndWorking(true, responseBody);
                        }
                        else
                        {
                            await ant.EndWorking(false, responseBody);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await ant.EndWorking(false, ex.ToString());
                }
                finally
                {
                    if (_useMassTransit)
                    {
                        await _endpoint?.Publish(new AntWorkCompleted()
                        {
                            Id = ant.Id,
                            RequestedAt = ant.RequestedAt,
                            Succeed = ant.Succeed,
                            ResponseBody = await _messageDataRepository.PutString(ant.ResponseBody)
                        });
                    }
                }
            });
            _sw.Stop();
            return request;
        }

        public async Task<Request> Stop(Request request)
        {

            return request;
        }
    }
}
