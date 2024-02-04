namespace Arcadia.Server
{
    public static class HistoryHelper
    {
        #region Chat History
        public static void AppendHistory(string channel, string message)
        {
            File.AppendAllText(channel, message + Environment.NewLine);
        }
        #endregion
    }
}
