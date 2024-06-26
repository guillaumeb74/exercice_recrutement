﻿namespace ExerciceRecrutementRestaurant
{
    // The main class of the background service worker
    public class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }  

        // Parallel task used to keep service alive
        protected async Task TaskWaitForStopSignal(CancellationToken stoppingToken)
        {
            // If cancellation signal received, try to shutdown gracefully
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Wait X seconds before performing loop again, but immediately stop delay if interrupt requested 
                    try
                    {
                        await Task.Delay(10000, stoppingToken);
                    }
                    catch (Exception exception)
                    {
                        if (!stoppingToken.IsCancellationRequested)
                        {
                            throw exception;
                        }
                    }
                }
                catch (Exception exception)
                {
                    LogManager.AppendLogLine("ERROR", "error " + exception.Message);
                    throw exception;
                }
            }
            LogManager.AppendLogLine("INFO", "stopping service ...");
        }

        // Called when the service is launched
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            LogManager.AppendLogLine("INFO", "started service");
            // Looping main service and wait for it to stop if service stop requested
            try
            {
                // Create a linked cancellation token source to be able to manually stop all service tasks
                CancellationTokenSource innerCancellationTokenSource = new CancellationTokenSource();
                CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(innerCancellationTokenSource.Token, stoppingToken);

                // Create a list of tasks
                var tasks = new List<Task>
                {
                    Task.Run(() => TaskWaitForStopSignal(linkedTokenSource.Token).GetAwaiter().GetResult())
                };
                await Task.WhenAny(tasks);
                // When at least one task ends, cancel the token to stop all remaining tasks
                innerCancellationTokenSource.Cancel();
                await Task.WhenAll(tasks);
            }
            catch (Exception exception)
            {
                LogManager.AppendLogLine("ERROR", exception.Message);
                throw exception;
            }

            LogManager.AppendLogLine("INFO", "stopped service");
        }

    }
}