namespace LANMaker.Data
{
    public class ServerGame
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public bool Multiplayer { get; set; }
        public bool Portable { get; set; } = true;
        public string PosterUrl { get; set; }
        public string ExeName { get; set; }
        public string ZipUrl { get; set; }
        public string[] ScreenshotUrls { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int YearPublished { get; set; }
    }
}
