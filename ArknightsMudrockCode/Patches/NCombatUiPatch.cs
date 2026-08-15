#region

using ArknightsMudrock.ArknightsMudrockCode.Nodes;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Nodes.Combat;

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
    }
}