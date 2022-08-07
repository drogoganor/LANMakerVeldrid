using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LANMaker.Data;

namespace LANMaker.Services
{
    public class InstallerService
    {
        private readonly DownloadTrackerService _downloadTrackerService;
        private readonly ConfigurationService _configurationService;
        private readonly StateContainer state;

        public InstallerService(StateContainer state, ConfigurationService configurationService, DownloadTrackerService downloadTrackerService)
        {
            this.state = state;
            _configurationService = configurationService;
            _downloadTrackerService = downloadTrackerService;
        }

        public async Task InstallGame(ServerGame game, CancellationToken cancellationToken)
        {
            var installPath = Path.Combine(ManifestService.ConfigurationDirectory, game.Name);
            var configuration = state.Configuration;

            // Don't install games twice
            if (_downloadTrackerService.IsGameInstalling(game))
            {
                return;
            }

            if (!await IsGameInstalled(game, installPath, configuration))
            {
                var gameArchiveStream = await DownloadGameArchive(game, cancellationToken);

                _downloadTrackerService.TrackGameInstall(game, cancellationToken);
                await ExtractGame(gameArchiveStream, installPath, game, cancellationToken);
                await _configurationService.WriteInstalledGame(game, installPath, cancellationToken);

                // Stop tracking game download
                _downloadTrackerService.RemoveDownload(game);
            }
        }

        public async Task DeleteGame(ClientGame game, CancellationToken cancellationToken)
        {
            var installPath = Path.Combine(ManifestService.ConfigurationDirectory, game.Name);
            try
            {
                Directory.Delete(installPath, true);
            }
            catch
            {
                throw;
            }

            _configurationService.DeleteGame(game, cancellationToken);
        }

        public static void ViewInExplorer(ClientGame game)
        {
            var installPath = Path.Combine(ManifestService.ConfigurationDirectory, game.Name);
            Process.Start("explorer.exe", installPath);
        }

        private async Task<bool> IsGameInstalled(ServerGame game, string installPath, Configuration configuration)
        {
            // Check if the install directory exists
            if (!Directory.Exists(installPath))
            {
                return false;
            }

            // Check if it's in our local config
            if (!configuration.InstalledGames.Any(installedGame => installedGame.Name == game.Name))
            {
                return false;
            }

            return true;
        }

        private string ZipUrl(ServerGame game)
        {
            var serverUri = new Uri(state.Manifest.RootUrl);
            var uriBuilder = new UriBuilder(serverUri);
            uriBuilder.Path += game.Name + "/";
            uriBuilder.Path += game.ZipUrl;
            return uriBuilder.Uri.ToString();
        }

        private async Task<Stream> DownloadGameArchive(ServerGame game, CancellationToken cancellationToken)
        {
            var memoryStream = new MemoryStream();
            using (var client = new HttpClient()
            {
                BaseAddress = new Uri(state.Manifest.RootUrl),
                Timeout = TimeSpan.FromMinutes(5),
            })
            {
                _downloadTrackerService.TrackGameDownload(game, client, cancellationToken);

                try
                {
                    var result = await client.GetAsync(game.Name + "/" + game.ZipUrl, cancellationToken);
                    if (result.IsSuccessStatusCode)
                    {
                        var stream = await result.Content.ReadAsStreamAsync(cancellationToken);
                        await stream.CopyToAsync(memoryStream, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return memoryStream;
        }

        private async Task ExtractGame(Stream stream, string installPath, ServerGame game, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(installPath))
            {
                Directory.CreateDirectory(installPath);
            }

            using var zip = new ZipArchive(stream);
            zip.ExtractToDirectory(installPath);
            stream.Close();
        }
    }
}
