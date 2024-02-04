using WebSocketSharp;

namespace Arcadia.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter token: ");
            string token = Console.ReadLine();

            string endpoint = "ws://localhost:9910/Arcadia";
            var connection = Connect(endpoint);
            Console.WriteLine("Connected.");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (input == "exit")
                    break;
                else if (input != null)
                    connection.Send($"{{{username}:{token}}} !speak {input}");
            }

            connection.Close();
        }

        private static WebSocket Connect(string endpoint)
        {
            WebSocket ws = new(endpoint);
            ws.OnMessage += (sender, e) =>
                Console.WriteLine(e.Data);

            ws.Connect();
            return ws;
        }
    }
}
