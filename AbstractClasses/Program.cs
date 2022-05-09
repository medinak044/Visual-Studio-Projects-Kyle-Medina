using AbstractClasses;

PingIPV6 pingIPV6 = new PingIPV6();
Console.WriteLine(pingIPV6.Init());
Console.WriteLine(pingIPV6.SendPing());

PingIPV4 pingIPV4 = new PingIPV4();
Console.WriteLine(pingIPV4.Init());
Console.WriteLine(pingIPV4.SendPing());

