namespace Arcadia.Shared
{
    public class Message
    {
        public User User { get; set; }

        public string Command { get; set; }
        public string[] Arguments { get; set; }

        public Message(User user, string input)
        {
            var parts = input.SplitCommandLine().ToArray();
            Command = parts[0];
            Arguments = parts.Skip(1).ToArray();

            User = user;
        }
        public Message(User user, string command, string[] arguments)
        {
            Command = command;
            Arguments = arguments;

            User = user;
        }
    }
}
