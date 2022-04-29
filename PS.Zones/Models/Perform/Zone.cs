using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PS.Zones.Models.Perform
{
    [Serializable]
    public abstract class Zone
    {
        // TODO: Code refactoring. 
        // Block frames position update override.
        // In case if zone will update position only one time (like using commands, timers, etc.)

        public static event Action<Zone, UnturnedPlayer> OnPlayerEntered_Global;

        public static event Action<Zone, UnturnedPlayer> OnPlayerExited_Global;

        public event Action<UnturnedPlayer> OnPlayerEntered;

        public event Action<UnturnedPlayer> OnPlayerExited;

        public string Name
        {
            get => _name;
            protected set => _name = value;
        }

        public List<CSteamID> InsidePlayersSteamID
        {
            get
            {
                var result = new List<CSteamID>();

                foreach (ulong steam64 in InsidePlayersSteam64)
                {
                    result.Add((CSteamID)steam64);
                }

                return result;
            }
        }

        public List<UnturnedPlayer> InsidePlayers
        {
            get
            {
                var result = new List<UnturnedPlayer>();

                foreach (ulong steam64 in InsidePlayersSteam64)
                {
                    var unturnedPlayer = UnturnedPlayer.FromCSteamID((CSteamID)steam64);

                    result.Add(unturnedPlayer);
                }

                return result;
            }
        }

        protected readonly HashSet<ulong> InsidePlayersSteam64;

        private string _name;

        public Zone(string name)
        {
            _name = name;
            InsidePlayersSteam64 = new HashSet<ulong>();
        }

        /// <summary>
        /// Adds a player to InsidePlayers if he is inside the zone, otherwise removes
        /// </summary>
        /// <param name="unturnedPlayer"></param>
        /// <returns>true if the player is inside the zone, otherwise false</returns>
        public bool UpdatePlayerPosition(UnturnedPlayer unturnedPlayer)
        {
            bool isInside = IsPositionInside(unturnedPlayer.Position);

            if (isInside)
            {
                AddPlayer(unturnedPlayer);
                return true;
            }

            RemovePlayer(unturnedPlayer);
            return false;
        }

        public abstract bool IsPositionInside(Vector3 position);

        /// <summary>
        /// Faster way, than InsidePlayers.Contains(unturnedPlayer), to find out if a player is inside the zone, because of using HashSet
        /// </summary>
        /// <param name="unturnedPlayer"></param>
        /// <returns>true if the player is inside the zone, otherwise false</returns>
        public bool IsPlayerInside(UnturnedPlayer unturnedPlayer) => IsPlayerInside((ulong)unturnedPlayer.CSteamID);

        /// <summary>
        /// Faster way, than InsidePlayers.Contains(unturnedPlayer), to find out if a player is inside the zone, because of using HashSet
        /// </summary>
        /// <param name="unturnedPlayer"></param>
        /// <returns>true if the player is inside the zone, otherwise false</returns>
        public bool IsPlayerInside(CSteamID steamID) => IsPlayerInside((ulong)steamID);

        private void AddPlayer(UnturnedPlayer unturnedPlayer)
        {
            var steam64 = (ulong)unturnedPlayer.CSteamID;

            if (InsidePlayersSteam64.Add(steam64)) InvokeOnPlayerEntered(unturnedPlayer);
        }

        private void RemovePlayer(UnturnedPlayer unturnedPlayer)
        {
            var steam64 = (ulong)unturnedPlayer.CSteamID;

            if (InsidePlayersSteam64.Remove(steam64)) InvokeOnPlayerExited(unturnedPlayer);
        }

        private void InvokeOnPlayerEntered(UnturnedPlayer unturnedPlayer)
        {
            OnPlayerEntered_Global?.TryInvoke($"{nameof(OnPlayerEntered_Global)} | {unturnedPlayer.CSteamID}", this, unturnedPlayer);

            OnPlayerEntered?.TryInvoke($"{nameof(OnPlayerEntered)} | {unturnedPlayer.CSteamID}", unturnedPlayer);
        }

        private void InvokeOnPlayerExited(UnturnedPlayer unturnedPlayer)
        {
            OnPlayerExited_Global?.TryInvoke($"{nameof(OnPlayerExited_Global)} | {unturnedPlayer.CSteamID}", this, unturnedPlayer);

            OnPlayerExited?.TryInvoke($"{nameof(OnPlayerExited)} | {unturnedPlayer.CSteamID}", unturnedPlayer);
        }

        private bool IsPlayerInside(ulong steam64) => InsidePlayersSteam64.Contains(steam64);
    }
}
