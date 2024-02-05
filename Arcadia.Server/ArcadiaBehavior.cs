using Arcadia.Shared;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace Arcadia.Server
{
    public class Arcadia : WebSocketBehavior
    {
        public const int MessageFrequencyLimitInMinutes = 1; // User must wait this long before broadcasting another message
        public const int MessageLengthLimitInWords = 250; // User message mustn't exeed this length limit to save server from too much work load

        public static long _UniqueGuestID = 0;
        public static long UniqueGuestID 
        {
            get 
            {
                return Interlocked.Increment(ref _UniqueGuestID);
            } 
        }

        public string GetWelcomeMessage(long ID) => $"""
            Welcome to Arcadia! Unique guest number: {ID}
            Arcadia is the live discussion board for Parcel, you are welcome to share your ideas and comments and questions and general chit-chat here! Please note that the Arcadia server is stateless and all chat history will NOT be saved permanently. If you want to keep some chat history, please save them at your own regards.
            Please respect each other when posting your questions.
            You can find the source code for Arcadia here: https://github.com/Charles-Zhang-Parcel/Arcadia
            """;

        protected override void OnOpen()
        {
            base.OnOpen();

            Log.Info("New connection.");

            Send(GetWelcomeMessage(UniqueGuestID));
            Sessions.Broadcast($"New connection: guest {Sessions.Count - 1}; Current online: {Sessions.Count}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
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
}
