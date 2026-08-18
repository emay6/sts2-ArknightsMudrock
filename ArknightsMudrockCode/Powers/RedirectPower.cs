using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

/// TODO: Does not currently work in multiplayer (players without power will see and recieve additional hits without the damage reduction)
public class RedirectPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageDecrease", 0.5M)];

    protected override object? InitInternalData() => new Data();
    
    public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
    {
        GetInternalData<Data>().redirectActivated = false;
        if (attack._sourceType != AttackCommand.SourceType.Monster || !attack.DamageProps.IsPoweredAttack())
            return hitCount;
        
        var players = attack.GetPossibleTargets().Select(c => c.Player!);

        if (!LocalContext.GetMe(players)?.Creature.HasPower<RedirectPower>() ?? true)
            return hitCount;
        
        GetInternalData<Data>().redirectActivated = true;
        return hitCount * 2;
        
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        return (target == null || !target.IsPlayer || target != Owner || !props.IsPoweredAttack() || !target.HasPower<RedirectPower>())
            ? 1 
            : DynamicVars["DamageDecrease"].BaseValue;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (GetInternalData<Data>().redirectActivated)
        {
            await PowerCmd.Decrement(this);
            GetInternalData<Data>().redirectActivated = false;
        }
    }

    private class Data
    {
        public bool redirectActivated;
    }
}