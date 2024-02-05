namespace Arcadia.Server
{
    public static class Logging
    {
        #region Semantic Logging
        public static void Info(string message, bool print = true)
        {
            Log($"[Info] {message}", print);
        }
        #endregion

        #region Methods
        public static void Log(string message, bool print = true)
        {
            DateTime date = DateTime.Now;
            string outputLine = $"{date:yyyy-MM-dd HH:mm:ss} - {message}";

            if (print)
                Console.WriteLine(outputLine);
        }
        #endregion
    }
}
