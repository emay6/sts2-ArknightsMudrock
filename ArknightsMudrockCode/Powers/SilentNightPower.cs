#region

using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class SilentNightPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(0)];

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;

        var attacksPlayed = CombatManager.Instance.History.CardPlaysFinished.Count(e =>
            e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner.Creature == Owner &&
            e.HappenedThisTurn(CombatState));

        if (attacksPlayed == 0 && Owner.IsPlayer)
        {
            Flash();
            await ShieldCmd.GainShield(Amount, Owner.Player!);
        }
    }
}