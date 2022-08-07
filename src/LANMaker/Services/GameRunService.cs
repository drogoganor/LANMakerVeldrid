using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using LANMaker.Data;

namespace LANMaker.Services
{
    public class GameRunService
    {
        private Process process;

        public async Task PlayGame(ClientGame game)
        {
            if (process != null)
            {
                if (process.HasExited)
                {
                    process = null;
                }
                else
                {
                    // Game is already running; do nothing
                    return;
                }
            }

            var workingDirectory = Path.Combine(ManifestService.ConfigurationDirectory, game.Name);
            var processPath = Path.Combine(workingDirectory, game.ExePath);

            if (!File.Exists(processPath))
            {
                throw new Exception($"Couldn't find path: {processPath}");
            }

            var processInfo = new ProcessStartInfo
            {
                FileName = processPath,
                WorkingDirectory = workingDirectory,
            };

            process = Process.Start(processInfo);
        }
    }
}
