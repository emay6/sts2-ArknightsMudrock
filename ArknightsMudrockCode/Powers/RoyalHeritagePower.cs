using ArknightsMudrock.ArknightsMudrockCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class RoyalHeritagePower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override object? InitInternalData() => new Data();

    // uncomment to have the power retroactively check if momentum has already been gained the same turn it's played
    // public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    // {
    //     GetInternalData<Data>().gainedMomentumThisTurn = CombatManager.Instance.History.Entries.Any(e =>
    //         e.HappenedThisTurn(CombatState) && e.Actor == Owner && (e as PowerReceivedEntry)?.Power is MomentumPower);
    //     return Task.CompletedTask;
    // }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power.Owner != Owner || power is not MomentumPower || GetInternalData<Data>().gainedMomentumThisTurn) return;

        GetInternalData<Data>().gainedMomentumThisTurn = true;
        await PowerCmd.Apply<MomentumPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, applier, cardSource);
    }

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return Task.CompletedTask;
        
        GetInternalData<Data>().gainedMomentumThisTurn = false;
        return Task.CompletedTask;
    }

    private class Data
    {
        public bool gainedMomentumThisTurn;
    }
}