using System.Collections.Generic;

namespace LANMaker.Data
{
	public class StateContainer
	{
		public Configuration Configuration { get; set; }
		public Manifest Manifest { get; set; }
		public List<GameDownload> GameDownloads { get; private set; } = new List<GameDownload>();
		public List<CombinedGame> Games { get; set; } = new List<CombinedGame>();
	}
}
