namespace Arcadia.Server
{
    public static class Singleton
    {
        public static readonly ArcadiaServerState ServerState = new();
    }

    public class ArcadiaServerState
    {
        public List<string> ReservedNames = ["Airi", "Charles Zhang"];
        public List<string> PublicChannels = ["-public"];
        public List<string> PrivateChannels = ["--private1"];
        public bool RestrictedUserMode = false; // If true, do not allow guests without authentication
    }
}
