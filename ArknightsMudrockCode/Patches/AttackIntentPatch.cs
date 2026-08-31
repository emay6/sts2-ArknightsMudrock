using ArknightsMudrock.ArknightsMudrockCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

[HarmonyPatch(typeof(AttackIntent), "GetIntentDescription")]
public class AttackIntentPatch
{
    [HarmonyPostfix]
    private static void Postfix(SingleAttackIntent __instance, IEnumerable<Creature> targets, Creature owner, ref LocString __result)
    {
        var (hitCount, totalDamage) = MudrockUtils.CalculateRedirectData(__instance, targets, owner);
        if (hitCount != null) 
        {
            __result.Add("Damage", (int)(totalDamage / hitCount.Value));
            __result.Add("Repeat", hitCount.Value);
        }
    }
}