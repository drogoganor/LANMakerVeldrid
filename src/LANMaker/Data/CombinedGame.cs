namespace LANMaker.Data
{
    /// <summary>
    /// Contains both client and server game definitions
    /// </summary>
    public class CombinedGame
    {
        public ServerGame ServerGame { get; set; }
        public ClientGame ClientGame { get; set; }
        public bool IsInstalled => ClientGame != null;
        public bool IsUpdateAvailable => IsInstalled && ClientGame.InstalledVersion != ServerGame.Version;
    }
}
