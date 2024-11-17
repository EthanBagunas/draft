using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ASI.Basecode.Services.Interfaces;

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
    
namespace ASI.Basecode.Services.Services
{
    #nullable enable
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceProvider _services;
        private Timer? _timer = null;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");
        _timer = new Timer(Runthis, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromMinutes(1));

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
        private void DoWork(object? state)
        {
            using (var scope = _services.CreateScope())
            {
                var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
                bookService.UpdateBookingStatuses();
                _logger.LogInformation("Updated booking statuses at: {time}", DateTimeOffset.Now);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    #nullable disable
}