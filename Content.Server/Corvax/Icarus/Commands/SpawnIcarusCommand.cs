﻿using Content.Server.Administration;
using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Console;
using Robust.Shared.Map;

namespace Content.Server.Corvax.Icarus.Commands;

[UsedImplicitly]
[AdminCommand(AdminFlags.Fun)]
public sealed class SpawnIcarusCommand : IConsoleCommand
{
    public string Command => "spawnicarus";
    public string Description => "Spawn Icarus beam and direct to specified grid center.";
    public string Help => "spawnicarus <gridId>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError("Incorrect number of arguments. " + Help);
            return;
        }
        
        if (!EntityUid.TryParse(args[0], out var uid))
        {
            shell.WriteError("Not a valid entity ID.");
            return;
        }

        var entityManager = IoCManager.Resolve<IEntityManager>();
        if (!entityManager.EntityExists(uid))
        {
            shell.WriteError("That grid does not exist.");
            return;
        }
        
        var mapManager = IoCManager.Resolve<IMapManager>();
        if (mapManager.TryGetGrid(uid, out var grid))
        {
            var icarusSystem = IoCManager.Resolve<IEntityManager>().System<IcarusTerminalSystem>();
            var coords = icarusSystem.FireBeam(grid.LocalAABB);
            shell.WriteLine($"Icarus was spawned: {coords.ToString()}");
        }
        else
        {
            shell.WriteError($"No grid exists with ID {uid}");
        }
    }
}
