#region

using ArknightsMudrock.ArknightsMudrockCode.Nodes;
using MudrockCharacter = ArknightsMudrock.ArknightsMudrockCode.Character.ArknightsMudrock;
using Godot;
using HarmonyLib;
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
        shieldCounter.Initialize(LocalContext.GetMe(state)!);
        shieldCounter.Reparent(__instance.EnergyCounterContainer);
        shieldCounter.Position = new Vector2(60, -100);
        shieldCounter.Size = new Vector2(80, 80);

        var players = state.Players.ToList();
        var localPlayer = LocalContext.GetMe(state)!;
        var playersByCreature = players.ToDictionary(player => player.Creature);
        var playerCreatures = state.PlayerCreatures.ToList();
        var playerVisuals = FindPlayerVisuals(NCombatRoom.Instance?._allyContainer, playerCreatures.Count);
        AlignLocalPlayerVisual(playerCreatures, playerVisuals, localPlayer);

        for (var index = 0; index < Math.Min(playerCreatures.Count, playerVisuals.Count); index++)
        {
            if (!playersByCreature.TryGetValue(playerCreatures[index], out var player)) continue;

            var shieldRings = MudrockAddedNodes.NShieldRings[playerVisuals[index]];
            var characterVisuals = playerVisuals[index].GetNode<Node2D>("Visuals");
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

    private static List<NCreatureVisuals> FindPlayerVisuals(Node? node, int playerCount)
    {
        var visuals = new List<NCreatureVisuals>();
        if (node == null) return visuals;
        CollectCreatureVisuals(node, visuals);
        // Preserve the combat room's ally-container order. Sorting by screen
        // position can disagree with player-creature order on the host.
        return visuals.Take(playerCount).ToList();
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

    private static void AlignLocalPlayerVisual(
        List<MegaCrit.Sts2.Core.Entities.Creatures.Creature> playerCreatures,
        List<NCreatureVisuals> visuals,
        MegaCrit.Sts2.Core.Entities.Players.Player localPlayer)
    {
        var localPlayerIndex = playerCreatures.FindIndex(creature => creature == localPlayer.Creature);
        var localVisualIndex = visuals.FindIndex(visual =>
            FindCreatureAncestor(visual)?._isRemotePlayerOrPet == false);

        if (localPlayerIndex < 0 || localVisualIndex < 0 || localPlayerIndex == localVisualIndex) return;

        (visuals[localPlayerIndex], visuals[localVisualIndex]) =
            (visuals[localVisualIndex], visuals[localPlayerIndex]);
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
