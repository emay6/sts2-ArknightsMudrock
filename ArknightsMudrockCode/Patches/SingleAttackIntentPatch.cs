using ArknightsMudrock.ArknightsMudrockCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace ArknightsMudrock.ArknightsMudrockCode.Patches;

[HarmonyPatch(typeof(SingleAttackIntent), nameof(SingleAttackIntent.GetIntentLabel))]
public class SingleAttackIntentPatch
{
    [HarmonyPostfix]
    private static void Postfix(MultiAttackIntent __instance, IEnumerable<Creature> targets, Creature owner, ref LocString __result)
    {
        var targetList = targets.ToList();
        if (targetList.Any(c => c.HasPower<RedirectPower>()))
        {
            LocString newIntentLabel = new LocString("intents", "FORMAT_DAMAGE_MULTI");
            newIntentLabel.Add("Damage", __instance.GetTotalDamage(targetList, owner));
            newIntentLabel.Add("Repeat", __instance.Repeats * 2);
            __result = newIntentLabel;
        }
    }
}