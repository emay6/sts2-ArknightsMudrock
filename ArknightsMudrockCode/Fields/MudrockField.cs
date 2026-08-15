#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Fields;

public static class MudrockField
{
        public static readonly SpireField<PlayerCombatState, PlayerCombatStateExtension.ShieldCombatState>
                ShieldCombatState = new(() => null);
}