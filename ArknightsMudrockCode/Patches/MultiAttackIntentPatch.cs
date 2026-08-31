using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Powers;
using ArknightsMudrock.ArknightsMudrockCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

[HarmonyPatch(typeof(MultiAttackIntent), nameof(MultiAttackIntent.GetIntentLabel))]
public class MultiAttackIntentPatch
{
    [HarmonyPostfix]
    private static void Postfix(MultiAttackIntent __instance, IEnumerable<Creature> targets, Creature owner, ref LocString __result)
    {
        var (hitCount, totalDamage) = MudrockUtils.CalculateRedirectData(__instance, targets, owner);
        if (hitCount > 1) 
        {
            __result.Add("Damage", (int)(totalDamage / hitCount.Value));
            __result.Add("Repeat", hitCount.Value);
        } 
        else if (hitCount == 1)
        {
            LocString newIntentLabel = new LocString("intents", "FORMAT_DAMAGE_SINGLE");
            newIntentLabel.Add("Damage", totalDamage);
            __result = newIntentLabel;
        }
    }
}