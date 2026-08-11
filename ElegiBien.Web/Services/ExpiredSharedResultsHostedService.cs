using ElegiBien.Application.Interfaces;

namespace ElegiBien.Web.Services;

public sealed class ExpiredSharedResultsHostedService
    : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExpiredSharedResultsHostedService> _logger;

    public ExpiredSharedResultsHostedService(
        IServiceScopeFactory scopeFactory,
        ILogger<ExpiredSharedResultsHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        await RunCleanupAsync(stoppingToken);

        using var timer = new PeriodicTimer(Interval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunCleanupAsync(stoppingToken);
        }
    }

    private async Task RunCleanupAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            var cleanup = scope.ServiceProvider
                .GetRequiredService<IExpiredSharedResultCleanupService>();

            var removed = await cleanup.CleanupAsync(
                DateTime.UtcNow,
                cancellationToken);

            if (removed > 0)
            {
                _logger.LogInformation(
                    "Se eliminaron {Count} enlaces compartidos vencidos.",
                    removed);
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "No se pudo ejecutar la limpieza de enlaces compartidos vencidos.");
        }
    }
}
