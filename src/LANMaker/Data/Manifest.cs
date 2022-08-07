using System;

namespace LANMaker.Data
{
	/// <summary>
	/// Server configuration.
	/// </summary>
	public class Manifest
	{
		public string RootUrl { get; set; }
		public string ManifestFile { get; set; }
		public ServerGame[] Games { get; set; }
	}
}
