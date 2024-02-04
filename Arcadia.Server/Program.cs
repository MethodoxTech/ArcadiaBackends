using WebSocketSharp.Server;

namespace Arcadia.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string envVar = "PARCEL_ARCADIA_SERVER_ADDRESS";
            const string defaultAddress = "ws://localhost:9910";

            string serverAddress = args.Length > 0
                ? args[0]
                : Environment.GetEnvironmentVariable(envVar) != null
                    ? Environment.GetEnvironmentVariable(envVar)!
                    : defaultAddress;

            Console.WriteLine($"Start arcadia at {serverAddress}...");
            WebSocketServer wssv = new(serverAddress);
            // TODO: wssv logging is not working; We need to implement our own, so the behavior is more predictable
            wssv.Log.Level = WebSocketSharp.LogLevel.Info;
            wssv.Log.Output += (data, path) =>
            {
                Console.WriteLine(data);
            };

            wssv.AddWebSocketService<Arcadia>("/Arcadia");
            wssv.Start();

            Console.WriteLine("Arcadia is started. Press any key to quit.");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
