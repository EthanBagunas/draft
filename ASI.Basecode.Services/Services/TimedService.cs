using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using Microsoft.Extensions.DependencyInjection;

public class TimedHostedService : IHostedService, IDisposable
{
    private readonly ILogger<TimedHostedService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private Timer _timer;
    
    public TimedHostedService(ILogger<TimedHostedService> logger,
                                IServiceScopeFactory serviceScopeFactory)   
    {   
        _logger = logger;
         _serviceScopeFactory = serviceScopeFactory;
    }
    

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");
        _timer = new Timer(Runthis, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void Runthis(object state)
    {
         try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
            _logger.LogInformation("DoWork2 is starting.");
            var timedBookService = scope.ServiceProvider.GetRequiredService<ITimedBookService>();
            timedBookService.DoWork();
            _logger.LogInformation("DoWork2 completed successfully.");
            }
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing DoWork.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}