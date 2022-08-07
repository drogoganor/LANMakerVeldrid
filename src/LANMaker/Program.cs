using LANMaker;
using LANMaker.Components;
using LANMaker.Data;
using LANMaker.SampleBase;
using LANMaker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
            services
                .AddSingleton<MainMenu>()
                .AddSingleton<InstalledGamesView>()
                .AddSingleton<StartupService>()
                .AddSingleton<StateContainer>()
                .AddSingleton<ManifestService>()
                .AddSingleton<ConfigurationService>()
                .AddSingleton<InstallerService>()
                .AddSingleton<GameRunService>()
                .AddSingleton<DownloadTrackerService>()
                .AddSingleton<CombinedGameService>()
                .CreateWindow()
                .AddHostedService<Worker>()
            );

var host = CreateHostBuilder(args).Build();
await host.Services.RegisterLANMaker();
await host.RunAsync();

