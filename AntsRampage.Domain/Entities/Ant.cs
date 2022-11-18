using AntsRampage.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsRampage.Domain.Entities
{
    public class Ant : IEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime RequestedAt { get; private set; }
        public bool Succeed { get; private set; } = false;

        public void SetRequestDate(DateTime requestedAt) { 
            RequestedAt= requestedAt;
        }
        public void MarkFailed()
        {
            Succeed = false;
        }
        public void MarkSuccess()
        {
            Succeed = true;
        }

    }
}
