﻿using WebSocketSharp.Server;

namespace Arcadia.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string envVar = "PARCEL_ARCADIA_SERVER_ADDRESS";
            const string defaultAddress = "ws://0.0.0.0:9910";

            string serverAddress = args.Length > 0
                ? args[0]
                : Environment.GetEnvironmentVariable(envVar) != null
                    ? Environment.GetEnvironmentVariable(envVar)!
                    : defaultAddress;

            Logging.Info($"Start arcadia at {serverAddress}...");
            WebSocketServer wssv = new(serverAddress);

            wssv.AddWebSocketService<Arcadia>("/Arcadia");
            wssv.Start();

            Console.WriteLine("Arcadia is started. Press any key to quit.");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
