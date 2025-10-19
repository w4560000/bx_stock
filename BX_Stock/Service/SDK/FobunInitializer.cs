using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;

namespace BX_Stock.Service.SDK
{
    public class FobunInitializer : IHostedService
    {
        private readonly Fobun _fobun;

        public FobunInitializer(Fobun fobun)
        {
            _fobun = fobun;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _fobun.SDKInit();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}