using ArknightsMudrock.ArknightsMudrockCode.Cards;
using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Powers;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public class SetInStone() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ShieldVar(1),
        new PowerVar<DensityPower>(1),
        new PowerVar<NoMomentumPower>(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<DensityPower>(),
        HoverTipFactory.FromPower<MomentumPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner, play);
        //await CommonActions.Draw(this, choiceContext);
        await CommonActions.ApplySelf<DensityPower>(choiceContext, this);
        await CommonActions.ApplySelf<NoMomentumPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<DensityPower>().UpgradeValueBy(1);
    }
}