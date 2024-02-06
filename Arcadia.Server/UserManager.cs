namespace Arcadia.Server
{
    public static class HashHelper
    {
        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = (hash1 << 5) + hash1 ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = (hash2 << 5) + hash2 ^ str[i + 1];
                }

                return hash1 + hash2 * 1566083941;
            }
        }
    }
    public record UserConfigurations
    {
        public string Username { get; set; }
        public int TokenHash { get; set; }
        public string? Email { get; set; }

        public UserConfigurations(string username, int tokenHash, string email)
        {
            Username = username;
            TokenHash = tokenHash;
            Email = email;
        }
    }

    public static class UserManager
    {
        private static Dictionary<string, UserConfigurations> UserStore = new();

        /// <summary>
        /// Authenticate or register a new user
        /// </summary>
        public static bool AuthenticateUser(string username, string token, string email)
        {
            if (UserStore.TryGetValue(username, out UserConfigurations userConfiguration)) 
            {
                if (userConfiguration.TokenHash == token.GetDeterministicHashCode())
                {
                    userConfiguration.Email = email ?? userConfiguration.Email;
                    return true;
                }
                else return false;
            }
            else
            {
                UserStore[username] = new UserConfigurations(username, token.GetDeterministicHashCode(), email);
                return true;
            }
        }
    }
}
