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
        public Login? SessionUser;
        public string? UserIdentifier => SessionUser.ToString();

        public string CurrentChannel = DefaultChannel;

        public string GetWelcomeMessage() => $"""
            Welcome to Arcadia!
            Arcadia is the live discussion board for Parcel, you are welcome to share your ideas and comments and questions and general chit-chat here! Please note that the Arcadia server is stateless and all chat history will NOT be saved permanently. If you want to keep some chat history, please save them at your own regards.
            Please respect each other when posting your questions.
            You can find the source code for Arcadia here: https://github.com/Charles-Zhang-Parcel/Arcadia
            Your unique guest number: {SessionUser.UniqueGuestID}, session ID: {ID}, username: {SessionUser.Username}
            Current chat channel: {CurrentChannel}
            """;

        protected override void OnOpen()
        {
            base.OnOpen();

            Logging.Info("New connection.");

            SessionUser = Singleton.ServerState.UpdateLogin(this, UniqueGuestID, ID);

            Send(GetWelcomeMessage());
            Sessions.Broadcast($"New connection: {UserIdentifier}; Current online: {Sessions.Count}");
        }
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);

            Singleton.ServerState.SessionUsers.TryRemove(this, out var login);
            Sessions.Broadcast($"User {UserIdentifier} has left the chat room; Current online: {Sessions.Count}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Logging.Info($"{SessionUser.Username} (): {e.Data}");

            string input = e.Data;

            string command = "!speak";
            string content = input;

            foreach (string part in input.Split(' ').Take(2))
            {
                if (part.StartsWith('-'))
                {
                    CurrentChannel = part;
                    content = ReplaceFirstOccurence(content, part);
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
                    BroadcastAtChannel(SessionUser, CurrentChannel, content);
                    break;
                case "!login":
                    string[] arguments = content.SplitCommandLine().ToArray();
                    string username = arguments[0];
                    string token = arguments[1];
                    string email = arguments[2];
                    // TODO: Authenticate
                    if (UserManager.AuthenticateUser(username, token, email))
                    {
                        SessionUser = new Login(username, SessionUser.UniqueGuestID, ID);
                        Singleton.ServerState.SessionUsers[this] = SessionUser;
                        BroadcastAtChannel(SessionUser, CurrentChannel, content);
                    }
                    else
                        Send($"Failed to authenticate.");
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
