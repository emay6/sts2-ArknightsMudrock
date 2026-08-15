#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Hooks;

public interface IAfterShieldLost
{
    public Task  AfterShieldLost(ICombatState combatState, Player player, Creature? source = null);
}