using AntsRampage.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AntsRampage.Domain.Entities
{
    public class Ant : IEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime RequestedAt { get; private set; }
        public bool Succeed { get; private set; } = false;
        public bool Working { get; private set; } = false;

        public string ResponseBody { get; private set; }

        public TimeSpan Elapsed => _sw.Elapsed;

        private Stopwatch _sw = new Stopwatch();

        


        public async Task StartWorking(DateTime requestedAt)
        {
            _sw.Start();
            Working = true;
            RequestedAt = requestedAt;
        }

        public async Task EndWorking(bool succeed, string responseBody) 
        { 
            _sw.Stop();
            Succeed = succeed;
            Working = false;
            ResponseBody= responseBody;
        }

    }
}
