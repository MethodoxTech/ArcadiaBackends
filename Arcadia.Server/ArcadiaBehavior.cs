using WebSocketSharp.Server;
using WebSocketSharp;

namespace Arcadia.Server
{
    public class Arcadia : WebSocketBehavior
    {
        public const int MessageFrequencyLimitInMinutes = 1; // User must wait this long before broadcasting another message
        public const int MessageLengthLimitInWords = 250; // User message mustn't exeed this length limit to save server from too much work load
        public const string DefaultChannel = "-default";

        public static long _UniqueGuestID = 0;
        public static long UniqueGuestID 
        {
            get 
            {
                return Interlocked.Increment(ref _UniqueGuestID);
            } 
        }

        public string GetWelcomeMessage(long id, string sessionID, string username) => $"""
            Welcome to Arcadia!
            Arcadia is the live discussion board for Parcel, you are welcome to share your ideas and comments and questions and general chit-chat here! Please note that the Arcadia server is stateless and all chat history will NOT be saved permanently. If you want to keep some chat history, please save them at your own regards.
            Please respect each other when posting your questions.
            You can find the source code for Arcadia here: https://github.com/Charles-Zhang-Parcel/Arcadia
            Your unique guest number: {id}, session ID: {sessionID}, username: {username}
            """;

        protected override void OnOpen()
        {
            base.OnOpen();

            Logging.Info("New connection.");

            Login login = Singleton.ServerState.UpdateLogin(this, UniqueGuestID);

            Send(GetWelcomeMessage(login.ID, ID, login.Username));
            Sessions.Broadcast($"New connection: {login.Username} (#{login.ID}); Current online: {Sessions.Count}");
        }
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);

            Singleton.ServerState.SessionUsers.TryRemove(this, out var login);
            Sessions.Broadcast($"User {login.Username} (#{login.ID}) has left the chat room; Current online: {Sessions.Count}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var user = Singleton.ServerState.SessionUsers[this];
            Logging.Info($"{user}: {e.Data}");

            string input = e.Data;

            string channel = "-default";
            string command = "!speak";
            string content = input;

            foreach (string part in input.Split(' ').Take(2))
            {
                if (part.StartsWith('-'))
                {
                    channel = part;
                    content = ReplaceFirstOccurence(content, channel);
                }
                else if (part.StartsWith('!'))
                {
                    command = part;
                    content = ReplaceFirstOccurence(content, command);
                }
            }

            switch (command)
            {
                case "!speak":
                    BroadcastAtChannel(user, channel, content);
                    break;
                default:
                    Send($"Invalid command: {command} (In {e.Data})");
                    break;
            }

            static string ReplaceFirstOccurence(string content, string keyword)
            {
                int index = content.IndexOf(keyword);
                return content.Remove(index + keyword.Length) + content.Substring(index + 1);    // Replace the first occurence only
            }
        }

        #region Routines
        private void BroadcastAtChannel(Login user, string channel, string message)
        {
            // TODO: Handle channel
            string username = user.Username;
            string content = $"{username}: {message}";
            foreach (IWebSocketSession? session in Sessions.Sessions)
                if (session.ID != ID)
                    Sessions.SendTo(content, session.ID);
        }
        #endregion
    }
}
