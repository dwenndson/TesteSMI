using System;
using System.Threading;
using System.Threading.Tasks;
using IogServices.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IogServices.BackgroundTasks
{
    public class RestartCommandFieldsTask : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public RestartCommandFieldsTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                ticketService.RestartCommandFields();
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}