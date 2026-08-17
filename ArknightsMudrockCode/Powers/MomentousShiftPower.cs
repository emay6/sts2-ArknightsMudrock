#region

using ArknightsMudrock.ArknightsMudrockCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class MomentousShiftPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override object? InitInternalData() => new Data();

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (ShouldSkip(card))
        {
            modifiedCost = originalCost;
            return false;
        }
        else
        {
            modifiedCost = 0;
            return true;
        }
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner
            && cardPlay is { IsAutoPlay: false, IsLastInSeries: true }
            && cardPlay.Card.Keywords.Contains(MudrockKeywords.Inertial))
        {
            ++GetInternalData<Data>().inertialCardsPlayedThisTurn;
        }

        return Task.CompletedTask;
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return Task.CompletedTask;
        GetInternalData<Data>().inertialCardsPlayedThisTurn = 0;
        return Task.CompletedTask;
    }

    private bool ShouldSkip(CardModel card)
    {
        if (card.Owner.Creature != Owner 
            || !card.Keywords.Contains(MudrockKeywords.Inertial) 
            || (card.Pile?.Type != PileType.Hand && card.Pile?.Type != PileType.Play)) return true;
        
        return GetInternalData<Data>().inertialCardsPlayedThisTurn >= Amount;
    }

    private class Data
    {
        public int inertialCardsPlayedThisTurn;
    }
}