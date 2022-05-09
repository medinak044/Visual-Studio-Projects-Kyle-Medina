using Pinger;
using System.Net.NetworkInformation;
using System.Text;



// Pinging Google DNS Server 4.2.2.2


#region Refactored in PingService script
//Ping pingSender = new Ping();
//PingOptions options = new PingOptions();

//options.DontFragment = true; // In case of errors

//string data = "Learn to code";
//byte[] buffer = Encoding.ASCII.GetBytes(data);
//int timeout = 120;
//string address = "4.2.2.2";
//PingReply reply = pingSender.Send(address, timeout, buffer, options);

//if (reply.Status == IPStatus.Success) 
//{ 
//    Console.WriteLine($"Response: {"{0}"} {reply.Status.ToString()}");
//    Console.WriteLine($"Roundtrip: {"{0}"} {reply.RoundtripTime}");
//    Console.WriteLine($"Time to live: {"{0}"} {reply.Options.Ttl}");
//    Console.WriteLine($"Buffer size: {"{0}"} {reply.Buffer.Length}");
//}
#endregion

void Func_Ping()
{
    PingService pingService = new PingService();
    var response = pingService.SendPing();
    Console.WriteLine(response);
}

void Func_Dog()
{
    Dog lucky = new Dog();
    lucky.DogBarking();

    //IDogWalker lucky2 = new Dog(); // Creates a new Dog
}

Func_Dog();
