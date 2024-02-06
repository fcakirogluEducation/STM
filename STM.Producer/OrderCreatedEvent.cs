using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM.Producer
{
    internal record OrderCreatedEvent
    {
        public string OrderCode { get; init; }
        public string Details { get; init; }
    }
}