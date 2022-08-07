using LANMaker.Components;
using LANMaker.Data;
using LANMaker.SampleBase;
using LANMaker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LANMaker
{
	public static class Startup
    {
        public static IServiceCollection CreateWindow(this IServiceCollection services)
        {
            var window = new VeldridStartupWindow("LANMaker");
            services.AddSingleton<IApplicationWindow>(window);

            //window.Run();

            return services;
        }

        public static async Task RegisterLANMaker(this IServiceProvider services)
		{
            // Load settings
            var loadSettingsAdapter = services.GetRequiredService<StartupService>();

            var cancellationToken = new CancellationToken();
            var configuration = await loadSettingsAdapter.GetConfiguration(cancellationToken);
            var manifest = await loadSettingsAdapter.GetManifest(cancellationToken);

            var stateContainer = services.GetRequiredService<StateContainer>();
            stateContainer.Configuration = configuration;
            stateContainer.Manifest = manifest;

            var games = await loadSettingsAdapter.GetCombinedGames(cancellationToken);
            stateContainer.Games = games;



            var mainMenu = services.GetRequiredService<MainMenu>();

            mainMenu.Show();
            mainMenu.OnNewGame += () =>
            {
                mainMenu.Hide();
                //game.Show();
                //game.OnEndGame += () =>
                //{
                //    game.Hide();
                //    mainMenu.Show();
                //};
            };

            //var window = services.GetRequiredService<IApplicationWindow>();
        }
	}
}
