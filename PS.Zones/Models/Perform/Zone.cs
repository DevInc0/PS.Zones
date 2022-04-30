using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PS.Zones.Models.Perform
{
    [Serializable]
    public abstract class Zone
    {
        public event Action<UnturnedPlayer> OnPlayerEntered;

        public event Action<UnturnedPlayer> OnPlayerExited;

        public string Name
        {
            get => _name;
            protected set => _name = value;
        }

        public List<CSteamID> InsidePlayers => InsidePlayersSteamIDs.ToList();

        /// <summary>
        /// Verifying if zone should update players positions periodically        
        /// </summary>        
        protected virtual bool ShouldUpdatePlayersPositions => true;

        protected readonly HashSet<CSteamID> InsidePlayersSteamIDs;

        private string _name;

        public Zone(string name)
        {
            _name = name;
            InsidePlayersSteamIDs = new HashSet<CSteamID>();
        }

        public abstract bool IsPositionInside(Vector3 position);

        /// <summary>
        /// Adds a player to <see cref="InsidePlayers"/> if he is inside the zone, otherwise removes        
        /// <para> Won't be called at all if <see cref="ShouldUpdatePlayersPositions"/> set to <see langword="false"/> </para>
        /// </summary>
        /// <returns><see langword="true"/> if the player is inside the zone, otherwise <see langword="false"/></returns>
        public bool UpdatePlayerPosition(UnturnedPlayer unturnedPlayer)
        {
            if (IsPositionInside(unturnedPlayer.Position))
            {
                AddPlayer(unturnedPlayer);
                return true;
            }

            RemovePlayer(unturnedPlayer);
            return false;
        }

        /// <summary>
        /// Faster way, than <see cref="InsidePlayers"/>, to find out if a player is inside the zone, because of using <see cref="HashSet{T}"/>
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the player is inside the zone, otherwise <see langword="false"/>
        /// <para> Converts to <see cref="IsPositionInside(Vector3)"/> where <see cref="Vector3"/> is <see cref="UnturnedPlayer.Position"/> if <see cref="ShouldUpdatePlayersPositions"/> set to <see langword="false"/> </para>
        /// </returns>
        public bool IsPlayerInside(UnturnedPlayer unturnedPlayer)
        {
            return ShouldUpdatePlayersPositions ? InsidePlayersSteamIDs.Contains(unturnedPlayer.CSteamID) : IsPositionInside(unturnedPlayer.Position);
        }

        private void AddPlayer(UnturnedPlayer unturnedPlayer)
        {
            if (InsidePlayersSteamIDs.Add(unturnedPlayer.CSteamID))
            {
                InvokeOnPlayerEntered(unturnedPlayer);
            }
        }

        private void RemovePlayer(UnturnedPlayer unturnedPlayer)
        {
            if (InsidePlayersSteamIDs.Remove(unturnedPlayer.CSteamID))
            {
                InvokeOnPlayerExited(unturnedPlayer);
            }
        }

        private void InvokeOnPlayerEntered(UnturnedPlayer unturnedPlayer)
        {
            PSZones.Events.InternalInvokeOnPlayerEntered(this, unturnedPlayer);

            OnPlayerEntered?.TryInvoke($"{nameof(OnPlayerEntered)} | {Name} | {unturnedPlayer.CSteamID}", unturnedPlayer);
        }

        private void InvokeOnPlayerExited(UnturnedPlayer unturnedPlayer)
        {
            PSZones.Events.InternalInvokeOnPlayerExited(this, unturnedPlayer);

            OnPlayerExited?.TryInvoke($"{nameof(OnPlayerExited)} | {Name} | {unturnedPlayer.CSteamID}", unturnedPlayer);
        }
    }
}
