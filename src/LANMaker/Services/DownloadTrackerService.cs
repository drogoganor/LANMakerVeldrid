using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using LANMaker.Data;

namespace LANMaker.Services
{
    public class DownloadTrackerService
    {
        public event EventHandler DownloadStatusChanged;
        private readonly StateContainer state;

        public DownloadTrackerService(StateContainer state)
        {
            this.state = state;
        }

        public bool IsGameInstalling(ServerGame game)
        {
            if (state.GameDownloads.Any(gameDownload => gameDownload.Game.Name == game.Name))
            {
                return true;
            }

            return false;
        }

        public void TrackGameDownload(ServerGame game, HttpClient client, CancellationToken cancellationToken)
        {
            var gameDownload = new GameDownload
            {
                DownloadTime = DateTime.Now,
                Game = game,
                HttpClient = client,
                CancellationToken = cancellationToken,
                DownloadStatus = GameDownloadStatus.Downloading,
            };

            state.GameDownloads.Add(gameDownload);
            DownloadStatusChanged?.Invoke(this, null);
        }

        public void TrackGameInstall(ServerGame game, CancellationToken cancellationToken)
        {
            //var originalDateTime = RemoveDownload(game);
            var download = state.GameDownloads.First(gameDownload => gameDownload.Game.Name == game.Name);
            download.DownloadStatus = GameDownloadStatus.Installing;
            download.HttpClient = null;
            DownloadStatusChanged?.Invoke(this, null);

            //var gameInstall = new GameDownload
            //{
            //    DownloadTime = originalDateTime,
            //    Game = game,
            //    CancellationToken = cancellationToken,
            //    DownloadStatus = GameDownloadStatus.Installing,
            //    HttpClient = null,
            //};

            //GameDownloads.Add(gameInstall);
        }

        public DateTime RemoveDownload(ServerGame game)
        {
            var existingDownload = GetGameDownload(game);
            state.GameDownloads.Remove(existingDownload);
            DownloadStatusChanged?.Invoke(this, null);
            return existingDownload.DownloadTime;
        }

        public void CancelDownload(GameDownload gameDownload)
        {
            if (gameDownload.CancellationToken.CanBeCanceled)
            {
                gameDownload.CancellationToken.ThrowIfCancellationRequested();
            }

            state.GameDownloads.Remove(gameDownload);
            DownloadStatusChanged?.Invoke(this, null);
        }

        private GameDownload GetGameDownload(ServerGame game)
        {
            return state.GameDownloads.FirstOrDefault(gameDownload => gameDownload.Game.Name == game.Name);
        }
    }
}
