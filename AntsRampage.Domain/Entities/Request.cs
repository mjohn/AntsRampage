using AntsRampage.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsRampage.Domain.Entities
{
    public class Request : IAggregateRoot
    {
        public HttpMethod Method { get; private set; }
        public string Body { get; private set; }
        public string Url { get; private set; }
        public int TotalCount { get; private set; }
        public List<Ant> Ants { get; private set; }
        public Request(HttpMethod method,  string body, string url, int totalCount)
        {
            Method = method;
            Body = body;
            Url = url;
            TotalCount = totalCount;

            Ants = new List<Ant>();
        }

        public void AddAnt(Ant ant)
        {
            Ants.Add(ant);
        }

    }
}
