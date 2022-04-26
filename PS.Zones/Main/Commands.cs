using DI.Sources.Extensions;
using PS.Zones.Extensions;
using PS.Zones.Models.Perform;
using Rocket.API;
using Rocket.Core.Commands;
using System;

namespace PS.Zones.Main
{
    public sealed partial class Plugin
    {
        // TODO: Add commands explanation file

        #region /zones        

        [RocketCommand(Name: "zones", Help: "", Syntax: "", AllowedCaller.Player)]
        [RocketCommandPermission("zones")]
        public void ExecuteZones(IRocketPlayer caller, string[] parameters)
        {
            var sender = caller.ToUnturnedPlayer();
            var message = string.Join(" ", parameters.ToLower());

            try
            {
                if (message.Contains("help"))
                {
                    SendCommandHelpTo(caller);
                    return;
                }

                if (message.Contains("add zone"))
                {
                    string zoneName = parameters[3];
                    Zone newZone;

                    switch (parameters[2].ToLower())
                    {
                        case "polygon":
                            newZone = new PolygonZone(zoneName);
                            break;
                        case "round":
                            var radius = float.Parse(parameters[4]);
                            newZone = new RoundZone(zoneName, sender.Position, radius);
                            break;
                        default: throw new ArgumentException();
                    }

                    if (Instance.Context.TryAddZone(newZone) == false)
                    {
                        SayTo(sender, $"<color=yellow>There's already a zone with the name</color> {zoneName}");
                        return;
                    }

                    SayTo(sender, "<color=green>New zone successfully created!</color>");
                    return;
                }

                if (message.Contains("add point"))
                {
                    var zoneName = parameters[2];

                    if (Instance.Context.TryGetZoneByName(zoneName, out Zone zone) == false)
                    {
                        SayTo(sender, $"<color=yellow>There's no zone with name</color> {zoneName}");
                        return;
                    }

                    if (zone is PolygonZone polygonZone)
                    {
                        polygonZone.AddPoint(sender.Position);

                        SayTo(sender, "<color=green>New point successfully created at your position!</color>");
                        return;
                    }

                    SayTo(sender, "<color=yellow>The selected zone is not polygonal!</color>");
                    return;
                }

                if (message.Contains("remove zone"))
                {
                    string zoneName = parameters[2];

                    if(Instance.Context.RemoveZoneByName(zoneName) == false)
                    {
                        SayTo(sender, $"<color=green>Zone named</color> {zoneName} <color=green>has been deleted successfully!</color>");
                        return;
                    }

                    SayTo(sender, $"<color=yellow>There's no zone with name</color> {zoneName}");
                    return;
                }

                if (message.Contains("modify"))
                {
                    string zoneName = parameters[1];

                    if (Instance.Context.TryGetZoneByName(zoneName, out Zone zone) == false)
                    {
                        SayTo(sender, $"<color=yellow>There's no zone with name</color> {zoneName}");
                        return;
                    }

                    if (zone is RoundZone roundZone)
                    {
                        switch (parameters[2].ToLower())
                        {
                            case "center":
                                roundZone.Center = sender.Position.ToPoint();
                                SayTo(sender, $"<color=green>The center of the zone has been successfully moved to your position!</color>");
                                return;
                            case "radius":
                                var newRadius = float.Parse(parameters[3]);
                                roundZone.Radius = newRadius;
                                SayTo(sender, $"<color=green>The radius of the zone has been successfully changed to {newRadius}!</color>");
                                return;
                            default: throw new ArgumentException();
                        }
                    }

                    throw new ArgumentException();
                }

                throw new ArgumentException();
            }
            catch
            {
                SayTo(sender, "<color=yellow>Someting went wrong! Use</color> /zones help <color=yellow>to view instructions!</color>");
            }
        }

        #endregion
    }
}
