#region

using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class UnstoppableObject() : ArknightsMudrockCard(5,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12, ValueProp.Move),
        new PowerVar<IntangiblePower>(1)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
        await CommonActions.ApplySelf<IntangiblePower>(choiceContext, this, DynamicVars.Power<IntangiblePower>().BaseValue);
        
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}