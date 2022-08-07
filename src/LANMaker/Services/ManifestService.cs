using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LANMaker.Data;
using static System.Environment;

namespace LANMaker.Services
{
    public class ManifestService
    {
        public static string ConfigurationDirectory => Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "LANMaker");
        public static string ManifestPath => Path.Combine(ConfigurationDirectory, "manifest.json");

        private readonly ConfigurationService _configurationService;
        private readonly StateContainer state;

        public ManifestService(StateContainer state, ConfigurationService configurationService)
        {
            this.state = state;
            _configurationService = configurationService;
        }

        public async Task<Manifest> GetManifest(CancellationToken stoppingToken)
        {
            CreateManifestDirectory();

            if (!LocalManifestExists())
            {
                var manifest = await FetchManifest(stoppingToken);
                await SaveManifest(manifest);

                return manifest;
            }

            return await ReadManifest();
        }

        public async Task UpdateManifest(CancellationToken stoppingToken)
        {
            DeleteManifest();
            var manifest = await GetManifest(stoppingToken);
            await SaveManifest(manifest);
        }

        private static async Task<Manifest> ReadManifest()
        {
            try
            {
                using var stream = new FileStream(ManifestPath, FileMode.Open, FileAccess.Read);
                var manifest = await JsonSerializer.DeserializeAsync<Manifest>(stream);
                return manifest;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch the manifest from the remote source in the configuration.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task<Manifest> FetchManifest(CancellationToken stoppingToken)
        {
            var configuration = state.Configuration;
            if (configuration == null)
            {
                return null;
            }

            var manifestUrl = configuration.ManifestUrl;
            var manifestStream = await DownloadTextFile(manifestUrl, stoppingToken);
            Manifest manifest;
            try
            {
                manifest = JsonSerializer.Deserialize<Manifest>(manifestStream);
            }
            catch
            {
                throw;
            }

            return manifest;
        }

        private static void DeleteManifest()
        {
            if (File.Exists(ManifestPath))
            {
                try
                {
                    File.Delete(ManifestPath);
                }
                catch
                {
                    throw;
                }
            }
        }

        private async Task SaveManifest(Manifest newManifest)
        {
            try
            {
                var json = JsonSerializer.Serialize(newManifest, new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                });

                using var manifestFile = new StreamWriter(ManifestPath);
                await manifestFile.WriteAsync(json);

                // Update state with new manifest
                state.Manifest = newManifest;
            }
            catch
            {
                throw;
            }
        }

        private static void CreateManifestDirectory()
        {
            if (!Directory.Exists(ConfigurationDirectory))
            {
                try
                {
                    Directory.CreateDirectory(ConfigurationDirectory);
                }
                catch
                {
                    throw;
                }
            }
        }

        private static bool LocalManifestExists()
        {
            if (!File.Exists(ManifestPath))
            {
                return false;
            }

            return true;
        }

        private static async Task<string> DownloadTextFile(string url, CancellationToken stoppingToken)
        {
            using (var client = new HttpClient())
            {
                using var result = await client.GetAsync(url, stoppingToken);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync(stoppingToken);
                }
            }

            return null;
        }
    }
}
