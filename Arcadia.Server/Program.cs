using WebSocketSharp.Server;
using WebSocketSharp;
using Arcadia.Shared;

namespace Arcadia.Server
{
    public static class LogHelper
    {
        public static void LogLine(string content)
        {
            Console.WriteLine(content);
        }
    }

    public class Arcadia : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();

            LogHelper.LogLine("New connection.");
            Log.Info("New connection.");

            Send("Welcome to Arcadia!");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            LogHelper.LogLine(e.Data);
            Log.Info(e.Data);

            string input = e.Data;
            string[] parts = input.SplitCommandLine().ToArray();
            string userID = parts[0].TrimStart('{').TrimEnd('}');
            string command = parts[1];
            string content = parts[2];

            string userName = userID.Split(':').First();
            string userToken = userID.Split(':').Last();
            User user = new(userName, userToken, true);

            switch (command)
            {
                case "!speak":
                    Sessions.Broadcast($"{user.Name}: {content}");
                    break;
                default:
                    Sessions.Broadcast($"{user.Name}: {content}");
                    break;
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start arcadia...");
            WebSocketServer wssv = new("ws://localhost:9910");
            wssv.AddWebSocketService<Arcadia>("/Arcadia");
            wssv.Start();

            Console.WriteLine("Arcadia is started. Pressed any key to quit.");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
