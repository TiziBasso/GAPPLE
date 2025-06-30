using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace GAPPLE.Client.Services
{

    public class SignalRService
    {
        private NavigationManager NavigationManager { get; }
        private ILogger<SignalRService> Log { get; set; }
        public HubConnection HubConnection { get; set; }
        private const string URI_BASE = "api/signalr";

        private bool IsDisposed { get; set; }

        public SignalRService(NavigationManager navigationManager, ILogger<SignalRService> logger)
        {
            Log = logger;
            NavigationManager = navigationManager;
            Build();
        }

        private void Build()
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/" + URI_BASE))
                .Build();
            IsDisposed = false;
        }

        public async Task StartConnection()
        {
            try
            {
                if (IsDisposed)
                    Build();

                if (HubConnection?.State != HubConnectionState.Connected)
                    await HubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Log.LogError("{message}", "Error connecting to SignalR: " + ex.ToString());
            }
        }

        public async Task StopConnection()
        {
            if (HubConnection?.State == HubConnectionState.Connected)
            {
                try
                {
                    await HubConnection.StopAsync();
                }
                finally
                {
                    await HubConnection.DisposeAsync();
                    IsDisposed = true;
                }
            }
        }

        public string GetConnectionId()
        {
            return HubConnection.ConnectionId;
        }

        public void ReceivePercent(Action<int> percentReceived)
        {
            HubConnection.On<int>("CambiarPorcentaje", (p) =>
            {
                percentReceived(p);
            });
        }

        public void ReceiveText(Action<string> textReceived)
        {
            HubConnection.On<string>("CambiarTexto", (t) =>
            {
                textReceived(t);
            });
        }
    }
}
