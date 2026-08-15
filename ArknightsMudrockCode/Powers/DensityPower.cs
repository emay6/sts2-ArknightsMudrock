#region

using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class DensityPower() : ArknightsMudrockPower
{
    public override PowerType Type => 
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    // maybe change at some point
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new HoverTip(MudrockResources.ShieldLocStringTitle, MudrockResources.ShieldLocStringDescription)];

    public override bool AllowNegative => true;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power.Owner.IsPlayer && power == this)
        {
            var player = power.Owner.Player!;
            var baseValue = Character.ArknightsMudrock.BaseShieldValue;
            await ShieldCmd.SetShieldValue(baseValue + _amount, player);
        }
    }
}