using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LANMaker.Data;

namespace LANMaker.Services
{
    public class CombinedGameService
    {
        private readonly StateContainer state;

        public CombinedGameService(StateContainer state)
        {
            this.state = state;
        }

        public List<CombinedGame> GetCombinedGames()
        {
            var manifest = state.Manifest;
            var configuration = state.Configuration;

            var list = new List<CombinedGame>();

            foreach (var serverGame in manifest.Games)
            {
                list.Add(new CombinedGame
                {
                    ServerGame = serverGame,
                    ClientGame = configuration.InstalledGames.FirstOrDefault(clientGame => clientGame.Name == serverGame.Name)
                });
            }

            return list.OrderBy(combinedGame => combinedGame.ServerGame.Title).ToList();
        }
    }
}
