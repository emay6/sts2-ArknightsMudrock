#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class MercenaryPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override object? InitInternalData() => new Data();

    public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature? target,
        CardModel? cardSource)
    {
        if (target != Owner || power is not MomentumPower || GetInternalData<Data>().gainedMomentumThisTurn >= Amount) return 0;
        
        ++GetInternalData<Data>().gainedMomentumThisTurn;
        return Amount;
    }

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return Task.CompletedTask;
        
        GetInternalData<Data>().gainedMomentumThisTurn = 0;
        return Task.CompletedTask;
    }

    private class Data
    {
        public int gainedMomentumThisTurn;
    }
}