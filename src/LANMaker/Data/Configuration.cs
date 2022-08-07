using System;

namespace LANMaker.Data
{
	/// <summary>
	/// Client configuration. 
	/// </summary>
	public class Configuration
	{
		public string ManifestUrl { get; set; }
		public int TimeoutMinutes { get; set; } = 10;
		public string StoragePath { get; set; }
		public string ThemeName { get; set; }
		public ClientGame[] InstalledGames { get; set; } = Array.Empty<ClientGame>();
	}
}
