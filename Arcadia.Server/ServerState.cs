using System.Collections.Concurrent;

namespace Arcadia.Server
{
    public record Login(string Username, long ID)
    {
        public override string ToString()
        {
            return $"{Username} ({ID})";
        }
    }

    public static class Singleton
    {
        public static readonly ArcadiaServerState ServerState = new();
    }

    /// <summary>
    /// TODO: Check thread safety
    /// </summary>
    public class ArcadiaServerState
    {
        public List<string> ReservedNames = ["Airi", "Charles Zhang"];
        public List<string> PublicChannels = ["-public"];
        public List<string> PrivateChannels = ["--private1"];
        public bool RestrictedUserMode = false; // If true, do not allow guests without authentication

        #region Properties
        public ConcurrentDictionary<Arcadia, Login> SessionUsers = new();
        #endregion

        #region Methods
        public Login UpdateLogin(Arcadia arcadia, long ID)
        {
            string username = $"Guest {ID}";
            var login = new Login(username, ID);
            SessionUsers[arcadia] = login;
            return login;
        }
        #endregion
    }
}
