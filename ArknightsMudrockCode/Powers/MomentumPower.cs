#region

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class MomentumPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner.Creature == this.Owner && card.Type == CardType.Attack)
        {
            modifiedCost = originalCost - this.Amount;
            return true;
        }

        modifiedCost = originalCost;
        return false;
    }
}