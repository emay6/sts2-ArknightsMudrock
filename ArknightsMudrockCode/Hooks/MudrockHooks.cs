#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Hooks;

public static class MudrockHooks
{
    public static Task AfterShieldGained(ICombatState combatState, Player player)
    {
        return HookUtils.Dispatch<IAfterShieldGained>(combatState, model => model.AfterShieldGained(combatState, player));
    }

    public static Task AfterShieldLost(ICombatState combatState, Player player, Creature? source)
    {
        return HookUtils.Dispatch<IAfterShieldLost>(combatState, model => model.AfterShieldLost(combatState, player, source));
    }
}