#region

using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class KazdelianArts() : ArknightsMudrockCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
        new EnergyVar(1),
        new PowerVar<DensityPower>(1),
        new PowerVar<VigorPower>(2)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<DensityPower>(),
        HoverTipFactory.FromPower<VigorPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CommonActions.Draw(this, choiceContext);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CommonActions.ApplySelf<DensityPower>(choiceContext, this);
        await CommonActions.ApplySelf<VigorPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<DensityPower>().UpgradeValueBy(1);
        DynamicVars.Power<VigorPower>().UpgradeValueBy(2);
    }
}