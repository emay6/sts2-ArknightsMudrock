#region

using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Common;

public class ShapeTheEarth() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Debris>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner, play);
        if (CombatState != null)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(
            (CardModel)this.CombatState.CreateCard<Debris>(this.Owner), PileType.Discard, this.Owner));
        await Cmd.Wait(0.5f);
    }
    
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
