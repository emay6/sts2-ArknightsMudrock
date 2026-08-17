#region

using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class AftershocksPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuakePower>()];
    
    // logic inside QuakePower itself for now
}