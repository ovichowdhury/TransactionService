using Cronos;

namespace TransactionService.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly CronExpression _cronExpression;
    private readonly TimeZoneInfo _timeZoneInfo;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;

        // Read cron expression from configuration. If using a 6-field cron (with seconds),
        // parse with CronFormat.IncludeSeconds. Provide a safe default (every minute).
        var cronExpr = configuration["Schedule:Cron"];
        if (string.IsNullOrWhiteSpace(cronExpr))
        {
            cronExpr = "0 */1 * * * *"; // default: every minute (seconds precision)
            _logger.LogInformation("No cron expression configured; using default: {cron}", cronExpr);
        }

        _cronExpression = CronExpression.Parse(cronExpr, CronFormat.IncludeSeconds);
        _timeZoneInfo = TimeZoneInfo.Local;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            var now = DateTime.UtcNow;
            var next = _cronExpression.GetNextOccurrence(now, _timeZoneInfo);
            if (!next.HasValue)
            {
                _logger.LogWarning("No next occurrence found for the cron expression.");
                break;
            }

            var delay = next.Value - now;
            _logger.LogInformation("Next occurrence at: {nextTime} (in {delay})", next.Value, delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Task was canceled, exit gracefully
                break;
            }

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                // Place the scheduled task logic here
                _logger.LogInformation("Executing scheduled task at: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the scheduled task.");
            }
        }
    }
}
