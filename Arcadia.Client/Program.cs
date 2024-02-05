using WebSocketSharp;

namespace Arcadia.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Connecting to Arcadia server...");

            string endpoint = "ws://localhost:9910/Arcadia";
            var connection = Connect(endpoint);

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (input == "exit")
                    break;
                else if (input != null)
                    connection.Send(input);
            }

            connection.Close();
        }

        private static WebSocket Connect(string endpoint)
        {
            WebSocket ws = new(endpoint);
            ws.OnMessage += (sender, e) =>
            {
                // Assuming we are in the input row, this automatically fixes display
                if (Console.CursorLeft != 0)
                {
                    Console.CursorLeft = 0;
                    Console.WriteLine(e.Data);
                    Console.Write("> ");
                }
                else
                    Console.WriteLine(e.Data);
            };

            ws.Connect();
            return ws;
        }
    }
}
