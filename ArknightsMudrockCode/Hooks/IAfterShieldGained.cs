#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Hooks;

public interface IAfterShieldGained
{
    public Task AfterShieldGained(ICombatState combatState, Player player);
}