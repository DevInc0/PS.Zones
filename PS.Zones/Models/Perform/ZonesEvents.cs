using Rocket.Unturned.Player;
using SDG.Unturned;
using System;

namespace PS.Zones.Models.Perform
{
    public sealed class ZonesEvents
    {
        public static event Action<Zone, UnturnedPlayer> OnPlayerEntered_Global;

        public static event Action<Zone, UnturnedPlayer> OnPlayerExited_Global;

        internal void InternalInvokeOnPlayerEntered(Zone zone, UnturnedPlayer unturnedPlayer)
        {
            Invoke(OnPlayerEntered_Global, nameof(OnPlayerEntered_Global), zone, unturnedPlayer);
        }

        internal void InternalInvokeOnPlayerExited(Zone zone, UnturnedPlayer unturnedPlayer)
        {
            Invoke(OnPlayerExited_Global, nameof(OnPlayerExited_Global), zone, unturnedPlayer);
        }

        private void Invoke(Action<Zone, UnturnedPlayer> targetEvent, string targetEventName, Zone zone, UnturnedPlayer unturnedPlayer)
        {
            string debugMessage = $"{targetEventName} | {zone.Name} | {unturnedPlayer.CSteamID}";

            targetEvent?.TryInvoke(debugMessage, zone, unturnedPlayer);
        }
    }
}
