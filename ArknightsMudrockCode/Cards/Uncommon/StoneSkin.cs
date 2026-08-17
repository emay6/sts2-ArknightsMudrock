#region

using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public class StoneSkin() : ArknightsMudrockCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlatingPower>(4M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, this, DynamicVars.Power<PlatingPower>().BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<PlatingPower>().UpgradeValueBy(2M);
    }
}