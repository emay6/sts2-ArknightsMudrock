using ArknightsMudrock.ArknightsMudrockCode.Powers;
using ArknightsMudrock.ArknightsMudrockCode.Relics;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace ArknightsMudrock.ArknightsMudrockCode.Relics;

public class MercenaryBadge() : ArknightsMudrockRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MomentumPower>(1)];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            Flash();
            await PowerCmd.Apply<MomentumPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
                DynamicVars.Power<MomentumPower>().BaseValue, Owner.Creature, null);
        }
    }
}