using Rocket.Unturned;
using Rocket.Unturned.Player;

namespace PS.Zones.Main
{
    public sealed partial class Plugin
    {
        protected override void SubscribeToEvents()
        {
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
        }

        protected override void UnsubscribeFromEvents()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
        }

        private void OnPlayerConnected(UnturnedPlayer unturnedPlayer)
        {
            Instance._players.Add(unturnedPlayer);
        }

        private void OnPlayerDisconnected(UnturnedPlayer unturnedPlayer)
        {
            Instance._players.Remove(unturnedPlayer);
        }
    }
}
