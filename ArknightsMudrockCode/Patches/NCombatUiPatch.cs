#region

using ArknightsMudrock.ArknightsMudrockCode.Nodes;
using ArknightsMudrock.ArknightsMudrockCode.Utils;
using MudrockCharacter = ArknightsMudrock.ArknightsMudrockCode.Character.ArknightsMudrock;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

[HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
public class NCombatUiPatch
{
    [HarmonyPostfix]
    private static void Postfix(NCombatUi __instance, CombatState state)
    {
        var shieldCounter = MudrockAddedNodes.NShieldIcon[__instance];
        if (!GodotObject.IsInstanceValid(shieldCounter))
        {
            // Same stale-cache hazard as NShieldRings below -- see the comment
            // there for why AddedNode can hand back an already-disposed instance.
            shieldCounter = PreloadManager.Cache.GetScene(MudrockResources.NShieldIconPath)
                .Instantiate<NShieldIcon>();
            __instance.AddChild(shieldCounter);
        }
        shieldCounter.Initialize(LocalContext.GetMe(state)!);
        shieldCounter.Reparent(__instance.EnergyCounterContainer);
        shieldCounter.Position = new Vector2(60, -100);
        shieldCounter.Size = new Vector2(80, 80);

        var players = state.Players.ToList();
        var localPlayer = LocalContext.GetMe(state)!;
        var playersByCreature = players.ToDictionary(player => player.Creature);
        var playerVisuals = FindPlayerVisuals(NCombatRoom.Instance?._allyContainer);

        // Match each ally visual to its owning player by walking up to its
        // NCreature ancestor and reading the entity it's backing, instead of
        // pairing playerCreatures[i] with playerVisuals[i] by list position.
        // Position-based pairing only worked for the local player (via the
        // old AlignLocalPlayerVisual swap) and had no guarantee the ally
        // container's child order matched state.PlayerCreatures for anyone
        // else -- e.g. join order, reconnects, or another mod reordering
        // ally visuals could silently attach shield-ring data to the wrong
        // player's creature. Matching on the actual backing entity is
        // correct regardless of child order.
        foreach (var visual in playerVisuals)
        {
            if (FindCreatureAncestor(visual)?.Entity is not { IsPlayer: true } entity) continue;
            if (!playersByCreature.TryGetValue(entity, out var player)) continue;

            var shieldRings = MudrockAddedNodes.NShieldRings[visual];
            if (!GodotObject.IsInstanceValid(shieldRings))
            {
                // AddedNode<NCreatureVisuals, NShieldRings> caches one instance per
                // visual and has no way to know when that instance has freed itself
                // independently (NShieldRings._Process calls QueueFree() once its
                // player's creature is no longer alive). If a stale, already-disposed
                // instance comes back from the cache -- which happens when re-entering
                // combat from certain event flows, e.g. the Mysterious Knight event's
                // follow-up VisualOnly encounter -- calling Reparent/Initialize on it
                // throws ObjectDisposedException. Build and attach a replacement the
                // same way the AddedNode factory would, rather than trusting the cache.
                shieldRings = PreloadManager.Cache.GetScene(MudrockResources.NShieldRingsPath)
                    .Instantiate<NShieldRings>();
                visual.AddChild(shieldRings);
            }
            // Vanilla characters (and well-behaved mods) nest a "Visuals" Node2D
            // inside their NCreatureVisuals scene. Some third-party character mods
            // convert a bare Node2D directly into NCreatureVisuals with no such
            // child, so fall back to the NCreatureVisuals node itself in that case
            // -- it's a Node2D too, so it works as an anchor either way.
            var characterVisuals = visual.GetNodeOrNull<Node2D>("Visuals") ?? visual;
            shieldRings.Reparent(characterVisuals, false);
            // Counter the complete creature transform so the 512px ring art
            // has the same screen size for every character archetype.
            var visualScale = characterVisuals.GlobalScale;
            var inverseScale = new Vector2(
                visualScale.X == 0f ? 1f : 1f / visualScale.X,
                visualScale.Y == 0f ? 1f : 1f / visualScale.Y);
            shieldRings.Position = new Vector2(-255f * inverseScale.X, -385f * inverseScale.Y);
            shieldRings.Scale = inverseScale;
            // The rings are drawn after the sprite within Visuals, while the
            // health and power UI are outside this subtree and draw above it.
            shieldRings.ZIndex = 0;
            shieldRings.Initialize(player, player.NetId == localPlayer.NetId);
        }
    }

    private static List<NCreatureVisuals> FindPlayerVisuals(Node? node)
    {
        var visuals = new List<NCreatureVisuals>();
        if (node == null) return visuals;
        CollectCreatureVisuals(node, visuals);
        return visuals;
    }

    private static void CollectCreatureVisuals(Node node, List<NCreatureVisuals> result)
    {
        foreach (var child in node.GetChildren())
        {
            if (child is NCreatureVisuals creatureVisuals)
                result.Add(creatureVisuals);

            CollectCreatureVisuals(child, result);
        }
    }

    private static NCreature? FindCreatureAncestor(Node node)
    {
        while (node != null)
        {
            if (node is NCreature creature) return creature;
            node = node.GetParent();
        }

        return null;
    }

}