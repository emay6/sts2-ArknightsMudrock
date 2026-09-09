using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

/// TODO: Does not work properly in multiplayer.
public class RedirectPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override object? InitInternalData() => new Data();
    
    public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
    {
        if (attack._sourceType != AttackCommand.SourceType.Monster || !attack.DamageProps.IsPoweredAttack())
            return hitCount;
        
        var shieldAmount = Owner.Player?.PlayerCombatState?.ShieldState()?.Shields;
        if (shieldAmount > 0)
        {
            return shieldAmount.Value;
        }

        return hitCount;

    }

    public override decimal ModifyHpLostBeforeOsty(Creature target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        var shieldAmount = Owner.Player?.PlayerCombatState?.ShieldState()?.Shields;

        if (shieldAmount == 0
            || target != Owner
            || dealer == null
            || !dealer.IsMonster
            || !props.IsPoweredAttack()
           ) return amount;
        
        var intent = dealer.Monster!.NextMove.Intents.FirstOrDefault(intent => intent is AttackIntent) as AttackIntent;
        if (intent == null) return amount;

        if (GetInternalData<Data>().initalHit)
        {
            GetInternalData<Data>().damageToDeal =
                (decimal) intent.GetIntentLabel(CombatState.PlayerCreatures, dealer).Variables["Damage"];
            GetInternalData<Data>().initalHit = false;
        }

        return GetInternalData<Data>().damageToDeal;
        
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        // AfterAttack can be observed by powers outside the attacking creature
        // in multiplayer. Only consume Redirect for an attack that actually
        // hit this power's owner.
        if (command.Attacker == Owner
            || command._sourceType != AttackCommand.SourceType.Monster
            || !command.Results.SelectMany(results => results).Any(result => result.Receiver == Owner))
            return;

        await PowerCmd.Decrement(this);
        GetInternalData<Data>().initalHit = true;
    }

    private class Data
    {
        public bool initalHit = true;
        public decimal damageToDeal;
    }
}
