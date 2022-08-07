using LANMaker.SampleBase;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LANMaker;

public class Worker : BackgroundService
{
    private readonly IApplicationWindow _applicationWindow;

    public Worker(IApplicationWindow window)
    {
        _applicationWindow = window;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _applicationWindow.Run();
        }
    }
}