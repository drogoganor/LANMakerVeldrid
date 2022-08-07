using LANMaker.Data;

namespace LANMaker.Services
{
    public sealed class StartupService
    {
        private readonly ConfigurationService _configurationService;
        private readonly ManifestService _manifestService;
        private readonly CombinedGameService _combinedGameService;

        public StartupService(
            ConfigurationService configurationService,
            ManifestService manifestService,
            CombinedGameService combinedGameService)
        {
            _configurationService = configurationService;
            _manifestService = manifestService;
            _combinedGameService = combinedGameService;
        }

        public async Task<Configuration> GetConfiguration(CancellationToken cancellationToken)
        {
            return await _configurationService.GetConfiguration(cancellationToken);
        }

        public async Task<Manifest> GetManifest(CancellationToken cancellationToken)
        {
            return await _manifestService.GetManifest(cancellationToken);
        }

        public async Task<List<CombinedGame>> GetCombinedGames(CancellationToken cancellationToken)
        {
            return _combinedGameService.GetCombinedGames();
        }
    }
}
