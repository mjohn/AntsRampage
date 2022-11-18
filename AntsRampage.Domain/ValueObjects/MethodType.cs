using AntsRampage.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsRampage.Domain.ValueObjects
{
    public class Method : ValueObject
    {
        public string Type { get; private set; } = "";

        private Method()
        {
        }

        private Method(string type)
        {
            Type = type;
        }


        public static Method POST => new("POST");

        public static Method GET => new("GET");





        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
        }
    }
}
