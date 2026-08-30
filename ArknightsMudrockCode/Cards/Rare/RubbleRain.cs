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
        /*new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("HitCount").WithMultiplier((card, _) => MudrockUtils.GetDeckInCombat(Owner).Count(c => c.IsTransformable && c is Debris or GiantRock))*/
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

        //var numHits = (int)((CalculatedVar)DynamicVars["HitCount"]).Calculate(play.Target);
        
        await CommonActions.CardAttack(this, play, hitCount: numHits, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);

        /*var transformableCards = MudrockUtils.GetDeckInCombat(Owner).Where(c => c is Debris && c.IsTransformable);

        if (CombatState != null)
        {
            foreach (var originalCard in transformableCards)
            {
                var transformCard = CombatState.CreateCard<GiantRock>(Owner);
                if (IsUpgraded) CardCmd.Upgrade(transformCard);
                await CardCmd.Transform(originalCard, transformCard);
            }
        }

        // kinda arbitrary so can revisit
        await Cmd.CustomScaledWait(0.2f, 0.4f);

        var giantRockCards = MudrockUtils.GetDeckInCombat(Owner).Where(c => c is GiantRock && !c.Keywords.Contains(CardKeyword.Unplayable));
        foreach (var giantRock in giantRockCards)
        {
            await CardCmd.AutoPlay(choiceContext, giantRock, null);
        }*/
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}