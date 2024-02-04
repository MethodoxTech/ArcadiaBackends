using WebSocketSharp.Server;
using WebSocketSharp;
using Arcadia.Shared;

namespace Arcadia.Server
{
    public class Arcadia : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();

            Send("Welcome to Arcadia!");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            string input = e.Data;
            Message? message = null;

            User user = new("Guest", null, false);
            message = input.StartsWith('!') 
                ? new(user, input) 
                : new(user, "!speak", [input]);

            switch (message.Command)
            {
                case "speak":
                    Sessions.Broadcast($"{user.Name}: {message.Arguments.First()}");
                    break;
                default:
                    Sessions.Broadcast($"{user.Name}: {message.Arguments.First()}");
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
