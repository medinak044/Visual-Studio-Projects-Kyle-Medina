using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility_Fake.Ping;

public class NetworkService
{
    public string SendPing()
    {
        return "Success: Ping Sent!";
    }

    public int PingTimeOut(int a, int b)
    {
        return a + b;
    }
}
