#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Fields;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

[HarmonyPatch(typeof(PlayerCombatState), MethodType.Constructor)]
[HarmonyPatch([typeof(Player)])]
public class PlayerCombatStatePatches
{
    [HarmonyPostfix]
    private static void Postfix(Player player, PlayerCombatState __instance)
    {
        var shieldCombatState = new PlayerCombatStateExtension.ShieldCombatState(__instance);
        
        MudrockField.ShieldCombatState[__instance] = shieldCombatState;
        
        CombatManager.Instance.StateTracker.SubscribeShieldChanged(shieldCombatState);
        CombatManager.Instance.StateTracker.SubscribeShieldEnergyChanged(shieldCombatState);
        CombatManager.Instance.StateTracker.SubscribeShieldValueChanged(shieldCombatState);
    }
}