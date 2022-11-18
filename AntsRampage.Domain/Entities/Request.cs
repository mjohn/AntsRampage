using AntsRampage.Domain.Common;
using AntsRampage.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsRampage.Domain.Entities
{
    public class Request : IAggregateRoot
    {
        public Method Method { get; private set; }
        public string Body { get; private set; }
        public string Url { get; private set; }
        public List<Ant> Ants { get; private set; }
        public Request(Method method, string body, string url)
        {
            Method = method;
            Body = body;
            Url = url;

            Ants = new List<Ant>();
        }

        public void AddAnt(Ant ant)
        {
            Ants.Add(ant);
        }

    }
}
