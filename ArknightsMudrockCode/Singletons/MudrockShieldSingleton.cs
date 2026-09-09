#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Hooks;
using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Abstracts;
using GodotPlugins.Game;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Singletons;

public class MudrockShieldSingleton() : CustomSingletonModel(HookType.Combat), IAfterShieldGained, IAfterShieldLost
{
    private static readonly string[] ShieldBreakSounds =
    [
        "../ArknightsMudrock/sfx/shield_break_1.ogg",
        "../ArknightsMudrock/sfx/shield_break_2.ogg",
        "../ArknightsMudrock/sfx/shield_break_3.ogg"
    ];

    private const string ShieldGainSound = "../ArknightsMudrock/sfx/shield_gain.ogg";

    public override decimal ModifyHpLostBeforeOsty(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (target == null || !target.IsPlayer || !props.IsPoweredAttack() || amount < 1) return amount;

        var player = target.Player!;
        var playerCombatState = player.PlayerCombatState;
        
        if (playerCombatState == null || playerCombatState.ShieldState()?.Shields == 0) return amount;

        var shieldState = playerCombatState.ShieldState()!;

        // player will receive damage reduction but will not lose their shield after enemy attacks if they have this power
        if (dealer?.IsEnemy == false || !target.HasPower<UnshakableSolidarityPower>())
        {
            shieldState.Shields -= 1;
            MudrockHooks.AfterShieldLost(new HookPlayerChoiceContext(player, player.NetId, GameActionType.Combat), player, 1, dealer, props);
        }
        return Math.Max(0, amount - shieldState.ShieldValue);
    }

    // default behavior upon losing shield (gaining energy)
    public async Task AfterShieldLost(PlayerChoiceContext choiceContext, Player player, int amount, Creature? source = null, ValueProp? props = null)
    {
        if (amount > 0)
            NDebugAudioManager.Instance?.Play(Rng.Chaotic.NextItem(ShieldBreakSounds)!);

        var combatState = player.Creature.CombatState;
        var shieldState = player.PlayerCombatState?.ShieldState();
        var target = player.Creature;
        
        if (combatState == null || shieldState == null) return;
        
        // uses energy next turn power when enemy's turn since otherwise energy is lost
        if (combatState.CurrentSide == CombatSide.Enemy)
        {
            // hits will only ever reduce by one, so no extra calculation needed
            await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, target, shieldState.EnergyValue,
                target, null,
                silent: true);
        } else if (combatState.CurrentSide == CombatSide.Player)
        {
            await PlayerCmd.GainEnergy(shieldState.EnergyValue * amount, player);
        }
    }

    public Task AfterShieldGained(ICombatState combatState, Player player)
    {
        NDebugAudioManager.Instance?.Play(ShieldGainSound);
        return Task.CompletedTask;
    }
}
