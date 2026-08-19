#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Hooks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Commands;

public static class ShieldCmd
{
    public static async Task GainShield(int amount, Player player, CardPlay? cardPlay = null)
    {
        if (amount > 0 && !CombatManager.Instance.IsEnding && player.Creature.CombatState != null)
        {
            var shieldState = player.PlayerCombatState?.ShieldState();
            if (shieldState?.Shields == shieldState?.MaxShieldCount) return;
            
            shieldState?.Shields += amount;
            await MudrockHooks.AfterShieldGained(player.Creature.CombatState, player);
        }
    }

    public static async Task LoseShield(int amount, Player player, Creature? source = null, CardPlay? cardPlay = null)
    {
        if (amount > 0 && !CombatManager.Instance.IsEnding && player.Creature.CombatState != null)
        {
            var shieldState = player.PlayerCombatState?.ShieldState();
            if (shieldState?.Shields == 0) return;
            
            shieldState?.Shields -= amount;
            await MudrockHooks.AfterShieldLost(new HookPlayerChoiceContext(player, player.NetId, GameActionType.Combat), player, source);
        }
    }
    
    public static async Task SetShieldValue(int amount, Player player, CardPlay? cardPlay = null)
    {
        if (!CombatManager.Instance.IsEnding && player.Creature.CombatState != null)
        {
            var shieldState = player.PlayerCombatState?.ShieldState();
            shieldState?.ShieldValue = amount;
        }
    }

    public static async Task SetMaxShieldAmount(int amount, Player player, CardPlay? cardPlay = null)
    {
        if (!CombatManager.Instance.IsEnding && player.Creature.CombatState != null)
        {
            var shieldState = player.PlayerCombatState?.ShieldState();
            shieldState?.MaxShieldCount = amount;
        }
    }
}