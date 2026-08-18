#region

using ArknightsMudrock.ArknightsMudrockCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class RubbleRain() : ArknightsMudrockCard(2, 
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<GiantRock>(IsUpgraded),
        HoverTipFactory.FromCard<Debris>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        
        var transformableCards = MudrockUtils.GetDeckInCombat(Owner).Where(c => c is Debris && c.IsTransformable);

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
        }
    }
}