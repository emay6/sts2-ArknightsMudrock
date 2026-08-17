#region

using ArknightsMudrock.ArknightsMudrockCode.Powers;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

// work around to calculate damage for Obsidian properly
[HarmonyPatch(typeof(CalculatedVar), nameof(CalculatedVar.Calculate))]
public class CalculatedVarPatch
{
    [HarmonyPostfix]
    private static void Postfix(CalculatedVar __instance, ref decimal __result)
    {
        var owner = (CardModel) __instance._owner!;
        if (owner.DynamicVars.TryGetValue(ObsidianCalculatedDamage.Key, out var obsidianCalculatedDamage))
        {
            var densityAmount = owner.DynamicVars.Power<DensityPower>().BaseValue;
            var damage = owner.DynamicVars.ExtraDamage.BaseValue;

            obsidianCalculatedDamage.BaseValue = __result + (densityAmount * damage);
            obsidianCalculatedDamage.WasJustUpgraded = true;
        }
    }
}