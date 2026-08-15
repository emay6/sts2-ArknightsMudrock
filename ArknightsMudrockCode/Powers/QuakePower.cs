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
        List<Creature> creatures = participants.ToList();
        
        if (CombatManager.Instance.IsOverOrEnding || !creatures.Contains(this.Owner) || this.Owner.IsDead) return;

        IEnumerable<Creature> targets = creatures.Where(c => c != this.Owner && c.IsHittable);
        this.Flash();
        foreach (Creature creature in targets)
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature, this.Amount, ValueProp.Unpowered, this.Applier, null);
        }
        
        if (this.Owner.IsAlive)
            await PowerCmd.Remove(this);
        else
            await Cmd.CustomScaledWait(0.1f, 0.25f);
    }
}