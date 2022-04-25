﻿using DI.Sources.Core.Plugins;
using DI.Sources.Extensions.Coroutines;
using PS.Zones.Models.Perform;
using Rocket.Unturned.Player;
using System.Collections;
using System.Collections.Generic;

namespace PS.Zones.Main
{
    public sealed partial class Plugin : Plugin<DBContext, Configuration>
    {
        public const int UPDATE_PER_FRAMES = 15;

        public Plugin Instance { get; private set; }

        private readonly List<UnturnedPlayer> _players = new List<UnturnedPlayer>();

        protected override void Load()
        {
            Instance = this;

            StartCoroutine(PositionUpdater());
        }

        protected override void Unload()
        {
            StopAllCoroutines();

            Instance = null;
        }

        private IEnumerator PositionUpdater()
        {
            while (true)
            {
                yield return new WaitForFrames(UPDATE_PER_FRAMES);

                foreach (UnturnedPlayer unturnedPlayer in Instance._players)
                {
                    foreach (Zone zone in Instance.Context.zones)
                    {
                        zone.UpdatePlayerPosition(unturnedPlayer);
                    }
                }
            }
        }
    }
}