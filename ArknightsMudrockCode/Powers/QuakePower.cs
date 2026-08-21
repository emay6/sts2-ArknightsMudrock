#region

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class QuakePower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override Color AmountLabelColor => _normalAmountLabelColor;

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        var creatures = participants.ToList();
        
        if (CombatManager.Instance.IsOverOrEnding || !creatures.Contains(Owner) || Owner.IsDead) return;

        var targets = creatures.Where(c => c.IsHittable).ToList();
        // temp solution for palpitations
        var palpitationsActive = Applier?.HasPower<PalpitationsPower>() ?? false;
        // make quake do damage to all enemies, and instead have palpiations have an extra hit
        foreach (var creature in targets/*(palpitationsActive ? targets : targets.Where(c => c != Owner))*/)
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature, Amount, ValueProp.Unpowered, Applier, null);
            if (palpitationsActive)
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature, Amount, ValueProp.Unpowered, Applier, null);
        }
        
        if (Owner.IsAlive)
            await PowerCmd.Remove(this);
        else
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        
        // temp solution for Aftershocks
        var aftershocksActive = Applier?.HasPower<AftershocksPower>() ?? false;
        if (!CombatManager.Instance.IsOverOrEnding && aftershocksActive)
        {
            var newTarget = CombatState.RunState.Rng.CombatTargets.NextItem(targets.Where(c => c != Owner && c.IsHittable));
            if (newTarget != null)
                await PowerCmd.Apply<QuakePower>(choiceContext, newTarget, Amount, Applier, null);
        }
    }
}