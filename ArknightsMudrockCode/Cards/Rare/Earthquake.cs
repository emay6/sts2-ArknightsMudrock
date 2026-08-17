#region

using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class Earthquake() : ArknightsMudrockCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllEnemies)
{   
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<QuakePower>(22)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuakePower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (CombatState != null)
        {
            foreach (var target in CombatState.HittableEnemies)
            {
                await CommonActions.Apply<QuakePower>(choiceContext, target, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<QuakePower>().UpgradeValueBy(8);
    }
}