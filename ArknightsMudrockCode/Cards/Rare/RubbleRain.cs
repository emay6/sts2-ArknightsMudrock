#region

using ArknightsMudrock.ArknightsMudrockCode.Utils;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class RubbleRain() : ArknightsMudrockCard(2, 
    CardType.Attack, CardRarity.Rare,
    TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(16, ValueProp.Move),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<GiantRock>(),
        HoverTipFactory.FromCard<Debris>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        var validCards = MudrockUtils.GetDeckInCombat(Owner)
            .Where(c => c.IsTransformable && c is Debris or GiantRock).ToList();
        var numHits = validCards.Count();

        foreach (var card in validCards)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }

        await CommonActions.CardAttack(this, play, hitCount: numHits, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}