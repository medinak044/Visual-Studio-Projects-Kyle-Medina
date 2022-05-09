using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractClasses
{
    public class PingIPV6: BasePing
    {
        public override bool Init() { return true; }
        public bool SendPing() { return true; } // Different implementation
    }
}
