using ArknightsMudrock.ArknightsMudrockCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

[HarmonyPatch(typeof(MultiAttackIntent), nameof(MultiAttackIntent.GetIntentLabel))]
public class MultiAttackIntentPatch
{
    [HarmonyPostfix]
    private static void Postfix(MultiAttackIntent __instance, IEnumerable<Creature> targets, ref LocString __result)
    {
        if (targets.Any(c => c.HasPower<RedirectPower>()))
        {
            __result.Add("Repeat", __instance.Repeats * 2);
        }
    }
}