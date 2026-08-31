using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Powers;
using ArknightsMudrock.ArknightsMudrockCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

[HarmonyPatch(typeof(SingleAttackIntent), nameof(SingleAttackIntent.GetIntentLabel))]
public class SingleAttackIntentPatch
{
    [HarmonyPostfix]
    private static void Postfix(SingleAttackIntent __instance, IEnumerable<Creature> targets, Creature owner, ref LocString __result)
    {
        var (hitCount, totalDamage) = MudrockUtils.CalculateRedirectData(__instance, targets, owner);
        if (hitCount > 1) {
            LocString newIntentLabel = new LocString("intents", "FORMAT_DAMAGE_MULTI");
            newIntentLabel.Add("Damage", (int)(totalDamage / hitCount.Value));
            newIntentLabel.Add("Repeat", hitCount.Value);
            __result = newIntentLabel;
        }
    }
}