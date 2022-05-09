using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractClasses
{
    public abstract class BasePing
    {
        public int HostAddress { get; set; }
        public int DestAddress { get; set; }
        public string Data { get; set; }
        //public virtual bool Init() { return true; } // If ping boots, return true
        public abstract bool Init(); // Abstract means you must implement this method
    }
}
