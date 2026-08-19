#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Hooks;

public static class MudrockHooks
{
    public static Task AfterShieldGained(ICombatState combatState, Player player)
    {
        return HookUtils.Dispatch<IAfterShieldGained>(combatState, model => model.AfterShieldGained(combatState, player));
    }

    public static Task AfterShieldLost(PlayerChoiceContext choiceContext, Player player, Creature? source, ValueProp? props = null)
    {
        return HookUtils.Dispatch<IAfterShieldLost>(player.Creature.CombatState, model => model.AfterShieldLost(choiceContext, player, source, props));
    }
}
