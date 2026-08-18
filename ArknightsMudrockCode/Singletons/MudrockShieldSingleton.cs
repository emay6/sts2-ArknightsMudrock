#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Hooks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Singletons;

public class MudrockShieldSingleton() : CustomSingletonModel(HookType.Combat), IAfterShieldLost
{
    public override decimal ModifyHpLostBeforeOsty(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (target == null || !target.IsPlayer || !props.IsPoweredAttack() || amount == 0) return amount;

        var player = target.Player!;
        var combatState = target.CombatState!;
        var playerCombatState = player.PlayerCombatState;
        
        if (playerCombatState == null || playerCombatState.ShieldState()?.Shields == 0) return amount;

        var shieldState = playerCombatState.ShieldState()!;
        
        shieldState.Shields -= 1;
        MudrockHooks.AfterShieldLost(combatState, player, dealer, props);
        return Math.Max(0, amount - shieldState.ShieldValue);
    }

    // default behavior upon losing shield (gaining energy)
    public Task AfterShieldLost(ICombatState combatState, Player player, Creature? source = null, ValueProp? props = null)
    {
        var shieldState = player.PlayerCombatState?.ShieldState();
        var target = player.Creature;
        
        if (shieldState == null) return Task.CompletedTask;
        
        // uses energy next turn power when enemy's turn since otherwise energy is lost
        if (combatState.CurrentSide == CombatSide.Enemy)
        {
            PowerCmd.Apply<EnergyNextTurnPower>(new ThrowingPlayerChoiceContext(), target, shieldState.EnergyValue,
                target, null,
                silent: true);
        } else if (combatState.CurrentSide == CombatSide.Player)
        {
            PlayerCmd.GainEnergy(shieldState.EnergyValue, player);
        }
        
        return Task.CompletedTask;
    }
}