using IntegrationService.Controllers;

namespace IntegrationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ClientsController clientsController = new ClientsController();
                clientsController.GetClientes();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
